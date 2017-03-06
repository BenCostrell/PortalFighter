using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject pickupPrefab;
	private GameObject player1;
	private GameObject player2;
	public Vector3 pickupSpawn;
	public Vector3 spawnPoint_P1;
	public Vector3 spawnPoint_P2;
	public GameObject livesUI_P1;
	public GameObject livesUI_P2;
	public GameObject gameOverUI;
	private int lives_P1;
	private int lives_P2;
	public int startingLives;
	public int frameCount;
	public int framesBeforeCheckingForDeath;
	public bool gameOver;
	public float timeBeforeLifeRespawn;

	// Use this for initialization
	void Start () {
		InitializePlayers ();
		frameCount = 0;
		gameOverUI.SetActive (false);
		gameOver = false;
		SpawnExtraLife ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Reset")) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		}
	}

	void FixedUpdate(){
		frameCount += 1;
	}

	void InitializePlayers(){
		player1 = InitializePlayer (1);
		player2 = InitializePlayer (2);
		lives_P1 = startingLives;
		lives_P2 = startingLives;
		UpdateLivesUI ();
	}

	GameObject InitializePlayer(int playerNum){
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
		return player;
	}


	public void PlayerDeath(int playerNum){
		if (playerNum == 1) {
			lives_P1 -= 1;
			if (lives_P1 == 0) {
				GameOver (playerNum);
			} else {
				RespawnPlayer (playerNum);
			}
		} else if (playerNum == 2) {
			lives_P2 -= 1;
			if (lives_P2 == 0) {
				GameOver (playerNum);
			} else {
				RespawnPlayer (playerNum);
			}
		}
		UpdateLivesUI ();
	}

	void RespawnPlayer(int playerNum){
		GameObject player;
		Vector3 spawnPoint;
		if (playerNum == 1) {
			player = player1;
			spawnPoint = spawnPoint_P1;
		}
		else {
			player = player2;
			spawnPoint = spawnPoint_P2;
		}
		player.transform.position = spawnPoint;
		player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
	}

	void UpdateLivesUI(){
		livesUI_P1.GetComponent<Text> ().text = lives_P1.ToString();
		livesUI_P2.GetComponent<Text> ().text = lives_P2.ToString();
	}

	void GameOver(int playerNum){
		int winningPlayerNum = 3 - playerNum;
		gameOverUI.SetActive (true);
		gameOverUI.GetComponent<Text> ().text = "PLAYER " + winningPlayerNum + " WINS";
		gameOver = true;
	}

	public void GetExtraLife(int playerNum){
		if (playerNum == 1) {
			lives_P1 += 1;
		} else if (playerNum == 2) {
			lives_P2 += 1;
		}
		UpdateLivesUI ();
		Invoke ("SpawnExtraLife", timeBeforeLifeRespawn);
	}

	void SpawnExtraLife(){
		Instantiate (pickupPrefab, pickupSpawn, Quaternion.identity);
	}
}
