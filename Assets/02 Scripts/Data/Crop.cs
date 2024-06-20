using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "cropID", menuName = "NewCrop", order = 1)]
public class Crop : ScriptableObject
{
    public string cropID;

    [Tooltip("Time to grow (seconds)")]
    public float growTime;

    [Tooltip("Coins earned when gather")]
    public int earnCoins;

    [Tooltip("Seed cost")]
    public int seedCost;

    [Tooltip("Player energy cost to gather")]
    public int energyCost;

    [Header("--- Sprites ---")]
    public Sprite seedBagSprite;

    public List<Sprite> growStatesSprites;
    public Sprite cropSprite;

    [Header("--- Storage (Chest) ---")]
    public int amountOfSeedsInStorage;

}
