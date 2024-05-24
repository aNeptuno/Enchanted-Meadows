using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Tooltip("From 0 to 4")]
    public int playerEnergy = 4;
    public int playerCoins = 10;

    public bool forceGrowAllCrops = false;

    #region  "Game Start"
    void Start()
    {
        // TESTING
        if (ChestController.Instance !=null)
        {
            foreach(Crop crop in ChestController.Instance.CropsInChest)
                crop.amountOfSeedsInStorage = 6;
        }
        RestoreEnergy();
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
        forceGrowAllCrops = true;
    }
    #endregion


}
