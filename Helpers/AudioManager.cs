using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip PickUpSound, DeadSound;

    void Awake()
    {
        MakeInstance();
    }

    void MakeInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlayPickUpSound()
    {
        AudioSource.PlayClipAtPoint(PickUpSound, transform.position);
    }

    public void PlayDeadSound()
    {
        AudioSource.PlayClipAtPoint(DeadSound, transform.position);
    }
}
