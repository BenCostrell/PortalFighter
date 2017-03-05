using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public GameObject playerPrefab;
	public Vector3 spawnPoint_P1;
	public Vector3 spawnPoint_P2;

	// Use this for initialization
	void Start () {
		InitializePlayers ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Reset")) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		}
	}

	void InitializePlayers(){
		InitializePlayer (1);
		InitializePlayer (2);
	}

	void InitializePlayer(int playerNum){
		GameObject player = Instantiate (playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		player.GetComponent<PlayerController> ().playerNum = playerNum;
		SpriteRenderer sr = player.GetComponent<SpriteRenderer> ();
		SpriteRenderer gunSr = player.GetComponentsInChildren<SpriteRenderer> () [1];
		if (playerNum == 1) {
			sr.color = Color.red;
			gunSr.color = Color.red;
			player.transform.position = spawnPoint_P1;
		} else {
			sr.color = Color.green;
			gunSr.color = Color.green;
			player.transform.position = spawnPoint_P2;
		}
	}
}
