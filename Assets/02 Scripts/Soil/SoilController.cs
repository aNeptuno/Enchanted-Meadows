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
                    SpriteIndex = 0;
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
                    naturalTile.SetActive(false);
                    seededTile.SetActive(false);
                    wateredTile.SetActive(true);
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

    public bool isForcedToGrow = false;

    public bool finishedForcedGrow = false;

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
        if (playerInTrigger)// && IsFacing)
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

        // --- "Normal" functioning of the seed growing --- \\\
        if (startGrowing && !startedGrowing && !isForcedToGrow && !readyToCollect)
            GrowSeed();

        if (!startGrowing && startedGrowing && !isForcedToGrow && !readyToCollect)
            GrowSeed();

        // --- Forced functioning of the seed growing --- \\\
        if (finishedForcedGrow) isForcedToGrow = false;

        // -- Collect crop
        if (playerInTrigger && IsFacing && readyToCollect && Input.GetKeyDown(KeyCode.E))
        {
            if (GameManager.Instance.playerEnergy > 0)
                CollectCrop();
            else AudioManager.Instance.PlaySFX("No",false);
        }
    }

    void Start()
    {
        if (currentCrop != null && readyToCollect)
            ForceToGrow();
    }

    public void ForceToGrow()
    {
        isForcedToGrow = true;
        SpriteIndex = currentCrop.growStatesSprites.Count;

        // Desactivate seeded tile
        CurrentDirtState = DirtStates.PLANTED;
        CanCollectCrop();
        finishedForcedGrow = true;
    }

    void CollectCrop()
    {
        //player.CollectCrop(currentCrop);
        // Coin animation

        // Sound
        AudioManager.Instance.PlaySFX("Collect",false);
        // UI
        GameManager.Instance.AddCoins(currentCrop.earnCoins);
        GameManager.Instance.DecreaseEnergy(currentCrop.energyCost);

        StartCoroutine(ResetSoil(0.3f));
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
                    ChestController.Instance.RemoveCropFromChest(player.seedInHand);
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
            startedGrowing = true;
            startGrowing = false;
            StartCoroutine(GrowingCrop());
        }
    }

    float timeMult = 1f;
    IEnumerator GrowingCrop()
    {
        if (currentCrop != null)
        {
            //MyDebugLog.Instance.MyDebugFunc("growing corutine",null,"cyan");
            List<Sprite> statesSprites = currentCrop.growStatesSprites;

            yield return new WaitForSeconds(growTime);


                growTime = currentCrop.growTime;

            // Desactivate seeded tile
            CurrentDirtState = DirtStates.PLANTED;

            int i = SpriteIndex;
            while(i < statesSprites.Count && !readyToCollect)
            {
                currentCropState.GetComponent<SpriteRenderer>().sprite = statesSprites[i];
                yield return new WaitForSeconds(growTime * timeMult);
                timeMult = timeMult + 1f;

                i++;
                SpriteIndex = i;
            }

            if (SpriteIndex == statesSprites.Count) CanCollectCrop();
        }
    }

    void CanCollectCrop()
    {
        readyToCollect = true;
        currentCropState.GetComponent<SpriteRenderer>().sprite = currentCrop.cropSprite;
    }

    IEnumerator ResetSoil(float waitTime)
    {
        startGrowing = false;
        currentCrop = null;
        currentCropState.GetComponent<SpriteRenderer>().sprite = null;
        startedGrowing = false;
        readyToCollect = false;

        isForcedToGrow = false;
        finishedForcedGrow = false;

        yield return new WaitForSeconds(waitTime);
        CurrentDirtState = DirtStates.NATURAL;
    }

}
