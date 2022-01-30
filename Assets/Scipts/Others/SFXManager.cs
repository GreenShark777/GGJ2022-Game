using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{

    private static AudioSource sfxSource;


    private void Awake()
    {

        sfxSource = GetComponent<AudioSource>();

    }

    public static void PlayNewSFX(AudioClip newSFX)
    {

        sfxSource.clip = newSFX;

        sfxSource.Play();

    }

}
