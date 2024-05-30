using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public float fadeDuration = 1f; // Melt sounds duration

    private Coroutine currentFadeCoroutine;

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

    #region "Player Assignation"
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignPlayer();
    }
    public void AssignPlayer()
    {
        if (player == null)
            player = FindObjectOfType<PlayerController>();
    }
    #endregion

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.soundName == name);

        if (s!= null)
        {
            musicSource.clip = s.clip;
            musicSource.Play();
            /* if (currentFadeCoroutine != null)
            {
                StopCoroutine(currentFadeCoroutine);
            }

            currentFadeCoroutine = StartCoroutine(FadeMusic(s.clip)); */
        }
    }

    private IEnumerator FadeMusic(AudioClip newClip)
    {
        // Fade out the current music
        float startVolume = musicSource.volume;
        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        // Stop the current music and switch to the new clip
        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.Play();

        // Fade in the new music
        while (musicSource.volume < startVolume)
        {
            musicSource.volume += startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        musicSource.volume = startVolume;
        currentFadeCoroutine = null;
    }


    public void PlaySFX(string name, bool player)
    {
        //MyDebugLog.Instance.MyDebugFunc("SFX: ",name);
        if (!player)
        {
            Sound s = Array.Find(sfxSounds, x => x.soundName == name);
            if (s != null)
            {
                sfxSource.clip = s.clip;
                if (!sfxSource.isPlaying)
                    sfxSource.Play();
                else if(name == "BuySell") sfxSource.Play();
            }

            else MyDebugLog.Instance.MyDebugFunc("SFX not found in sfxSounds: ",name);
        }
        else
        {
            Sound s = Array.Find(sfxPlayerSounds, x => x.soundName == name);

            if (s != null)
            {
                sfxPlayerSource.clip = s.clip;
                if (!sfxPlayerSource.isPlaying)
                    sfxPlayerSource.Play();
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
