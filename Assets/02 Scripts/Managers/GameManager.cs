
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

    public List<Crop> cropsInChest;

    #endregion
    public bool forceGrowCrops = false;

    //------------------------------

    #region "DATA SERVICE"
    private GameStats GameStats = new GameStats(); //new declaration cause it overrides

    public void SaveGame()
    {
        GameStats.SaveTime(TimeSystem.Instance.hours, TimeSystem.Instance.minutes, TimeSystem.Instance.days);
        GameStats.SaveGameStats(playerName, playerEnergy, playerCoins, newGame);//, cropsInChest);
        DataManager.Instance.SerializeJson();
    }

    public void NewGame()
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

        string text = "Loaded data: \r\n" + JsonConvert.SerializeObject(LoadedGame, Formatting.Indented);
        Debug.Log(text);
    }

    #endregion

    #region  "Game Start Initialization"
    void Start()
    {
        LoadGame();

        if (ChestController.Instance !=null)
        {
            foreach(Crop crop in ChestController.Instance.CropsInChest)
                cropsInChest.Add(crop);
        }

        if (newGame)
        {
            GameInitialization();
            SaveGame();
        }
    }

    public void GameInitialization()
    {
        if (ChestController.Instance !=null)
        {
            foreach(Crop crop in ChestController.Instance.CropsInChest)
                crop.amountOfSeedsInStorage = 4;
        }

        SoilManager.Instance.GenerateSoil();

        newGame = false;
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
