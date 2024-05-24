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

    [SerializeField] private GameObject currentCropState;

    private bool startGrowing = false;

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
            if (Input.GetKeyDown(KeyCode.E) && (wateredTile.activeInHierarchy || tilledTile.activeInHierarchy))
                PlantSeed();

        }

        if (startGrowing)
            GrowSeed();
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

                if (!wateredTile.activeInHierarchy)
                    StartCoroutine(ActivateMap(false,false,false,true));
                else StartCoroutine(ActivateMap(false,false,true,true));

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

        if (currentCrop != null && wateredTile.activeInHierarchy)
        {
            StartCoroutine(GrowingCrop());
        }
    }

    IEnumerator GrowingCrop()
    {
        MyDebugLog.Instance.MyDebugFunc("GrowingCrop coroutine");

        // Desactivate seeded tile
        StartCoroutine(ActivateMap(false,false,true,false));

        List<Sprite> statesSprites = currentCrop.growStatesSprites;
        float growTime = currentCrop.growTime;
        yield return new WaitForSeconds(growTime);

        for(int i = 0; i < statesSprites.Count; i++)
        {
            Sprite currentStateSprite = statesSprites[i];

            currentCropState.GetComponent<SpriteRenderer>().sprite = currentStateSprite;
            yield return new WaitForSeconds(growTime);
        }

        //ResetSoil(); Collect crop logic
    }

    void ResetSoil()
    {
        startGrowing = false;
        currentCrop = null;
        StartCoroutine(ActivateMap(true,false,false,false));
        currentCropState.GetComponent<SpriteRenderer>().sprite = null;
    }

}
