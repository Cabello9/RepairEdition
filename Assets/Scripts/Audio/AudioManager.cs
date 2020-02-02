using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio sources")]
    public AudioSource backgroundAudioSource;
    public AudioSource sfxAudioSource;
    public AudioSource jumpAudioSource;
    public AudioSource crashAudioSource;

    [Header("Clips")]
    public AudioClip winSound;
    public AudioClip jumpSound;
    public AudioClip stompSound;
    public AudioClip buttonSound;
    public AudioClip beepSound;
    public AudioClip boopSound;
    public AudioClip solidSnakeSound;
    public AudioClip gnomeTalkSound;
    public AudioClip crashSound;

    
    #region Music

    public void PlayBackgroundMusic()
    {
        backgroundAudioSource.Play();
    }

    public void StopBackgroundMusic()
    {
        backgroundAudioSource.Stop();
    }

    #endregion

    #region SFX

    public void PlayWinSound()
    {
        sfxAudioSource.PlayOneShot(winSound);
    }

    public void PlayJumpSound()
    {
        jumpAudioSource.Play();
    }

    public void PlayStompSound()
    {
        sfxAudioSource.PlayOneShot(stompSound);
    }

    public void PlayBoopSound()
    {
        sfxAudioSource.PlayOneShot(boopSound);
    }

    public void PlaySolidSnakeSound()
    {
        sfxAudioSource.PlayOneShot(solidSnakeSound);
    }

    public void PlayCrashSound()
    {
        crashAudioSource.PlayOneShot(crashSound);
    }

    public void PlayGnomeTalkSound()
    {
        sfxAudioSource.PlayOneShot(gnomeTalkSound);
    }

    #endregion

    #region UI

    public void PlayButtonSound()
    {
        sfxAudioSource.PlayOneShot(buttonSound);
    }

    public void PlayBeepSound()
    {
        sfxAudioSource.PlayOneShot(beepSound);
    }

    #endregion

}
