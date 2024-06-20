using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsUI : MonoBehaviour
{
    public void SetEn()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("Menu",false);

        if (Multilanguage.Instance != null)
            Multilanguage.Instance.CurrentLang = Multilanguage.Languages.EN;

    }
    public void SetSp()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("Menu",false);

        if (Multilanguage.Instance != null)
            Multilanguage.Instance.CurrentLang = Multilanguage.Languages.SP;
    }
}
