using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class PortalParticleSystem : MonoBehaviour {

	public float rotationSpeed = 16;
	public float speedToCenter = 1;

	private new ParticleSystem particleSystem;

	void Awake () {
		particleSystem = GetComponent<ParticleSystem>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
		var particleCount = particleSystem.GetParticles(particles);

		for (int i = 0; i < particleCount; i++) {
			var life = particles[i].remainingLifetime / particles[i].startLifetime;
			var distance = Vector3.Distance(particles[i].position, transform.position);
			var normalToCenter = -particles[i].position.normalized * speedToCenter;

			var p1 = new Vector3(Mathf.Cos(Time.time + i), Mathf.Sin(Time.time + i)) * distance * life;
			var p2 = new Vector3(Mathf.Cos(Time.time + i + 0.01f), Mathf.Sin(Time.time + i + 0.01f)) * distance * life + normalToCenter;

			particles[i].velocity += (p2 - p1).normalized * Time.deltaTime * rotationSpeed;
		}

		particleSystem.SetParticles(particles, particleCount);
	}
}
