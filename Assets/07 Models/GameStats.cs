using System;

[Serializable]
public class GameStats
{
    public string PlayerName = "";
    public int PlayerEnergy = 4;
    public int PlayerCoins = 10;
    public bool NewGame = true;

    // -- GAME TIME
    public int Hours = 8;
    public int Minutes = 0;
    public int Days = 1;


}

