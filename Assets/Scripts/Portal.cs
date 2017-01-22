using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using GlobalGameJam2017;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour {

	private Stopwatch levelEndTimer;

	// Use this for initialization
	void Start () {
		transform.position = GameObject.Find("endposition").GetComponent<BoxCollider2D>().bounds.center + Vector3.back;
	}
	
	// Update is called once per frame
	void Update () {
		if (levelEndTimer != null && levelEndTimer.ElapsedMilliseconds > 500) {
			App.LoadNextLevel();
		}
	}

	void OnTriggerEnter2D () {
		GameObject.Find("Sounds Controller").GetComponent<SoundsController>().PlaySound(16);

		PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name, ScoreManager.Instance.timerLevel);

		levelEndTimer = Stopwatch.StartNew();

		GameObject.Find("Player").GetComponent<ColorTester>().shrinkAnimation = true;
	}
}
