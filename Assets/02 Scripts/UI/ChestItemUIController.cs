
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChestItemUIController : MonoBehaviour, IPointerClickHandler
{
    public Crop thisCrop;

    public bool isStore;

    Dictionary<string,GameObject> posibleCropBags = new Dictionary<string,GameObject>();

    [SerializeField] private ChestDescriptionUIController descriptionUI;


    void Awake()
    {
        InitializeDictionary();
    }

    void InitializeDictionary()
    {
        foreach (Transform child in this.transform)
        {
            posibleCropBags.Add(child.name, child.gameObject);
        }
    }


    void Start()
    {
        if (thisCrop != null)
        {
            posibleCropBags[thisCrop.name].SetActive(true);
        }

    }

    public void UpdateUI()
    {
        foreach(KeyValuePair<string,GameObject> cropBag in posibleCropBags)
            cropBag.Value.SetActive(false);
        if (thisCrop != null)
            posibleCropBags[thisCrop.name].SetActive(true);
    }

    // -- Give Seed To Player || Buy
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isStore)
        {
            if (ChestController.Instance != null)
            {
                if (AudioManager.Instance != null)
                    AudioManager.Instance.PlaySFX("Menu",false);

                ChestController.Instance.GiveSeedToPlayer(thisCrop);
                CanvasController.Instance.HideChestUI();
            }
        }
        else if (isStore)
        {
            if (GameManager.Instance.playerCoins >= thisCrop.seedCost)
            {
                BuyCrop();
            }
            else
            {
                AudioManager.Instance.PlaySFX("No", false);
            }
        }
    }

    void BuyCrop()
    {
        // Buy crop
        thisCrop.amountOfSeedsInStorage++;
        GameManager.Instance.RemoveCoins(thisCrop.seedCost);
        AudioManager.Instance.PlaySFX("BuySell", false);
        descriptionUI.ShowDescription();

        // Add to chest
        ChestController.Instance.CropsInChest.Add(thisCrop);

        /* // Save chest state
        DataManager.Instance.SaveChestState(ChestController.Instance.CropsInChest); */
    }

}
