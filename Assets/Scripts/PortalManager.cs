﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour {

	public GameObject portalPrefab;
	private bool nextPortalOrangeP1;
	private bool nextPortalOrangeP2;
	private Portal bluePortalP1;
	private Portal orangePortalP1;
	private Portal bluePortalP2;
	private Portal orangePortalP2;

	// Use this for initialization
	void Start () {
		nextPortalOrangeP1 = false;
		nextPortalOrangeP2 = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Portal GenerateReticle(int playerNum, Vector3 location, Vector3 rotation){
		GameObject portalObj = Instantiate (portalPrefab, location, Quaternion.Euler(rotation)) as GameObject;
		Portal portal = portalObj.GetComponent<Portal> ();
		SpriteRenderer sr = portalObj.GetComponent<SpriteRenderer> ();
		SpriteRenderer ringSr = portalObj.GetComponentsInChildren<SpriteRenderer> () [1];
		if (playerNum == 1) {
			if (nextPortalOrangeP1) {
				sr.color = new Color (1, 0.5f, 0, 0.5f);
			} 
			ringSr.color = new Color (1, 0, 0, 0.5f);
		} else if (playerNum == 2) {
			if (nextPortalOrangeP2) {
				sr.color = new Color (1, 0.5f, 0, 0.5f);
			}
			ringSr.color = new Color (0, 1, 0, 0.5f);
		}
		portal.active = false;
		return portal;
	}

	public void ActivatePortal(Portal portal, int playerNum){
		portal.Activate ();
		SpriteRenderer sr = portal.gameObject.GetComponent<SpriteRenderer> ();		
		SpriteRenderer ringSr = portal.gameObject.GetComponentsInChildren<SpriteRenderer> () [1];
		ringSr.color = new Color (ringSr.color.r, ringSr.color.g, ringSr.color.b, 1);
		if (playerNum == 1) {
			if (nextPortalOrangeP1) {
				if (orangePortalP1 != null) {
					Destroy (orangePortalP1.gameObject);
				}
				orangePortalP1 = portal;
				portal.partnerPortal = bluePortalP1;
				bluePortalP1.partnerPortal = portal;
			} else {
				if (bluePortalP1 != null) {
					Destroy (bluePortalP1.gameObject);
				}
				bluePortalP1 = portal;
				if (orangePortalP1 != null) {
					portal.partnerPortal = orangePortalP1;
					orangePortalP1.partnerPortal = portal;
				}
			}
			nextPortalOrangeP1 = !nextPortalOrangeP1;
		} else if (playerNum == 2) {
			if (nextPortalOrangeP2) {
				if (orangePortalP2 != null) {
					Destroy (orangePortalP2.gameObject);
				}
				orangePortalP2 = portal;
				portal.partnerPortal = bluePortalP2;
				bluePortalP2.partnerPortal = portal;
			} else {
				if (bluePortalP2 != null) {
					Destroy (bluePortalP2.gameObject);
				}				
				bluePortalP2 = portal;
				if (orangePortalP2 != null) {
					portal.partnerPortal = orangePortalP2;
					orangePortalP2.partnerPortal = portal;
				}
			}
			nextPortalOrangeP2 = !nextPortalOrangeP2;
		}
	}
}
