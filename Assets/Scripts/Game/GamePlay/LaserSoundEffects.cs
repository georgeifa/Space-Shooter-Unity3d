using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSoundEffects : MonoBehaviour
{

    public AudioClip activationClip;
    public AudioClip shutDownClip;
    public AudioSource source;

    public void PlayActivation()
    {
        source.clip = activationClip;
        source.Play();

    }

    public void PlayShutDown()
    {
        source.clip = shutDownClip;
        source.Play();

    }

}
