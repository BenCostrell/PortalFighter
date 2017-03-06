using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public int playerNum;
	public float accel;
	public float aerialDrift;
	private Rigidbody2D rb;
	public float groundDetectionDistance;
	public LayerMask groundLayer;
	public LayerMask surfaceLayer;
	public float jumpForce;
	private Transform gunPivot;
	private PortalManager portalManager;
	public float timeLockedOutOfPortals;
	private Portal portalReticle;
	public float bumpPower;
	private GameManager gameManager;
	public float postShotFreezeTime;
	private float postShotFreezeTimer;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		gunPivot = transform.GetChild (0);
		portalManager = GameObject.FindGameObjectWithTag ("PortalManager").GetComponent<PortalManager>();
		gameManager = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager>();
	}


	void Update(){
		if (timeLockedOutOfPortals > 0) {
			timeLockedOutOfPortals -= Time.deltaTime;
		}
	}
	// Update is called once per frame
	void FixedUpdate () {
		if (!gameManager.gameOver) {
			if (gameManager.frameCount > gameManager.framesBeforeCheckingForDeath) {
				CheckIfDead ();
			}
			if (postShotFreezeTimer > 0) {
				postShotFreezeTimer -= Time.deltaTime;
			} else {
				ProcessInput ();
			}
		}
	}

	void ProcessInput(){
		float moveY = Input.GetAxis ("Vertical_P" + playerNum);
		float moveX = Input.GetAxis ("Horizontal_P" + playerNum);
		Vector2 moveVector = new Vector2 (moveX, moveY);
		Rotate (moveVector);
		if (!Input.GetButton ("Shoot_P" + playerNum) && (moveVector.magnitude > 0.1f)) {
			if (IsGrounded ()) {
				GroundMove (moveX);
			} else {
				AerialDrift (moveX);
			}
		}
		if (IsGrounded () && Input.GetButtonDown ("Jump_P" + playerNum)) {
			Jump ();
		}
			
		if (Input.GetButton ("Shoot_P" + playerNum)) {
			if (portalReticle == null) {
				SpawnReticle ();
			} else {
				MoveReticle ();
			}
		}
		else if (portalReticle != null) {
			Shoot ();
		}
	}

	void GroundMove(float x){
		Accelerate (accel, x * Vector2.right);
	}

	void AerialDrift(float x){
		Accelerate (aerialDrift, x * Vector2.right);
	}

	void Rotate(Vector2 inputVector){
		float angle = Mathf.Atan2 (inputVector.y, inputVector.x);
		gunPivot.rotation = Quaternion.Euler (0, 0, angle * Mathf.Rad2Deg);
	}
		
	void Accelerate(float acceleration, Vector2 inputVector){
		Vector2 force = acceleration * inputVector.normalized;
		rb.AddForce (force);
	}

	void Jump(){
		rb.velocity = jumpForce * Vector2.up;
	}

	void SpawnReticle(){
		float zRot = gunPivot.rotation.eulerAngles.z * Mathf.Deg2Rad;
		Vector2 aimVector = new Vector2 (Mathf.Cos (zRot), Mathf.Sin (zRot));
		RaycastHit2D hit = Physics2D.Raycast (transform.position, aimVector, Mathf.Infinity, surfaceLayer);
		Vector2 spawnPoint = new Vector2 (1000, 1000);
		Vector3 spawnRotation = Vector3.zero;
		bool onSurface = false;
		if (hit) {
			spawnRotation = hit.transform.gameObject.GetComponent<Surface> ().surfaceRotation;
			spawnPoint = hit.point;
			onSurface = true;
		}
		portalReticle = portalManager.GenerateReticle (playerNum, spawnPoint, spawnRotation);
		portalReticle.onSurface = onSurface;
	}

	void MoveReticle(){
		float zRot = gunPivot.rotation.eulerAngles.z * Mathf.Deg2Rad;
		Vector2 aimVector = new Vector2 (Mathf.Cos (zRot), Mathf.Sin (zRot));
		RaycastHit2D hit = Physics2D.Raycast (transform.position, aimVector, Mathf.Infinity, surfaceLayer);
		Vector2 spawnPoint = new Vector2 (1000, 1000);
		Vector3 spawnRotation = Vector3.zero;
		if (hit) {
			spawnRotation = hit.transform.gameObject.GetComponent<Surface> ().surfaceRotation;
			spawnPoint = hit.point;
			portalReticle.onSurface = true;
		} else {
			portalReticle.onSurface = false;
		}
		portalReticle.transform.position = spawnPoint;
		portalReticle.transform.rotation = Quaternion.Euler (spawnRotation);
	}

	void Shoot(){
		if (portalReticle.onSurface) {
			portalManager.ActivatePortal (portalReticle, playerNum);
		} else {
			Destroy (portalReticle);
		}
		portalReticle = null;

		postShotFreezeTimer = postShotFreezeTime;
	}

	bool IsGrounded(){
		return Physics2D.Raycast (transform.position, Vector2.down, groundDetectionDistance, groundLayer);
	}

	void OnTriggerEnter2D(Collider2D collider){
		GameObject playerObj = collider.gameObject;
		if (playerObj.tag == "Player") {
			Rigidbody2D otherRb = playerObj.GetComponent<Rigidbody2D> ();
			if (otherRb.velocity.magnitude < rb.velocity.magnitude){
				otherRb.AddForce (bumpPower * rb.velocity);
				rb.velocity = Vector3.zero;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D collision){
		GameObject obj = collision.collider.gameObject;
		if (obj.tag == "Pickup") {
			Destroy (obj);
			gameManager.GetExtraLife (playerNum);
		}
	}

	void CheckIfDead(){
		if (!GetComponent<SpriteRenderer> ().isVisible) {
			Die ();
		}
	}

	public void Die(){
		gameManager.PlayerDeath (playerNum);

	}
}
