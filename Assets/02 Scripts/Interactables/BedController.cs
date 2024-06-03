using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BedController : MonoBehaviour
{
    #region "Singleton"
    public static BedController Instance { get; private set; } // Singleton instance

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // If not, set it to this instance
            //DontDestroyOnLoad(gameObject); // Make this instance persist across scenes
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Destroy this instance if another instance already exists
        }
    }
    #endregion

    private bool playerInTrigger;

    private PlayerController player;

    void Update()
    {
        if (playerInTrigger)
        {
            if (Input.GetKeyDown(KeyCode.E))
                CanvasController.Instance.ShowBedUI();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            player = other.GetComponent<PlayerController>();

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CanvasController.Instance.HideBedUI();
            playerInTrigger = false;
            player = null;
        }
    }

    public void Sleep()
    {
        if (player != null)
        {
            player.RemoveSeedInHand();
            player.IsInBed = true;
        }
        TimeSystem.Instance.AddDay();
        GameManager.Instance.GrowCrops();
        GameManager.Instance.RestoreEnergy();
        StartCoroutine(SaveAndInit());
    }

    IEnumerator SaveAndInit()
    {
        GameManager.Instance.SaveGame();
        yield return new WaitForSeconds(3f);
        GameManager.Instance.GameInitialization();
        // sonido de gallo
        player.IsInBed = false;
    }

}
