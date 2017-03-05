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

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		gunPivot = transform.GetChild (0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		ProcessInput ();
	}

	void ProcessInput(){
		float y = Input.GetAxis ("Vertical_P" + playerNum);
		float x = Input.GetAxis ("Horizontal_P" + playerNum);
		Vector2 inputVector = new Vector2 (x, y);
		if (inputVector.magnitude > 0.1f) {
			Rotate (inputVector);
			if (IsGrounded ()) {
				GroundMove (x);
			} else {
				AerialDrift (x);
			}
		}
		if (IsGrounded () && Input.GetButtonDown ("Jump_P" + playerNum)) {
			Jump ();
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

	bool IsGrounded(){
		return Physics2D.Raycast (transform.position, Vector2.down, groundDetectionDistance, groundLayer);
	}

}
