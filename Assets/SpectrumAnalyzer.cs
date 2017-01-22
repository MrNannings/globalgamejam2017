using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumAnalyzer : MonoBehaviour {

	private int sampleCount = 256;
	private List<Transform> cubes = new List<Transform>();
	private float[] samples;
	private float[] maximums;

	private AudioSource source;

	// Use this for initialization
	void Start () {
		source = GameObject.Find("MusicOut Bass").GetComponent<AudioSource>();

		for (int i = 0; i < sampleCount; i++) {
			var cube = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
			cube.position = new Vector3(i / 10f - sampleCount / 100f, 0, 0);
			cube.localScale = Vector3.one / 10f;

			cubes.Add(cube);
		}

		maximums = new float[sampleCount];
	}
	
	// Update is called once per frame
	void Update () {
		samples = new float[sampleCount];

		source.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);

		for (int i = 0; i < sampleCount; i++) {
			if (false) {
				cubes[i].position = new Vector3(cubes[i].position.x, samples[i] * 6);
			}
			else {
				cubes[i].position = new Vector3(cubes[i].position.x, samples[i] * 3);
			}

			if (samples[i] > maximums[i]) {
				maximums[i] = samples[i];
			}
		}
	}
}
