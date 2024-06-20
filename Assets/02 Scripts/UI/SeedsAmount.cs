using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SeedsAmount : MonoBehaviour
{
    public PlayerController player;

    public TextMeshProUGUI amountText;

    void Awake()
    {
        player = FindAnyObjectByType<PlayerController>();
    }
    void Update()
    {
        if (player != null)
        {
            if (player.seedInHand != null)
            {
                amountText.text = "x" + player.seedInHand.amountOfSeedsInStorage.ToString();
            }
            else amountText.text ="";
        }
    }
}
