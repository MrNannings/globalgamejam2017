using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SinusWave : MonoBehaviour {

	public static SinusWave Instance;

	public float amplitude;
	public float frequency;
	public Vector2 offset;
	public float amplitudeAnimationStrength = 0.2f;
	public float frequencyAnimationStrength = 0.2f;
	public AnimationCurve curve;

	private Transform levelStart;
	private Transform levelEnd;
	private AnalyzeSound bassLine;
	private AnalyzeSound kickLine;
	private float frequencyAnimationTime = -1;
	private float amplitubeAnimationTime = -1;

	void Awake () {
		if (Instance != null) {
			Debug.LogError("SinusWave instance already exists");
		}

		Instance = this;

		levelStart = GameObject.Find("Level Start").transform;
		levelEnd = GameObject.Find("Level End").transform;
		bassLine = GameObject.Find("MusicOut Bass").GetComponent<AnalyzeSound>();
		kickLine = GameObject.Find("MusicOut Kick").GetComponent<AnalyzeSound>();
	}

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {
		SetAnimationTime();
    }

	public float Distance (Vector2 position) {
		return Vector2.Distance(position, GetProjectedPosition(position));
	}

	public Vector3 GetProjectedPosition (Vector2 position) {
		return new Vector3(position.x, GetValue(position));
	}

	public Vector3 GetDirection (Vector2 position) {
		var p1 = new Vector2(position.x - 0.0001f, GetValue(position.x - 0.0001f));
		var p2 = new Vector2(position.x + 0.0001f, GetValue(position.x + 0.0001f));
		return (p2 - p1).normalized;
	}

	public float GetValue (float x) {
		var amplitudeAnimation = curve.Evaluate(amplitubeAnimationTime) * amplitudeAnimationStrength;
		var frequencyAnimation = curve.Evaluate(frequencyAnimationTime) * frequencyAnimationStrength;

		return Mathf.Sin(x * (frequency + frequencyAnimation) + offset.x) * (amplitude + amplitudeAnimation) + offset.y;
	}

	public float GetValue (Vector2 position) {
		return GetValue(position.x);
	}

	private float AnimationStrengthByMap (float x) {
		var length = levelEnd.position.x - levelStart.position.x;
		var normalized = x / length;

		if (normalized > 0.5f) {
			normalized = 0.5f - (normalized - 0.5f);
		}

		return normalized * 2;
	}

	private void SetAnimationTime () {
		float[] samples = new float[256];

		bassLine.GetComponent<AudioSource>().GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);

		var total = 0f;
		for (int i = 0; i < 10; i++) {
			total += samples[i];
		}
		var average = total / 10;

		if (average > 0.02f && frequencyAnimationTime == -1) {
			frequencyAnimationTime = 0;
		}
		if (kickLine.PitchValue > 50.0f && amplitubeAnimationTime == -1) {
			amplitubeAnimationTime = 0;
		}

//		if (bassLine.PitchValue > 200 && frequencyAnimationTime == -1) {
//			frequencyAnimationTime = 0;
//		}
//		if (bassLine.PitchValue > 100 && amplitubeAnimationTime == -1) {
//			amplitubeAnimationTime = 0;
//		}

		if (frequencyAnimationTime >= 0) {
			frequencyAnimationTime += Time.deltaTime;
		}
		if (amplitubeAnimationTime >= 0) {
			amplitubeAnimationTime += Time.deltaTime;
		}

		if (frequencyAnimationTime > curve.keys.Last().time) {
			frequencyAnimationTime = -1;
		}
		if (amplitubeAnimationTime > curve.keys.Last().time) {
			amplitubeAnimationTime = -1;
		}
	}
}
