using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestUIController : MonoBehaviour
{
    [Header("Chest")]
    public ChestController chest;


    [Header("Chest content")]
    [SerializeField] private List<GameObject> itemListContainers;
    [SerializeField] private List<ChestItemUIController> itemListControllers;

    /* [Header("Chest description")] */
    void Start()
    {
        ShowCropsInChestInUI();
    }

    public void ShowCropsInChestInUI()
    {
        for(int i = 0; i < chest.CropsInChest.Count; i++)
        {
            itemListContainers[i].SetActive(true);
            ChestItemUIController itemListController = itemListContainers[i].GetComponent<ChestItemUIController>();
            itemListControllers.Add(itemListController);
            itemListController.thisCrop = chest.CropsInChest[i];

        }
    }
}
