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
	public float jumpForce;
	private Transform gunPivot;
	private PortalManager portalManager;
	public float timeLockedOutOfPortals;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		gunPivot = transform.GetChild (0);
		portalManager = GameObject.FindGameObjectWithTag ("PortalManager").GetComponent<PortalManager>();
	}


	void Update(){
		if (timeLockedOutOfPortals > 0) {
			timeLockedOutOfPortals -= Time.deltaTime;
		}
	}
	// Update is called once per frame
	void FixedUpdate () {
		ProcessInput ();
	}

	void ProcessInput(){
		float moveY = Input.GetAxis ("Vertical_P" + playerNum);
		float moveX = Input.GetAxis ("Horizontal_P" + playerNum);
		Vector2 moveVector = new Vector2 (moveX, moveY);
		if (Input.GetButton ("Shoot_P" + playerNum)) {
			Rotate (moveVector);
		} else if (moveVector.magnitude > 0.1f) {
			if (IsGrounded ()) {
				GroundMove (moveX);
			} else {
				AerialDrift (moveX);
			}
		}
		if (IsGrounded () && Input.GetButtonDown ("Jump_P" + playerNum)) {
			Debug.Log ("jumping");
			Jump ();
		}

		if (Input.GetButtonUp ("Shoot_P" + playerNum)) {
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

	void Shoot(){
		float zRot = gunPivot.rotation.eulerAngles.z * Mathf.Deg2Rad;
		Vector2 aimVector = new Vector2 (Mathf.Cos (zRot), Mathf.Sin (zRot));
		Debug.DrawRay (transform.position, aimVector * 100, Color.blue, 2f);
		RaycastHit2D hit = Physics2D.Raycast (transform.position, aimVector, Mathf.Infinity, groundLayer);
		if (hit) {
			Vector3 surfaceRotation = hit.transform.gameObject.GetComponent<Surface> ().surfaceRotation;
			portalManager.GeneratePortal (playerNum, hit.point, surfaceRotation);
		}
	}

	bool IsGrounded(){
		return Physics2D.Raycast (transform.position, Vector2.down, groundDetectionDistance, groundLayer);
	}

}
