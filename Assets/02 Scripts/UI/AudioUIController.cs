using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioUIController : MonoBehaviour
{
    public Slider _musicSlider, _sfxSlider;

    public void ToggleMusic()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.ToggleMusic();
    }

    public void ToggleSFX()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.ToggleSFX();
    }

    public void SetMusicVolume()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetMusicVolume(_musicSlider.value);
    }

    public void SetSFXVolume()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetSFXVolume(_sfxSlider.value);
    }
}
