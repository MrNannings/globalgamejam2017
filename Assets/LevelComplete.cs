using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class LevelComplete : MonoBehaviour {

	public float fadeInTime = 0.5f;
	public float stayTime = 2.0f;
	public float fadeOutTime = 0.5f;
	public AnimationCurve curve;

	private Stopwatch timer;
	private RectTransform rectTransform;

	void Awake () {
		rectTransform = GetComponent<RectTransform>();
	}

	// Use this for initialization
	void Start () {
		rectTransform.anchoredPosition = Vector2.left * rectTransform.rect.width * 1.5f;
	}
	
	// Update is called once per frame
	void Update () {
		if (timer == null || timer.IsRunning == false) {
			return;
		}

		var time = timer.ElapsedMilliseconds / 1000f;
		var length = rectTransform.rect.width * 1.5f;

		if (time < fadeInTime) {
			
		}
	}

	public void Fire () {
		timer = Stopwatch.StartNew();
	}
}
