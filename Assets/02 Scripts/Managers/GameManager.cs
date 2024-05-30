
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

    /* public List<Crop> cropsInChest; */

    #endregion
    public bool forceGrowCrops = false;

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

    public void NewGame() // testing
    {
        DataManager.Instance.NewGame();
    }

    public void LoadGame() // obtain data
    {
        GameStats LoadedGame = DataManager.Instance.DeserializeJson(); // como se inicializa desde menu start, no deberia ser nulo

        playerEnergy = LoadedGame.PlayerEnergy;
        playerCoins = LoadedGame.PlayerCoins;
        newGame = LoadedGame.NewGame;
        playerName =  LoadedGame.PlayerName;

        string text = "Loaded game data (From Game Manager): \r\n" + JsonConvert.SerializeObject(LoadedGame, Formatting.Indented);
        Debug.Log(text);
    }

    public void LoadChestState()
    {
        ChestState LoadedChest = DataManager.Instance.DeserializeJsonChest();
        DataManager.Instance.ParseAndAddToChest(LoadedChest.cropsInChest);
    }

    public void LoadSoilState()
    {
        MatrixSoilState LoadedSoil = DataManager.Instance.DeserializeJsonSoil();
        DataManager.Instance.LoadSoilMatrix(LoadedSoil);
    }

    #endregion

    #region  "Game Start Initialization"
    void Start()
    {
        LoadGame();
        LoadChestState();
        // Generates Soil Controllers and loads soil state
        SoilManager.Instance.GenerateSoil();

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
        forceGrowCrops = true;
    }
    #endregion


}
