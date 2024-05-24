using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    public Sprite customCursorSprite;

    [Header("Chest UI")]
    public GameObject ChestUI;
    public GameObject ChestUIButton;

    public ChestDescriptionUIController chestDescription;

    [Space(10)]

    [Header("Game Stats UI")]
    public GameObject GameStatsUICoins;
    public List<GameObject> GameStatsUIEnergy;

    public GameObject GameStatsUIGameTime;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && ChestUI.gameObject.activeSelf)
            HideChestUI();

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
}
