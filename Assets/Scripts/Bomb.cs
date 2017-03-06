using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D collision){
		GameObject obj = collision.collider.gameObject;
		if (obj.tag == "Player") {
			obj.GetComponent<PlayerController> ().Die ();
		}
		Destroy (gameObject);
	}
}
