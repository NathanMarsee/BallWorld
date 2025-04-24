using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{

    // Public Variables (set in the Inspector)
    public AudioClip soundToPlay;        // The sound to play on trigger
    [SerializeField] private string targetObjectName = "Player"; // The name of the object that triggers the sound (e.g., "Player")
    public float destroyDelay = 0f;      // Delay before destroying the trigger

    //private AudioSource audioSource;     // Reference to the AudioSource component - REMOVED


    void Start()
    {
        // Add an AudioSource component to the GameObject, if one doesn't exist - MOVED
        //audioSource = GetComponent<AudioSource>();
        //if (audioSource == null)
        //{
        //    audioSource = gameObject.AddComponent<AudioSource>();
        //}
        //audioSource.playOnAwake = false;
        //audioSource.loop = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == targetObjectName)
        {
            // Play the sound
            if (soundToPlay != null)
            {
                //GameObject tempGO = new GameObject("TempAudioSource"); //create a temp audio source
                //AudioSource tempAS = tempGO.AddComponent<AudioSource>(); //add an audio source
                //tempAS.clip = soundToPlay; //set the clip
                //tempAS.Play(); //play the sound
                //Destroy(tempGO, soundToPlay.length); //destroy the temp object after the sound has played
                AudioSource.PlayClipAtPoint(soundToPlay, transform.position); // Shorter version of the above

            }

            // Destroy the trigger object
            Destroy(gameObject, destroyDelay);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == targetObjectName)
        {
            // Play the sound
            if (soundToPlay != null)
            {
                //GameObject tempGO = new GameObject("TempAudioSource");
                //AudioSource tempAS = tempGO.AddComponent<AudioSource>();
                //tempAS.clip = soundToPlay;
                //tempAS.Play();
                //Destroy(tempGO, soundToPlay.length);
                AudioSource.PlayClipAtPoint(soundToPlay, transform.position); // Shorter version

            }

            // Destroy the trigger object
            Destroy(gameObject, destroyDelay);
        }
    }
}