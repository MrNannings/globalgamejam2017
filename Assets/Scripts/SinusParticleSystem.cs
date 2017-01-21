using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusParticleSystem : MonoBehaviour {

	public int particlesPerUnit;
	public float speed;
	public Transform levelStart;
	public Transform levelEnd;
	public ParticleSystem scoreParticleSystem;

	private int particleCount;
	private float[] particleRandom;
	private float[] particleXPositions;
	private new ParticleSystem particleSystem;
	private ParticleSystem.Particle[] particles;
	private ParticleSystem.Particle[] scoreParticles;

	void Awake () {
		particleSystem = GetComponent<ParticleSystem>();
	}

	// Use this for initialization
	void Start () {
		var levelLength = levelEnd.position - levelStart.position;

		particleCount = particlesPerUnit * (int)levelLength.x;

		particleRandom = new float[particleCount];
		particleXPositions = new float[particleCount];
		
		particles = new ParticleSystem.Particle[particleCount];
		scoreParticles = new ParticleSystem.Particle[particleCount];
		for (int i = 0; i < particleCount; i++) {
			var position = levelStart.position + levelLength * ((float)i / particleCount);

			particleRandom[i] = (Random.value - 0.5f) * 0.8f;
			particleXPositions[i] = position.x;

			var size = 0.05f + Random.value * 0.1f;

			particles[i] = new ParticleSystem.Particle {
														   position = position,
														   startColor = Color.white,
														   startSize = size
													   };

			scoreParticles[i] = new ParticleSystem.Particle {
														   position = position,
														   startColor = Color.red,
														   startSize = size
													   };
		}
		particleSystem.SetParticles(particles, particleCount);
		scoreParticleSystem.SetParticles(scoreParticles, particleCount);
	}

	private bool once = false;
	
	// Update is called once per frame
	void Update () {
//		if (once)
//			return;
//
//		once = true;

        for(int i = 0; i < particleCount; i++) {
			particleXPositions[i] += speed * Time.deltaTime;
	        if (particleXPositions[i] > levelEnd.position.x) {
		        var difference = particleXPositions[i] - levelEnd.position.x;
		        particleXPositions[i] = levelStart.position.x + difference;
	        }

	        var position = particleXPositions[i];
	        if (position + particleRandom[i] * 2 > 1 && position + particleRandom[i] * 2 < 3) {
				scoreParticles[i].position = new Vector3(position, SinusWave.Instance.GetValue(position) + particleRandom[i]);
		        particles[i].position = Vector3.one * 10000;
	        }
	        else {
		        particles[i].position = new Vector3(position, SinusWave.Instance.GetValue(position) + particleRandom[i]);
		        scoreParticles[i].position = Vector3.one * 10000;
	        }

	        particles[i].position += new Vector3(0, Mathf.Sin(Time.time * particleRandom[i] * 15f) * 0.2f);
	        scoreParticles[i].position += new Vector3(0, Mathf.Sin(Time.time * particleRandom[i] * 15f) * 0.2f);
        }

        particleSystem.SetParticles(particles, particleCount);
		scoreParticleSystem.SetParticles(scoreParticles, particleCount);
	}
}
