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
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    }

	private void OnDrawGizmos () {
		for (float i = -100; i < 100; i += 0.1f) {
			Gizmos.DrawLine(new Vector3(i, Mathf.Sin(i)), new Vector3(i + 0.1f, Mathf.Sin(i + 0.1f)));
		}

		var positionOnSinus = new Vector3(transform.position.x, Mathf.Sin(transform.position.x));

		if (Vector3.Distance(transform.position, positionOnSinus) < 0.5f) {
			Gizmos.color = Color.yellow;
			Gizmos.DrawCube(positionOnSinus, Vector3.one * 0.2f);
			Gizmos.color = Color.white;
        }

		Gizmos.DrawCube(positionOnSinus, Vector3.one * 0.1f);
	}

	public float GetValue (Vector2 position) {
		return Mathf.Sin(position.x * frequency) * amplitude;
	}
}
