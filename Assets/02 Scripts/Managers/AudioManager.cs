using System.Collections;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region "Singleton"
    public static AudioManager Instance { get; private set; } // Singleton instance

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // If not, set it to this instance
            DontDestroyOnLoad(gameObject); // Make this instance persist across scenes
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Destroy this instance if another instance already exists
        }
    }
    #endregion

    public Sound[] musicSounds, sfxSounds, sfxPlayerSounds;

    public AudioSource musicSource, sfxSource, sfxPlayerSource;

    public PlayerController player;

    void Update()
    {
        if (player != null)
        switch (player.GetCurrentState)
        {
            case PlayerController.PlayerStates.WALK:
                PlaySFX("Walk", true);
                break;
            case PlayerController.PlayerStates.RUN:
                PlaySFX("Run", true);
                break;
            case PlayerController.PlayerStates.TILING:
                PlaySFX("Tiling", true);
                break;
            case PlayerController.PlayerStates.WATERING:
                PlaySFX("Watering", true);
                break;
        }

    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.soundName == name);

        if (s!= null)
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name, bool player)
    {
        if (!player)
        {
            Sound s = Array.Find(sfxSounds, x => x.soundName == name);

            if (s != null)
            {
                sfxSource.clip = s.clip;
                if (!sfxSource.isPlaying)
                    sfxSource.Play();
            }

            else MyDebugLog.Instance.MyDebugFunc("SFX not found in sfxSounds: ",name);
        }
        else
        {
            Sound s = Array.Find(sfxPlayerSounds, x => x.soundName == name);

            if (s != null)
            {
                sfxSource.clip = s.clip;
                if (!sfxSource.isPlaying)
                    sfxSource.Play();
            }

            else MyDebugLog.Instance.MyDebugFunc("SFX not found in sfxPlayerSounds: ",name);
        }
    }

    #region  "Audio Controls"
    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
        sfxPlayerSource.mute = !sfxPlayerSource.mute;
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
        sfxPlayerSource.volume = volume;
    }
    #endregion
}
