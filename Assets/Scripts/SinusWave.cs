using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SinusWave : MonoBehaviour {

	public static SinusWave Instance;

	public float amplitude;
	public float frequency;
	public float amplitudeAnimationStrength = 0.2f;
	public float frequencyAnimationStrength = 0.2f;
	public AnimationCurve curve;
	public AnimationCurve centerEffectCurve;
	public GameObject touchParticle;
	public Transform levelStart;
	public Transform levelEnd;

	private Vector2 offset;
	private AnalyzeSound bassLine;
	private AnalyzeSound kickLine;
	private SoundsController soundsController;
	private float frequencyAnimationTime = -1;
	private float amplitubeAnimationTime = -1;
	private List<SinusWaveNode> touchNodes = new List<SinusWaveNode>();

	private NodePair lastTouchPair;
	
	void Awake () {
		if (Instance != null) {
			Debug.LogError("SinusWave instance already exists");
		}

		Instance = this;

		bassLine = GameObject.Find("MusicOut Bass").GetComponent<AnalyzeSound>();
		kickLine = GameObject.Find("MusicOut Kick").GetComponent<AnalyzeSound>();
		soundsController = GameObject.Find("Sounds Controller").GetComponent<SoundsController>();
	}

	// Use this for initialization
	void Start () {
		var middleline = GameObject.Find("middleLine").GetComponent<BoxCollider2D>();

		levelStart.transform.position = new Vector3(middleline.bounds.min.x, middleline.bounds.center.y);
		levelEnd.transform.position = new Vector3(middleline.bounds.max.x, middleline.bounds.center.y);
		offset = levelStart.transform.position;
	}
	
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
		var amplitudeAnimation = curve.Evaluate(amplitubeAnimationTime) * amplitudeAnimationStrength * AnimationStrengthByMap(x);
		var frequencyAnimation = curve.Evaluate(frequencyAnimationTime) * frequencyAnimationStrength * AnimationStrengthByMap(x);

		return Mathf.Sin(x * (frequency + frequencyAnimation) + offset.x) * (amplitude + amplitudeAnimation) + offset.y;
	}

	public float GetValue (Vector2 position) {
		return GetValue(position.x);
	}

	public bool IsTouching (Vector2 position) {
		var positionOnSinus = new Vector3(position.x, GetValue(position));

		return Vector3.Distance(position, positionOnSinus) < 0.5f;
	}

	public void Touch (Vector2 position) {
		var positionOnSinus = new Vector3(position.x, GetValue(position));

		ShowTouchParticle(positionOnSinus);
		soundsController.PlaySound(2);

		if (lastTouchPair == null) {
			lastTouchPair = new NodePair(new SinusWaveNode(0, 0, position));
		}
		else if (lastTouchPair.node2 == null) {
			
		}
	}

	public void ShowTouchParticle(Vector3 position)
    {
        if(!touchParticle.GetComponent<ParticleSystem>().isPlaying)
        {
            touchParticle.GetComponent<ParticleSystem>().Clear();
            touchParticle.transform.position = position;
            touchParticle.GetComponent<ParticleSystem>().Play();
        }
	}

	private float AnimationStrengthByMap (float x) {
		var localized = x - levelStart.position.x;
		var length = levelEnd.position.x - levelStart.position.x;

		return centerEffectCurve.Evaluate(localized / length);
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

	private class NodePair {

		public SinusWaveNode node1, node2;

		public NodePair (SinusWaveNode node1, SinusWaveNode node2 = null) {
			this.node1 = node1;
			this.node2 = node2;
		}

	}
}
