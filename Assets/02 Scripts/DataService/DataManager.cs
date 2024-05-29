
using System;
using System.Collections.Generic;
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
    private ChestState ChestState = new ChestState();
    private SoilState SoilState = new SoilState();
    private IDataService DataService = new JsonDataService();
    private bool EncryptionEnabled;
    private long SaveTime;
    private long LoadTime;

    [SerializeField] private List<Crop> availableCropsInGame;

    public void ToggleEncryption(bool EncryptionEnabled)
    {
        this.EncryptionEnabled = EncryptionEnabled;
    }

    public void NewGame()
    {
        SerializeJson(true,true,true);
    }

    #region "Game Stats"
    public void NewGameStats(string pName, int pEnergy, int pCoins, bool newGame)
    {
        GameStats.PlayerName = pName;
        GameStats.PlayerEnergy = pEnergy;
        GameStats.PlayerCoins = pCoins;
        GameStats.NewGame = newGame;
    }

    public void GameStatsSaveTime(int hours, int minutes, int days)
    {
        GameStats.Hours = hours;
        GameStats.Minutes = minutes;
        GameStats.Days = days;
    }
    #endregion

    #region "Chest state"
    public void SaveChestState(List<Crop> cropsInChest)
    {
        if (cropsInChest != null)
        {
            foreach(Crop crop in cropsInChest)
            {
                CropModel cropModel = new CropModel(crop.cropName, crop.amountOfSeedsInStorage);
                if (crop.amountOfSeedsInStorage > 0)
                    ChestState.cropsInChest.Add(cropModel);
            }
        }
        else Debug.Log("crops in chest is null");
    }

    public void ParseAndAddToChest(List<CropModel> cropsModelsInChest)
    {
        foreach(CropModel cropModel in cropsModelsInChest)
        {
            if (cropModel.AmountInStorage > 0)
            {
                Crop cropInChest = FindCropByName(cropModel.Name);
                cropInChest.amountOfSeedsInStorage = cropModel.AmountInStorage;

                if (ChestController.Instance.CropsInChest.Contains(cropInChest)) // To modify amount
                    ChestController.Instance.CropsInChest.Remove(cropInChest);

                ChestController.Instance.CropsInChest.Add(cropInChest);
            }
        }
    }

    public Crop FindCropByName(string name)
    {
        return availableCropsInGame.Find(crop => crop.cropName == name);
    }

    #endregion

    /// <summary>
    /// Save data
    /// </summary>
    public void SerializeJson(bool gameStats, bool chestState, bool soilState)
    {

        if (gameStats)
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

        if (chestState)
        {
            long startTime = DateTime.Now.Ticks;
            if (DataService.SaveData("/chest-state.json", ChestState, EncryptionEnabled))
            {
                SaveTime = DateTime.Now.Ticks - startTime;
                Debug.Log("SaveTime: " +SaveTime / 1000f +" ms");
            }
            else
            {
                Debug.Log("Could not save the file");
            };
        }

        if (soilState)
        {
            long startTime = DateTime.Now.Ticks;
            if (DataService.SaveData("/soil-state.json", SoilState, EncryptionEnabled))
            {
                SaveTime = DateTime.Now.Ticks - startTime;
                Debug.Log("SaveTime: " +SaveTime / 1000f +" ms");
            }
            else
            {
                Debug.Log("Could not save the file");
            };
        }
    }


    /// <summary>
    /// Load GameStats
    /// </summary>
    /// <returns></returns>
    public GameStats DeserializeJson()
    {
        long startTime = DateTime.Now.Ticks;
        try
        {
            GameStats loadedStats = DataService.LoadData<GameStats>("/game-stats.json", EncryptionEnabled);
            LoadTime = DateTime.Now.Ticks - startTime;
            Debug.Log("LoadTime (game data): " +LoadTime / 1000f +" ms");

            return(loadedStats);

        }
        catch (Exception e)
        {
            Debug.Log($"Could not load the file due to {e.Message}");
            return null;
        };
    }

    /// <summary>
    /// Load ChestState
    /// </summary>
    /// <returns></returns>
    public ChestState DeserializeJsonChest()
    {
        long startTime = DateTime.Now.Ticks;
        try
        {
            ChestState loadedStats = DataService.LoadData<ChestState>("/chest-state.json", EncryptionEnabled);
            LoadTime = DateTime.Now.Ticks - startTime;
            Debug.Log("LoadTime (chest): " +LoadTime / 1000f +" ms");

            return(loadedStats);

        }
        catch (Exception e)
        {
            Debug.Log($"Could not load the file due to {e.Message}");
            return null;
        };
    }

    /// <summary>
    /// Load SoilState
    /// </summary>
    /// <returns></returns>
    public SoilState DeserializeJsonSoil()
    {
        long startTime = DateTime.Now.Ticks;
        try
        {
            SoilState loadedStats = DataService.LoadData<SoilState>("/soil-state.json", EncryptionEnabled);
            LoadTime = DateTime.Now.Ticks - startTime;
            Debug.Log("LoadTime (soil): " +LoadTime / 1000f +" ms");

            return(loadedStats);

        }
        catch (Exception e)
        {
            Debug.Log($"Could not load the file due to {e.Message}");
            return null;
        };
    }
    #endregion

    // TIME FORMATTING

    public string FormatTime(GameStats Stats)
    {
        string period = "AM";
        int displayHours = Stats.Hours;

        if (Stats.Hours >= 12)
        {
            period = "PM";
            if (Stats.Hours > 12)
            {
                displayHours -= 12;
            }
        }
        else if (Stats.Hours == 0)
        {
            displayHours = 12;
        }

        return string.Format("{0:D2}:{1:D2} {2}", displayHours, Stats.Minutes, period);
    }

}
