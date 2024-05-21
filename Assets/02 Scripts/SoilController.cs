using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoilController : MonoBehaviour
{
    public GameObject naturalTile;
    public GameObject tilledTile;
    public GameObject wateredTile;
    public GameObject seededTile;

    public bool IsFacing;

    private bool playerInTrigger;

    public bool PlayerInTrigger {get => playerInTrigger;}

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("<color=yellow> Enter Trigger Soil</color>");
        if(other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }


    void Update()
    {
        // Logic will change later
        if (playerInTrigger && IsFacing)
        {
            if (Input.GetKeyDown(KeyCode.E)) // Tille soil
                ActivateMap(false,true,false,false);
            else if (Input.GetKeyDown(KeyCode.Q) && tilledTile.activeInHierarchy) // Water soil
                ActivateMap(false,false,true,false);
            else if (Input.GetKeyDown(KeyCode.R) && wateredTile.activeInHierarchy) // Plant seed
                ActivateMap(false,false,false,true);

        }
    }

    void ActivateMap(bool natural, bool tilled, bool watered, bool seeded)
    {
        naturalTile.SetActive(natural);
        tilledTile.SetActive(tilled);
        wateredTile.SetActive(watered);
        seededTile.SetActive(seeded);
    }
}
