using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class PortalParticleSystem : MonoBehaviour {

	private ParticleSystem particleSystem;

	void Awake () {
		particleSystem = GetComponent<ParticleSystem>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//var particles = particleSystem.GetParticles();
	}
}
