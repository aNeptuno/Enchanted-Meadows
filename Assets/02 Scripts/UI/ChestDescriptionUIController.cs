using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChestDescriptionUIController : MonoBehaviour
{
    public Crop selectedCrop;
    public GameObject descriptionName;

    public GameObject descriptionEarnCoins;

    public GameObject descriptionEnergyCost;

    public GameObject descriptionGrowTime;

    public ChestItemUIController selectedCropDisplay;

    void Start()
    {
        if (selectedCrop != null)
            ShowDescription();
    }

    public void ShowDescription()
    {
        descriptionName.GetComponent<TextMeshProUGUI>().text = selectedCrop.cropName + " (x"+selectedCrop.amountOfSeedsInStorage.ToString()+")";

        if (!selectedCropDisplay.isStore)
            descriptionEarnCoins.GetComponent<TextMeshProUGUI>().text = selectedCrop.earnCoins.ToString();
        else
            descriptionEarnCoins.GetComponent<TextMeshProUGUI>().text = selectedCrop.seedCost.ToString();

        descriptionEnergyCost.GetComponent<TextMeshProUGUI>().text = selectedCrop.energyCost.ToString();
        descriptionGrowTime.GetComponent<TextMeshProUGUI>().text = selectedCrop.growTime.ToString();

        selectedCropDisplay.thisCrop = selectedCrop;
        selectedCropDisplay.UpdateUI();

    }

    public void ResetDescription()
    {
        descriptionName.GetComponent<TextMeshProUGUI>().text = "";
        descriptionEarnCoins.GetComponent<TextMeshProUGUI>().text = "";
        descriptionEnergyCost.GetComponent<TextMeshProUGUI>().text = "";
        descriptionGrowTime.GetComponent<TextMeshProUGUI>().text = "";
        selectedCropDisplay.thisCrop = null;
    }


}
