using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{
    #region "Singleton"
    public static CanvasController Instance { get; private set; } // Singleton instance

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

    [Header("Chest UI")]
    public GameObject ChestUI;
    public GameObject ChestUIButton;

    public ChestDescriptionUIController chestDescription;

    [Space(10)]

    [Header("Game Stats UI")]
    public GameObject GameStatsUICoins;
    public List<GameObject> GameStatsUIEnergy;

    public GameObject GameStatsUIGameTime;
    public GameObject GameStatsUIGameTimeDay;

    [Space(10)]

    [Header("Bed UI")]
    public GameObject BedUI;
    public GameObject SleepFade;

    [Space(10)]

    [Header("Bridge UI")]
    public GameObject BridgeUIHouse;
    public GameObject BridgeUITown;

    [Space(10)]

    [Header("Store UI")]
    public GameObject StoreUI;
    public GameObject StoreUIButton;

    [Space(10)]

    [Header("Exit Game UI")]
    public GameObject ExitGameUI;

    [Space(20)]

    [Header("--- TESTING ---")]
    public GameObject errorPanel;
    public GameObject errorPanelMsg;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ChestUI.gameObject.activeSelf) HideChestUI();
            else if (BedUI.gameObject.activeSelf) HideBedUI();
            else if (StoreUI.gameObject.activeSelf) HideStoreUI();
            else if (ExitGameUI.gameObject.activeSelf) HideExitGameUI();
            else ShowExitGameUI();
        }

        UpdateUICoins();
        UpdateUIEnergy();
        UpdateUIGameTime();
    }

    #region "ChestUI"
    public void ShowChestUI()
    {
        ChestUI.SetActive(true);
        ChestUIButton.SetActive(true);
        ChestUI.GetComponent<ChestUIController>().RestoreUI();
        chestDescription.ResetDescription();
        ChestUI.GetComponent<ChestUIController>().ShowCropsInChestInUI();
    }

    public void HideChestUI()
    {
        if (ChestController.Instance != null && ChestController.Instance.isOpened)
            ChestController.Instance.CloseChest();

        if (ChestUI != null)
        {
            chestDescription.ResetDescription();
            ChestUI.SetActive(false);
        }
        if (ChestUIButton != null)
            ChestUIButton.SetActive(false);


    }
    #endregion

    #region "Game Stats UI"
    void UpdateUICoins()
    {
        if (GameManager.Instance != null)
            GameStatsUICoins.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.playerCoins.ToString();
    }

    void UpdateUIEnergy()
    {
        foreach(GameObject go in GameStatsUIEnergy)
            go.SetActive(false);
        GameStatsUIEnergy[GameManager.Instance.playerEnergy].SetActive(true);
    }

    void UpdateUIGameTime()
    {
        GameStatsUIGameTime.GetComponent<TextMeshProUGUI>().text = TimeSystem.Instance.FormatTime();
        GameStatsUIGameTimeDay.GetComponent<TextMeshProUGUI>().text = "Day " + TimeSystem.Instance.days.ToString();
    }

    #endregion

    #region "Bed UI"
    public void ShowBedUI()
    {
        BedUI.SetActive(true);
    }

    public void HideBedUI()
    {
        BedUI.SetActive(false);
    }

    public void Sleep()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("Menu",false);

        if (BedUI.activeSelf)
            BedUI.SetActive(false);

        StartCoroutine(FadeEffect(3f));

        BedController.Instance.Sleep();
    }

    #endregion

    #region "Bridge UI"
    public void ShowBridgeUI(bool fromHouse)
    {
        BridgeUIHouse.SetActive(fromHouse);
        BridgeUITown.SetActive(!fromHouse);
    }

    public void HideBridgeUI(bool fromHouse)
    {
        if (fromHouse && BridgeUIHouse.activeSelf)
            BridgeUIHouse.SetActive(false);
        else if (BridgeUITown.activeSelf)
            BridgeUITown.SetActive(false);
    }

    public void Travel()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("Menu",false);

        StartCoroutine(FadeEffect(1f));

        if (Bridge.Instance != null)
            Bridge.Instance.Travel();
        else Debug.Log("Bridge is null");
    }
    #endregion

    #region "Store"
    public void ShowStoreUI()
    {
        StoreUI.SetActive(true);
        StoreUIButton.SetActive(true);
    }

    public void HideStoreUI()
    {
        StoreUI.SetActive(false);
        StoreUIButton.SetActive(false);
    }
    #endregion

    #region "Exit UI"
    public void ShowExitGameUI()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("Menu",false);
        StartCoroutine(FadeEffect(0.3f));
        ExitGameUI.SetActive(true);
    }

    public void HideExitGameUI()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("Menu",false);
        StartCoroutine(FadeEffect(0.3f));
        ExitGameUI.SetActive(false);
    }

    public void SaveAndExitGame()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.SaveGame();
        else ShowErrorInUI("GM is null");
        ExitGame();
    }
    public void ExitGame()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("Menu",false);
        StartCoroutine(FadeEffect(0.3f));
        ExitGameUI.SetActive(false);
        SceneManager.LoadScene("Scene00");
    }
    #endregion

    /* public void MakeBedFade(float time, PlayerController player)
    {
        StartCoroutine(FadeEffect(time, player));
    }

    IEnumerator FadeEffect(float time, PlayerController player)
    {
        player.IsSleeping = true;
        SleepFade.SetActive(true);
        yield return new WaitForSeconds(time);
        SleepFade.SetActive(false);
        player.IsSleeping = false;
    }
 */
    IEnumerator FadeEffect(float time)
    {
        SleepFade.SetActive(true);
        yield return new WaitForSeconds(time);
        SleepFade.SetActive(false);
    }

    // --- Testing
    public void ShowErrorInUI(string errorLog)
    {
        #if !UNITY_EDITOR

        errorPanel.SetActive(true);
        errorPanelMsg.GetComponent<TextMeshProUGUI>().text = errorLog;

        #endif
    }

}
