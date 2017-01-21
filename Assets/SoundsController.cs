using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsController : MonoBehaviour {

    public AudioClip[] soundClips;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start ()
    {
        audioSource.PlayOneShot(soundClips[4]);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void PlaySound(int soundsNumber)
    {
        //audioSource.clip = soundClips[soundsNumber];
        audioSource.PlayOneShot(soundClips[soundsNumber]);
    }
}
