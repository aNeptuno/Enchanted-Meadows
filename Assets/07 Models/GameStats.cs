using System;
using System.Collections.Generic;

[Serializable]
public class GameStats
{
    public string PlayerName = "playerName";
    public int PlayerEnergy = 4;
    public int PlayerCoins = 10;
    public bool NewGame = true;
    //public List<Crop> CropsInChest = null;

    // -- GAME TIME
    public int Hours = 0;
    public int Minutes = 0;
    public int Days = 1;

    public void SaveGameStats(string pName, int pEnergy, int pCoins, bool newGame) //, List<Crop> cropsInChest)
    {
        PlayerName = pName;
        PlayerEnergy = pEnergy;
        PlayerCoins = pCoins;
        NewGame = newGame;
        //CropsInChest = cropsInChest;
    }

    public void SaveTime(int hours, int minutes, int days)
    {
        Hours = hours;
        Minutes = minutes;
        Days = days;
    }
    public void ResetGameStats()
    {
        PlayerName = "playerName";
        PlayerEnergy = 4;
        PlayerCoins = 10;
        NewGame = true;
        //CropsInChest = null;

        //-- Time Reset
        Hours = 0;
        Minutes = 0;
        Days = 1;
    }
}

