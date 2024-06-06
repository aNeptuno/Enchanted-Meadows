
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.VisualScripting;
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
    //private SoilState SoilState = new SoilState();
    private MatrixSoilState MatrixSoilState = new MatrixSoilState();
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
    public void NewSoil() => MatrixSoilState = new MatrixSoilState();

        #region "Game Stats"
    public void NewGameStats(string pName, int pEnergy, int pCoins, bool newGame)
    {
        GameStats.PlayerName = pName;
        GameStats.PlayerEnergy = pEnergy;
        GameStats.PlayerCoins = pCoins;
        GameStats.NewGame = newGame;
        GameStats.Hours = 8;
        GameStats.Minutes = 0;
        GameStats.Days = 1;
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
            // Empty list
            ChestState.EmptyList();

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

        #region "Soil state"

        public void LoadSoilMatrix(MatrixSoilState loadedSoil)
        {
            if (SoilManager.Instance != null)
            {
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        ParseToSoilController(SoilManager.Instance.soilControllers[i,j] , loadedSoil.SoilStateMatrix[i,j]);
                    }
                }
            }
        }

        public void ParseToSoilController(SoilController soilController, SoilState soilState)
        {
            soilController.CurrentDirtState = soilState.CurrentDirtState;

            CropInSoil cropInSoil = soilState.CurrentCrop;
            if (cropInSoil.Name != "")
            {
                soilController.currentCrop = FindCropByName(cropInSoil.Name);
            }
            soilController.StartGrowing = cropInSoil.StartGrowing;
            soilController.StartedGrowing = cropInSoil.StartedGrowing;
            soilController.SpriteIndex = cropInSoil.SpriteIndex;
            soilController.ReadyToCollect = cropInSoil.ReadyToCollect;

            soilController.isForcedToGrow = cropInSoil.IsForcedToGrow;
            soilController.finishedForcedGrow = cropInSoil.FinishedForcedGrow;
        }

        public void SaveSoilMatrixState()
        {
            if (SoilManager.Instance != null)
            {
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        MatrixSoilState.AddMemberToMatrix(i,j, ParseToSoilState(SoilManager.Instance.soilControllers[i,j]));
                    }
                }
            }
        }

        public SoilState ParseToSoilState(SoilController soilCon)
        {
            string cropName ="";
            if (soilCon != null)
            {
                if (soilCon.currentCrop != null)
                    cropName = soilCon.currentCrop.cropName;

                CropInSoil currentCrop = new CropInSoil(cropName, soilCon.StartGrowing, soilCon.StartedGrowing, soilCon.SpriteIndex, soilCon.ReadyToCollect, soilCon.isForcedToGrow, soilCon.finishedForcedGrow);

                return new SoilState(soilCon.CurrentDirtState, currentCrop);
            } else return null;
        }


        #endregion

        #region "Serialize / Deserialize"
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
            if (DataService.SaveData("/soil-state.json", MatrixSoilState, EncryptionEnabled))
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
            //Debug.Log("LoadTime (game data): " +LoadTime / 1000f +" ms");

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
            //Debug.Log("LoadTime (chest): " +LoadTime / 1000f +" ms");

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
    public MatrixSoilState DeserializeJsonSoil()
    {
        long startTime = DateTime.Now.Ticks;
        try
        {
            MatrixSoilState loadedStats = DataService.LoadData<MatrixSoilState>("/soil-state.json", EncryptionEnabled);
            LoadTime = DateTime.Now.Ticks - startTime;
            //Debug.Log("LoadTime (soil): " +LoadTime / 1000f +" ms");

            return(loadedStats);

        }
        catch (Exception e)
        {
            Debug.Log($"Could not load the file due to {e.Message}");
            return null;
        };
    }
    #endregion

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
