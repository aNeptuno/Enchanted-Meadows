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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && ChestUI.gameObject.activeSelf)
            HideChestUI();

        if (Input.GetKeyDown(KeyCode.Escape) && BedUI.gameObject.activeSelf)
            HideBedUI();

        UpdateUICoins();
        UpdateUIEnergy();
    }

    #region "ChestUI"
    public void ShowChestUI()
    {
        ChestUI.SetActive(true);
        ChestUIButton.SetActive(true);
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
        if (BedUI.activeSelf)
            BedUI.SetActive(false);

        FadeEffect(3f);

        GameManager.Instance.RestoreEnergy();
        GameManager.Instance.GrowCrops();
        // sonido de gallo
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
        MyDebugLog.Instance.MyDebugFunc("Travel");
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

    IEnumerator FadeEffect(float time)
    {
        SleepFade.SetActive(true);
        yield return new WaitForSeconds(time);
        SleepFade.SetActive(false);
    }

}
