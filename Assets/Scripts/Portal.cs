using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

	public Portal partnerPortal;
	public float simulatedFallSpeed;
	public float lockoutTime;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collider){
		GameObject go = collider.gameObject;
		if (go.tag == "Player") {
			if ((partnerPortal != null) && go.GetComponent<PlayerController>().timeLockedOutOfPortals <= 0){
				Rigidbody2D rb = go.GetComponent<Rigidbody2D> ();
				float portalRot = transform.rotation.eulerAngles.z;
				float rotationDifference = (partnerPortal.transform.rotation.eulerAngles.z - portalRot + 180) * Mathf.Deg2Rad;
				if (portalRot == 0){
					rb.velocity += simulatedFallSpeed * Vector2.down;
				}
				float playerVelocityAngle = Mathf.Atan2 (rb.velocity.y, rb.velocity.x);
				float playerVelocityMagnitude = rb.velocity.magnitude;
				Vector3 rotatedVelocity = new Vector3 (Mathf.Cos (playerVelocityAngle + rotationDifference) * playerVelocityMagnitude,
					                          Mathf.Sin (playerVelocityAngle + rotationDifference) * playerVelocityMagnitude);
				go.transform.position = partnerPortal.transform.position;
				rb.velocity = rotatedVelocity;
				go.GetComponent<PlayerController> ().timeLockedOutOfPortals = lockoutTime;
			}
		}
	}
}
