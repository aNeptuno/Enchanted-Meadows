using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreUIController : MonoBehaviour
{
    [Header("Chest")]
    public ChestController chest;

    public List<Crop> cropsInStore;


    [Header("Chest content")]
    [SerializeField] private List<GameObject> itemListContainers;
    [SerializeField] private List<ChestItemUIController> itemListControllers;

    void Start()
    {
        if (ChestController.Instance != null)
            chest = ChestController.Instance;
        ShowCropsInStoreInUI();
    }

    public void ShowCropsInStoreInUI()
    {
        for(int i = 0; i < cropsInStore.Count; i++)
        {
            itemListContainers[i].SetActive(true);
            ChestItemUIController itemListController = itemListContainers[i].GetComponent<ChestItemUIController>();

            itemListControllers.Add(itemListController);
            itemListController.thisCrop = cropsInStore[i];
            itemListController.isStore = true;

        }
    }
}
