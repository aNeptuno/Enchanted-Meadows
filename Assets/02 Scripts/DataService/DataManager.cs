
using System;
using Newtonsoft.Json;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    #region "Singleton"
    public static DataManager Instance { get; private set; } // Singleton instance

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

    #region "DATA SERVICE"
    private GameStats GameStats = new GameStats();
    private IDataService DataService = new JsonDataService();
    private bool EncryptionEnabled;
    private long SaveTime;
    private long LoadTime;

    public void ToggleEncryption(bool EncryptionEnabled)
    {
        this.EncryptionEnabled = EncryptionEnabled;
    }

    /* public void SaveGame()
    {
        GameStats.SaveTime(TimeSystem.Instance.hours, TimeSystem.Instance.minutes, TimeSystem.Instance.days);
        GameStats.SaveGameStats(playerName, playerEnergy, playerCoins, newGame);//, cropsInChest);
        SerializeJson();
    } */

    public void NewGame()
    {
        GameStats.ResetGameStats();
        SerializeJson();
    }

    /* public void LoadGame()
    {
        GameStats loadGame = DeserializeJson();

        playerEnergy = loadGame.PlayerEnergy;
        playerCoins = loadGame.PlayerCoins;
        newGame = loadGame.NewGame;
        playerName =  loadGame.PlayerName;

        string text = "Loaded data: \r\n" + JsonConvert.SerializeObject(loadGame, Formatting.Indented);
        Debug.Log(text);
    } */

    public void SerializeJson() //save data
    {
        long startTime = DateTime.Now.Ticks;
        if (DataService.SaveData("/game-stats.json", GameStats, EncryptionEnabled))
        {
            SaveTime = DateTime.Now.Ticks - startTime;
            Debug.Log("SaveTime: " +SaveTime / 1000f +" ms");
        }
        else
        {
            Debug.Log("Could not save the file");
        };
    }

    public GameStats DeserializeJson() //load data
    {
        long startTime = DateTime.Now.Ticks;
        try
        {
            GameStats loadedStats = DataService.LoadData<GameStats>("/game-stats.json", EncryptionEnabled);
            LoadTime = DateTime.Now.Ticks - startTime;
            Debug.Log("LoadTime: " +LoadTime / 1000f +" ms");

            return(loadedStats);

        }
        catch (Exception e)
        {
            Debug.Log($"Could not load the file due to {e.Message}");
            return null;
        };
    }
    #endregion

}
