using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
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

    [SerializeField] private GameObject currentCropState;

    private bool startGrowing = false;

    private bool startedGrowing = false;

    private bool readyToCollect = false;

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
        if (playerInTrigger && IsFacing)
        {
            // Tille soil only if it doesnt have an active plant on it
            if (Input.GetKeyDown(KeyCode.R) && !wateredTile.activeInHierarchy && !seededTile.activeInHierarchy)
                StartCoroutine(ActivateMap(false,true,false,false));

            // Water soil
            if (Input.GetKeyDown(KeyCode.Q) && tilledTile.activeInHierarchy)
            {
                if (!seededTile.activeInHierarchy)
                    StartCoroutine(ActivateMap(false,false,true,false));
                else
                {
                    StartCoroutine(ActivateMap(false,false,true,true));
                    startGrowing = true;
                }
            }

            // Plant seed
            if (Input.GetKeyDown(KeyCode.E) && (wateredTile.activeInHierarchy || tilledTile.activeInHierarchy) && !seededTile.activeInHierarchy)
                PlantSeed();

        }

        if (startGrowing && !startedGrowing && !GameManager.Instance.forceGrowAllCrops)
            GrowSeed();

        if (playerInTrigger && IsFacing && readyToCollect && Input.GetKeyDown(KeyCode.E))
        {
            player.CollectCrop(currentCrop);
            StartCoroutine(ResetSoil(0.8f));
        }

        if (GameManager.Instance.forceGrowAllCrops)
        {
            ForceGrowCrops();
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

                if (wateredTile.activeInHierarchy)
                    StartCoroutine(ActivateMap(false,false,true,true));
                else if (tilledTile.activeInHierarchy) StartCoroutine(ActivateMap(false,true,false,true));

                player.seedInHand.amountOfSeedsInStorage--;
                if (player.seedInHand.amountOfSeedsInStorage == 0)
                {
                    player.RemoveSeedInHand();
                }
                if (wateredTile.activeInHierarchy)
                    startGrowing = true;
            }
        }
    }

    void GrowSeed()
    {
        if (currentCrop != null)
        {
            StartCoroutine(GrowingCrop());
            startedGrowing = true;
        }
    }

    IEnumerator GrowingCrop()
    {
        List<Sprite> statesSprites = currentCrop.growStatesSprites;
        float growTime = currentCrop.growTime;

        yield return new WaitForSeconds(growTime);

        // Desactivate seeded tile
        StartCoroutine(ActivateMap(false,false,true,false));

        float timeMult = 1f;
        for(int i = 0; i < statesSprites.Count; i++)
        {
            currentCropState.GetComponent<SpriteRenderer>().sprite = statesSprites[i];
            yield return new WaitForSeconds(growTime * timeMult);
            timeMult = timeMult + 1f;
        }
        currentCropState.GetComponent<SpriteRenderer>().sprite = currentCrop.cropSprite;
        MyDebugLog.Instance.MyDebugFunc("READY TO COLLECT!",null,"green"); // play sound
        readyToCollect = true;
    }

    IEnumerator ResetSoil(float waitTime)
    {
        startGrowing = false;
        currentCrop = null;
        currentCropState.GetComponent<SpriteRenderer>().sprite = null;
        startedGrowing = false;
        readyToCollect = false;

        yield return new WaitForSeconds(waitTime);
        StartCoroutine(ActivateMap(true,false,false,false));
    }

    void ForceGrowCrops()
    {
        currentCropState.GetComponent<SpriteRenderer>().sprite = currentCrop.cropSprite;
        GameManager.Instance.forceGrowAllCrops = false;
        readyToCollect = true;
        startedGrowing = true;
        seededTile.SetActive(false);
        ResetSoil(0.1f);
    }

}
