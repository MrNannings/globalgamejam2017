﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D () {
		GameObject.Find("Sounds Controller").GetComponent<SoundsController>().PlaySound(16);

		App.LoadNextLevel();
	}
}