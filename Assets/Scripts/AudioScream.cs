using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScream : MonoBehaviour
{
     [SerializeField]
    private AudioSource audioNearScreamer;

    public void PlayAudio()
    {
        audioNearScreamer.Play();
    }

    public void StopAudio()
    {
        audioNearScreamer.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        audioNearScreamer.Play();    
    }

    private void OnTriggerExit(Collider other) 
    {
        audioNearScreamer.Stop();    
    }
}
