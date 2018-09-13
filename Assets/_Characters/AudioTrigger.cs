using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour {

    [SerializeField]    AudioClip clip;
    [SerializeField] float triggerRadius = 5f;
    [SerializeField] bool isOneTimeOnly = true;
    [SerializeField] bool hasPlayed = false;

    GameObject layerFilter;

    AudioSource audioSource;
    // Use this for initialization
    void Start () {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = clip;

        SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = triggerRadius;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
	}
	
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == layerFilter)
        {
            RequestPlayAudioClip();
        }
    }

    private void RequestPlayAudioClip()
    {
        if (isOneTimeOnly && hasPlayed)
        {
            return;
        }
        else if (audioSource.isPlaying == false)
        {
            audioSource.Play();
        }
        throw new NotImplementedException();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
