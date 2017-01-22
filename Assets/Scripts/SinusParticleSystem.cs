using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SinusParticleSystem : MonoBehaviour {

	public int particlesPerUnit;
	public float speed;
	public float avoidanceStrength = 1;
	public ParticleSystem scoreParticleSystem;
	public Transform[] obstacles;
	public AnimationCurve avoidanceCurve;

	private int particleCount;
	private float[] particleRandom;
	private float[] particleXPositions;
	private new ParticleSystem particleSystem;
	private ParticleSystem.Particle[] particles;
	private ParticleSystem.Particle[] scoreParticles;

	void Awake () {
		particleSystem = GetComponent<ParticleSystem>();

		transform.position += Vector3.back * 5;
	}

	// Use this for initialization
	void Start () {
		var levelLength = SinusWave.Instance.levelEnd.position - SinusWave.Instance.levelStart.position;

		particleCount = particlesPerUnit * (int)levelLength.x;

		particleRandom = new float[particleCount];
		particleXPositions = new float[particleCount];
		
		particles = new ParticleSystem.Particle[particleCount];
		scoreParticles = new ParticleSystem.Particle[particleCount];
		for (int i = 0; i < particleCount; i++) {
			var position = SinusWave.Instance.levelStart.position + levelLength * ((float)i / particleCount);

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

	// Update is called once per frame
	void Update () {
        for(int i = 0; i < particleCount; i++) {
			particleXPositions[i] += speed * Time.deltaTime;
	        if (particleXPositions[i] > SinusWave.Instance.levelEnd.position.x) {
		        var difference = particleXPositions[i] - SinusWave.Instance.levelEnd.position.x;
		        particleXPositions[i] = SinusWave.Instance.levelStart.position.x + difference;
	        }

	        var xPosition = particleXPositions[i];
	        var position = new Vector3(xPosition, SinusWave.Instance.GetValue(xPosition) + particleRandom[i]);

	        if (SinusWave.Instance.touchNodes.Any(a => Vector3.Distance(a.position, position) < 0.8f)) {
				scoreParticles[i].position = position;
		        particles[i].position = Vector3.one * 10000;
	        }
	        else {
		        particles[i].position = position;
		        scoreParticles[i].position = Vector3.one * 10000;
	        }

	        particles[i].position += new Vector3(0, Mathf.Sin(Time.time * particleRandom[i] * 15f) * 0.2f);
	        scoreParticles[i].position += new Vector3(0, Mathf.Sin(Time.time * particleRandom[i] * 15f) * 0.2f);

	        foreach (var obstacle in obstacles) {
		        var distance = 3 - Vector3.Distance(obstacle.position, particles[i].position);

		        if (distance < 0) {
			        distance = 0;
		        }

		        var avoidance = avoidanceCurve.Evaluate(distance / 3);

		        particles[i].position += AvoidVector(obstacle.position, particles[i].position) * avoidance * avoidanceStrength;
		        scoreParticles[i].position += AvoidVector(obstacle.position, scoreParticles[i].position) * avoidance * avoidanceStrength;
	        }
        }

        particleSystem.SetParticles(particles, particleCount);
		scoreParticleSystem.SetParticles(scoreParticles, particleCount);
	}

	private Vector3 AvoidVector (Vector3 position, Vector3 particle) {
		var normal = (particle - position).normalized;
		var perpendicular = new Vector2(normal.y, -normal.x);

		if (Vector2.Dot(perpendicular, Vector2.up) < 0) {
			perpendicular = -perpendicular;
		}

		var goUp = Vector3.Dot(normal, Vector3.up) > 0;
		if (goUp) {
			return perpendicular;
		}

		return -perpendicular;
	}
}
