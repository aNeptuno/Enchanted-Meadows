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

    [SerializeField] private PlayerController player;

    [SerializeField] private Crop currentCrop;

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("<color=yellow> Enter Trigger Soil</color>");
        if(other.CompareTag("Player"))
        {
            playerInTrigger = true;
            player = other.GetComponent<PlayerController>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            player = null;
        }
    }


    void Update()
    {
        // Logic will change later
        if (playerInTrigger && IsFacing)
        {
            if (Input.GetKeyDown(KeyCode.R) && !wateredTile.activeInHierarchy && !seededTile.activeInHierarchy) // Tille soil only if it doesnt have an active plant on it
                StartCoroutine(ActivateMap(false,true,false,false));
            else if (Input.GetKeyDown(KeyCode.Q) && tilledTile.activeInHierarchy) // Water soil
                StartCoroutine(ActivateMap(false,false,true,false));
            else if (Input.GetKeyDown(KeyCode.E) && wateredTile.activeInHierarchy) // Plant seed
                PlantSeed();

        }
    }

    private IEnumerator ActivateMap(bool natural, bool tilled, bool watered, bool seeded)
    {
        yield return new WaitForSeconds(0.5f);
        naturalTile.SetActive(natural);
        tilledTile.SetActive(tilled);
        wateredTile.SetActive(watered);
        seededTile.SetActive(seeded);
    }

    void PlantSeed()
    {
        if (player != null)
        {
            if (player.seedInHand != null)
            {
                currentCrop = player.seedInHand;

                StartCoroutine(ActivateMap(false,false,false,true));

                player.seedInHand.amountOfSeedsInStorage--;
                if (player.seedInHand.amountOfSeedsInStorage == 0)
                {
                    player.RemoveSeedInHand();
                }
            }
        }
    }

}
