using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusWave : MonoBehaviour {

	public static SinusWave Instance;

	public float amplitude;
	public float frequency;
	public Vector2 offset;

	void Awake () {
		if (Instance != null) {
			Debug.LogError("SinusWave instance already exists");
		}

		Instance = this;
	}

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {
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
		return Mathf.Sin(x * frequency) * amplitude;
	}

	public float GetValue (Vector2 position) {
		return GetValue(position.x);
	}
}
