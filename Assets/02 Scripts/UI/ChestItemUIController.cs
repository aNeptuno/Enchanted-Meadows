
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChestItemUIController : MonoBehaviour, IPointerClickHandler
{
    public Crop thisCrop;

    Dictionary<string,GameObject> posibleCropBags = new Dictionary<string,GameObject>();


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

    // -- Give Seed To Player
    public void OnPointerClick(PointerEventData eventData)
    {
        if (ChestController.Instance != null)
        {
            ChestController.Instance.GiveSeedToPlayer(thisCrop);
            CanvasController.Instance.HideChestUI();
        }
    }

}
