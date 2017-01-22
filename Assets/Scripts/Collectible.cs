using System.Collections;
using System.Collections.Generic;
using GlobalGameJam2017;
using UnityEngine;

public class Collectible : MonoBehaviour {

	public Transform[] visuals;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		foreach (var visual in visuals) {
			visual.Rotate(Vector3.up, Time.deltaTime * 90f);
		}
	}

	private void OnTriggerEnter2D (Collider2D collider) {
		if (collider.tag == "Player") {
			GameObject.Find("Player").GetComponent<ColorTester>().Collect(this);

			Destroy(gameObject);
		}
	}
}
