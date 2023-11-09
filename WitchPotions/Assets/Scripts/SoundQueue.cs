using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundQueue : MonoBehaviour
{
    public AudioSource audioSource;
    [SerializeField] AudioClip SoundSourceA;
    [SerializeField] AudioClip SoundSourceB;
    Queue<AudioClip> clipQueue = new Queue<AudioClip>();

    private void Start()
    {
        clipQueue.Enqueue(SoundSourceA);
    }
    void Update()
    {
        if (audioSource.isPlaying == false && clipQueue.Count > 0)
        {
            audioSource.clip = clipQueue.Dequeue();
            audioSource.Play();
            PlaySound(SoundSourceB);
        }
    }
    void PlaySound(AudioClip clip)
    {
        clipQueue.Enqueue(clip);
    }
}
