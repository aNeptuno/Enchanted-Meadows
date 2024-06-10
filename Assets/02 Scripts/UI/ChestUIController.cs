using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestUIController : MonoBehaviour
{
    [Header("Chest content")]
    [SerializeField] private List<GameObject> itemListContainers;
    [SerializeField] private List<ChestItemUIController> itemListControllers;

    public List<ChestItemUIController> ItemListControllers {get => itemListControllers; set => itemListControllers = value;}

    public void ShowCropsInChestInUI()
    {
        for(int i = 0; i < ChestController.Instance.CropsInChest.Count; i++)
        {
            if (ChestController.Instance.CropsInChest[i].amountOfSeedsInStorage > 0)
            {
                itemListContainers[i].SetActive(true);
                ChestItemUIController itemListController = itemListContainers[i].GetComponent<ChestItemUIController>();
                itemListControllers.Add(itemListController);
                itemListController.thisCrop = ChestController.Instance.CropsInChest[i];
                itemListController.UpdateUI();
            }
        }

    }

    public void RestoreUI()
    {
        for(int i = 0; i < itemListContainers.Count; i++)
        {
            ChestItemUIController itemListController = itemListContainers[i].GetComponent<ChestItemUIController>();
            itemListController.thisCrop = null;
            itemListControllers.Remove(itemListController);
            itemListContainers[i].SetActive(false);
        }
    }
}
