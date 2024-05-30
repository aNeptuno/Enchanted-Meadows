using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;

public enum DirtStates {
        NATURAL,
		TILLED,
		WATERED,
		SEEDED,
        PLANTED,
}
public class SoilController : MonoBehaviour
{
    [Header("Reference to its soil matrix")]
    public int iMatrix;
    public int jMatrix;

    #region "Dirt states management"

    [Space(5)]
    [Header("Dirt states management")]
    public GameObject naturalTile;
    public GameObject tilledTile;
    public GameObject wateredTile;
    public GameObject seededTile;

    DirtStates currentDirtState;
    public DirtStates CurrentDirtState
    {
        set
        {
            currentDirtState = value;

            switch (currentDirtState)
            {
                case DirtStates.NATURAL:
                    StartCoroutine(ActivateMap(true, false, false, false));
                    break;
                case DirtStates.TILLED:
                    StartCoroutine(ActivateMap(false, true, false, false));
                    break;
                case DirtStates.WATERED:
                    StartCoroutine(ActivateMap(false, false, true, false));
                    break;
                case DirtStates.SEEDED:
                    StartCoroutine(ActivateMap(false, false, true, true));
                    break;
                case DirtStates.PLANTED:
                    StartCoroutine(ActivateMap(false, false, true, false));
                    break;
            }
        }
        get => currentDirtState;
    }


    #endregion
    public bool IsFacing;

    private bool playerInTrigger;

    public bool PlayerInTrigger {get => playerInTrigger;}

    [SerializeField] private PlayerController player;

    [SerializeField] public Crop currentCrop;

    [SerializeField] private GameObject currentCropState;

    private bool startGrowing = false;

    private bool startedGrowing = false;

    private bool readyToCollect = false;

    private float growTime;

    public bool StartGrowing  {get => startGrowing; set => startGrowing = value;}
    public bool StartedGrowing {get => startedGrowing; set => startedGrowing = value;}
    public bool ReadyToCollect {get => readyToCollect; set => readyToCollect = value;}

    public int SpriteIndex = 0;

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
                CurrentDirtState = DirtStates.TILLED;

            // Water soil
            if (Input.GetKeyDown(KeyCode.Q) && tilledTile.activeInHierarchy)
            {
                if (!seededTile.activeInHierarchy)
                    CurrentDirtState = DirtStates.WATERED;
                else
                {
                    CurrentDirtState = DirtStates.SEEDED;
                    startGrowing = true;
                }
            }

            // Plant seed
            if (Input.GetKeyDown(KeyCode.E) && !startedGrowing && (wateredTile.activeInHierarchy || tilledTile.activeInHierarchy) && !seededTile.activeInHierarchy)
                PlantSeed();
        }

        if (startGrowing && !startedGrowing)
            GrowSeed();

        if (startedGrowing)
            StartCoroutine(GrowingCrop());

        if (playerInTrigger && IsFacing && readyToCollect && Input.GetKeyDown(KeyCode.E) && GameManager.Instance.playerEnergy > 0)
        {
            player.CollectCrop(currentCrop);
            StartCoroutine(ResetSoil(0.8f));
        }

        if (GameManager.Instance.forceGrowCrops) growTime = 0f;
    }

    private IEnumerator ActivateMap(bool natural, bool tilled, bool watered, bool seeded)
    {
        yield return new WaitForSeconds(0.3f);
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
                growTime = currentCrop.growTime;

                if (wateredTile.activeInHierarchy)
                    StartCoroutine(ActivateMap(false,false,true,true));
                else if (tilledTile.activeInHierarchy) StartCoroutine(ActivateMap(false,true,false,true));

                player.seedInHand.amountOfSeedsInStorage--;
                if (player.seedInHand.amountOfSeedsInStorage == 0)
                {
                    player.RemoveSeedInHand();
                    ChestController.Instance.CropsInChest.Remove(player.seedInHand);
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

    float timeMult = 1f;
    IEnumerator GrowingCrop()
    {
        List<Sprite> statesSprites = currentCrop.growStatesSprites;

        yield return new WaitForSeconds(growTime);

        // Desactivate seeded tile
        CurrentDirtState = DirtStates.PLANTED;

        for (int i = SpriteIndex; i < statesSprites.Count; i++)
        {
            currentCropState.GetComponent<SpriteRenderer>().sprite = statesSprites[i];
            yield return new WaitForSeconds(growTime * timeMult);
            timeMult = timeMult + 1f;

            SpriteIndex++;
        }
        if (SpriteIndex == statesSprites.Count)
            CollectCrop();
        else if (GameManager.Instance.forceGrowCrops) CollectCrop();
    }

    void CollectCrop()
    {
        currentCropState.GetComponent<SpriteRenderer>().sprite = currentCrop.cropSprite;
        readyToCollect = true;
    }

    IEnumerator ResetSoil(float waitTime)
    {
        startGrowing = false;
        currentCrop = null;
        currentCropState.GetComponent<SpriteRenderer>().sprite = null;
        startedGrowing = false;
        readyToCollect = false;
        SpriteIndex = 0;

        yield return new WaitForSeconds(waitTime);
        CurrentDirtState = DirtStates.NATURAL;
    }

}
