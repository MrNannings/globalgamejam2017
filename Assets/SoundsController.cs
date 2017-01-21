using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsController : MonoBehaviour {

    public AudioClip[] soundClips;
    private AudioSource audioSource;
    private int lastPlayedNumber;

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
        if (!audioSource.isPlaying)
        {
            if(lastPlayedNumber != 3) audioSource.PlayOneShot(soundClips[soundsNumber]);
            lastPlayedNumber = soundsNumber;
        }
        
    }
}
