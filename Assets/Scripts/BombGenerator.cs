using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombGenerator : MonoBehaviour {

	public float spawnRate;
	public GameObject bombPrefab;
	public float spawnRange;

	// Use this for initialization
	void Start () {
		InvokeRepeating ("SpawnBomb", 0, spawnRate);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SpawnBomb(){
		Vector3 spawnLocation;
		float xLocation = Random.Range (-spawnRange, spawnRange);
		spawnLocation = new Vector3 (xLocation, 12, 0);
		Instantiate (bombPrefab, spawnLocation, Quaternion.identity);
	}
}
