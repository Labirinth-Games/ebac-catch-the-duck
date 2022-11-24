using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public void PlaySFX(AudioSource sfx)
    {
        sfx.Play();
    }

    public void PlayMusic(AudioSource music)
    {
        music.loop = true;
        music.Play();
    }
}
