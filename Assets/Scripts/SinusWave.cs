using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusWave : MonoBehaviour {

	public static SinusWave Instance;

	public float amplitude;
	public float frequency;
	public Vector2 offset;

	private LineRenderer lineRenderer;

	void Awake () {
		lineRenderer = GetComponent<LineRenderer>();

		if (Instance != null) {
			Debug.LogError("SinusWave instance already exists");
		}

		Instance = this;
	}

	// Use this for initialization
	void Start () {
		List<Vector3> positions = new List<Vector3>();

		for (float i = -100; i < 100; i += 0.1f) {
			positions.Add(new Vector3(i, GetValue(new Vector2(i, 0))));
		}

		lineRenderer.numPositions = positions.Count;
		lineRenderer.SetPositions(positions.ToArray());
	}
	
	// Update is called once per frame
	void Update () {
    }

	public float Distance (Vector2 position) {
		return Vector2.Distance(position, GetProjectedPosition(position));
	}

	public Vector3 GetProjectedPosition (Vector2 position) {
		return new Vector3(position.x, GetValue(position));
	}

	public float GetValue (Vector2 position) {
		return Mathf.Sin(position.x * frequency) * amplitude;
	}
}
