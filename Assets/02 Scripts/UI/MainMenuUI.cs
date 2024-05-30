using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Main Menu UI")]

    public GameObject SleepFade;

    public GameObject StartGameUI;

    public GameObject NewGameUI;

    void OnEnable()
    {
        if (StartGameUI.activeSelf) StartGameUI.SetActive(false);
        if (NewGameUI.activeSelf) NewGameUI.SetActive(false);
    }
    void Update()
    {
        if ((StartGameUI.activeSelf || NewGameUI.activeSelf) && Input.GetKeyDown(KeyCode.Escape))
        {
            if (StartGameUI.activeSelf) StartGameUI.SetActive(false);
            else if (NewGameUI.activeSelf) NewGameUI.SetActive(false);
        }
    }

    void Start()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayMusic("Intro");
    }

    public void StartGame() // Play
    {
        if (DataManager.Instance != null)
        {
            if (DataManager.Instance.DeserializeJson() == null)
            {
                NewGame();
            }
            else
            {
                if (AudioManager.Instance != null)
                    AudioManager.Instance.PlaySFX("Menu",false);
                StartCoroutine(FadeEffect(0.3f));
                StartGameUI.SetActive(true);
            }
        }
    }

    public void NewGame()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("Menu",false);
        StartCoroutine(FadeEffect(1f));
        NewGameUI.SetActive(true);
    }

    /// <summary>
    /// Called from NewGame UI
    /// </summary>
    public void SavePlayerNameAndInitialize(string pName)
    {
        if (DataManager.Instance != null)
        {
            DataManager.Instance.NewGameStats(pName, 4, 10, true);
            DataManager.Instance.SerializeJson(true,true,true);
        }
    }

    public void StartPlaying()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("Menu",false);

        /* if (AudioManager.Instance != null)
            AudioManager.Instance.PlayMusic("MainTheme"); */
        StartCoroutine(FadeEffect(0.5f));
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
    IEnumerator FadeEffect(float time)
    {
        SleepFade.SetActive(true);
        yield return new WaitForSeconds(time);
        SleepFade.SetActive(false);
    }
}
