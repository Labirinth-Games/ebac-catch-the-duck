using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [Header("References")]
    public AudioSource audioSource;

    [Header("Settings")]
    public List<AudioSFX> sfxs;

    public void Play(SFXType sfxType)
    {
        AudioClip clip = sfxs.Find(i => i.sfxType == sfxType).sfx;

        if(clip != null)
            audioSource?.PlayOneShot(clip);
    }

    private void OnValidate()
    {
        if (audioSource != null)
            audioSource = GetComponent<AudioSource>();
    }

}

public enum SFXType
{
    DEAD,
    PUNCH,
    JUMP_DOWN
}

[Serializable]
public class AudioSFX
{
    public SFXType sfxType;
    public AudioClip sfx;
}
