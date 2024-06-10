using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseFloor : MonoBehaviour
{
    public bool isPlayerInsideHouse;

    public GameObject rooft;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInsideHouse = true;
            if (rooft.activeSelf) rooft.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInsideHouse = false;
            if (!rooft.activeSelf) rooft.SetActive(true);
        }
    }
}
