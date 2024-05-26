using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseFloor : MonoBehaviour
{
    public bool isPlayerInsideHouse;

    public GameObject rooft;

    public FadeController rooftFadeController;

    /* void Update()
    {
        if (rooft != null)
        {
            //rooft.SetActive(!isPlayerInsideHouse);
            if (!isPlayerInsideHouse)
                rooftFadeController.DeactivateWithFade();
        }
    } */
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInsideHouse = true;
            if (rooft.activeSelf)
                rooftFadeController.DeactivateWithFade();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInsideHouse = false;
            if (!rooft.activeSelf)
                rooftFadeController.ActivateWithFade();
        }
    }
}
