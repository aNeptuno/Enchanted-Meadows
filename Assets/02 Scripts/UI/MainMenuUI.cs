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
    public GameObject MainMenu;

    public GameObject SleepFade;

    public GameObject StartGameUI;

    public GameObject StartGameUICoins;
    public List<GameObject> StartGameUIEnergy;
    public GameObject StartGameUIGameTime;
    public GameObject StartGameUIGameTimeDay;

    public GameObject NewGameUI;



    void InitializeData()
    {
        if (DataManager.Instance != null)
        {
            GameStats stats = DataManager.Instance.DeserializeJson();
            if (stats != null)
            {
                string text = "Loaded data: \r\n" + JsonConvert.SerializeObject(stats, Formatting.Indented);
                Debug.Log(text);
                // Coins
                StartGameUICoins.GetComponent<TextMeshProUGUI>().text = stats.PlayerCoins.ToString();

                // Energy
                foreach(GameObject go in StartGameUIEnergy)
                    go.SetActive(false);
                StartGameUIEnergy[stats.PlayerEnergy].SetActive(true);

                //Time
                StartGameUIGameTime.GetComponent<TextMeshProUGUI>().text = TimeSystem.Instance.FormatTime();
                StartGameUIGameTimeDay.GetComponent<TextMeshProUGUI>().text = "Day " + TimeSystem.Instance.days.ToString();
            }
        }
    }

    public void StartGame() // Play
    {
        if (DataManager.Instance != null)
        {
            if (DataManager.Instance.DeserializeJson() == null)
            {
                StartGameUI.SetActive(false);
                NewGame();
            }
            else
            {
                if (AudioManager.Instance != null)
                    AudioManager.Instance.PlaySFX("Menu",false);
                StartCoroutine(FadeEffect(1f));
                StartGameUI.SetActive(true);
                InitializeData();
            }
        }
    }

    public void ContinueGame()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("Menu",false);
        StartCoroutine(FadeEffect(1f));
        MainMenu.SetActive(false);
        SceneManager.LoadScene("Scene01");
    }

    public void NewGame()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("Menu",false);
        StartCoroutine(FadeEffect(1f));
        MainMenu.SetActive(false);
        StartGameUI.SetActive(false);
        NewGameUI.SetActive(true);
    }

    public void SavePlayerNameAndInitialize(string pName)
    {
        if (DataManager.Instance != null)
        {
            GameStats GameStats = new GameStats();
            GameStats.SaveGameStats(pName, 4, 10, true);
            DataManager.Instance.SerializeJson();
        }
    }

    public void StartNewGame()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("Menu",false);
        StartCoroutine(FadeEffect(1f));
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
