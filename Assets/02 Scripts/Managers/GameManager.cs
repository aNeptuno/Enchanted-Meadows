
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region "Singleton"
    public static GameManager Instance { get; private set; } // Singleton instance

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

    #region  "Data Variables"
    public int playerEnergy;
    public int playerCoins;
    public bool newGame;
    public string playerName;

    #endregion
    public bool forceGrowCrops = false;

    public PlayerController player;
    public Vector3 playerStartPosition;

    //------------------------------

    #region "SAVE GAME - LOAD GAME"
    public void SaveGame()
    {
        // Save game stats
        DataManager.Instance.NewGameStats(playerName, playerEnergy, playerCoins, newGame);
        DataManager.Instance.GameStatsSaveTime(TimeSystem.Instance.hours, TimeSystem.Instance.minutes, TimeSystem.Instance.days);

        // Save chest state
        DataManager.Instance.SaveChestState(ChestController.Instance.CropsInChest);

        // Save soil state
        DataManager.Instance.SaveSoilMatrixState();

        // Save all to file
        DataManager.Instance.SerializeJson(true,true,true);
    }

    public void SaveGameTemp()
    {
        // Save game stats
        DataManager.Instance.NewGameStatsTemp(playerName, playerEnergy, playerCoins, newGame);
        DataManager.Instance.GameStatsSaveTimeTemp(TimeSystem.Instance.hours, TimeSystem.Instance.minutes, TimeSystem.Instance.days);

        // Save chest state
        DataManager.Instance.SaveChestStateTemp(ChestController.Instance.CropsInChest);

        // Save soil state
        DataManager.Instance.SaveSoilMatrixStateTemp();

        // Save all to file
        DataManager.Instance.SerializeJsonTemp(true,true,true);
    }

    public void NewGame() // testing
    {
        DataManager.Instance.NewGame();
    }

    public void LoadGame(bool temp) // obtain data
    {
        GameStats LoadedGame;
        if (!temp)
            LoadedGame = DataManager.Instance.DeserializeJson(); // como se inicializa desde menu start, no deberia ser nulo
        else
            LoadedGame = DataManager.Instance.DeserializeJsonTemp();

        playerEnergy = LoadedGame.PlayerEnergy;
        playerCoins = LoadedGame.PlayerCoins;
        newGame = LoadedGame.NewGame;
        playerName =  LoadedGame.PlayerName;

        string text = "Loaded game data (From Game Manager): \r\n" + JsonConvert.SerializeObject(LoadedGame, Formatting.Indented);
        //Debug.Log(text);
    }

    public void LoadChestState(bool temp)
    {
        ChestState LoadedChest;
        if (!temp)
            LoadedChest = DataManager.Instance.DeserializeJsonChest();
        else
            LoadedChest = DataManager.Instance.DeserializeJsonChestTemp();

        DataManager.Instance.ParseAndAddToChest(LoadedChest.cropsInChest);
    }

    public void LoadSoilState(bool temp)
    {
        MatrixSoilState LoadedSoil;
        if (!temp)
            LoadedSoil = DataManager.Instance.DeserializeJsonSoil();
        else
            LoadedSoil = DataManager.Instance.DeserializeJsonSoilTemp();
        DataManager.Instance.LoadSoilMatrix(LoadedSoil);
    }

    #endregion

    #region  "Game Start Initialization"
    void Start()
    {
        GameInitialization(false);
        player = FindAnyObjectByType<PlayerController>();
        playerStartPosition = player.transform.position;
    }

    /// Temp indicates when the data is temporal data
    public void GameInitialization(bool temp)
    {
        MyDebugLog.Instance.MyDebugFunc($"Game initialization temp?: {temp}",null,"cyan");
        LoadGame(temp);
        LoadChestState(temp);

        // Remove previous soil controllers
        SoilManager.Instance.RemoveAllSoilControllers();

        // Generates new soil controllers and loads soil state
        SoilManager.Instance.GenerateSoil(temp);

        if (newGame == true) newGame = false;
    }


    #endregion

    #region  "Coins"
    public void AddCoins(int amount)
    {
        playerCoins+= amount;
    }

    public void RemoveCoins(int amount)
    {
        playerCoins-= amount;
    }
    #endregion

    #region  "Energy and crops (bed)"
    public void IncreaseEnergy(int amount)
    {
        if (playerEnergy + amount > 0)
            playerEnergy = 4;
        else
            playerEnergy += amount;
    }

    public void DecreaseEnergy(int amount)
    {
        if (playerEnergy - amount < 0)
            playerEnergy = 0;
        else
            playerEnergy -= amount;
    }

    public void RestoreEnergy()
    {
        playerEnergy = 4;
    }

    public void GrowCrops()
    {
        //forceGrowCrops = true;
        SoilManager.Instance.ForceGrowCropOnEachSoil();
    }
    #endregion


}
