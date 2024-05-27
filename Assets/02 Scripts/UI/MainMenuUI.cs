using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Main Menu UI")]
    public GameObject MainMenu;

    public GameObject SleepFade;

    #region "Main Menu"
    public void StartGame()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("Menu",false);
        StartCoroutine(FadeEffect(1f));
        MainMenu.SetActive(false);
        SceneManager.LoadScene("Scene01");
    }

    public void ExitGame()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("Menu",false);
        // Si estamos en el editor de Unity, detener el juego
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // Si estamos en una build, cerrar la aplicación
        Application.Quit();
        #endif
    }
    #endregion

    IEnumerator FadeEffect(float time)
    {
        SleepFade.SetActive(true);
        yield return new WaitForSeconds(time);
        SleepFade.SetActive(false);
    }
}
