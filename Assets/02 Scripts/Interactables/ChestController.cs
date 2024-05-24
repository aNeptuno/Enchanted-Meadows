using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    #region "Singleton"
    public static ChestController Instance { get; private set; } // Singleton instance

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // If not, set it to this instance
            DontDestroyOnLoad(gameObject); // Make this instance persist across scenes
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Destroy this instance if another instance already exists
        }
    }
    #endregion

    bool playerInTrigger;

    Animator animator;

    private bool isOpened = false;

    public AudioSource chestOpen;
    public AudioSource chestClose;

    [SerializeField] private PlayerController player;
    [SerializeField] private List<Crop> cropsInChest;

    public List<Crop> CropsInChest {get => cropsInChest;}

    void Start()
    {
        animator = GetComponent<Animator>();

        // testing (esto lo va a hacer la tienda luego)
        foreach(Crop crop in cropsInChest)
        {
            crop.amountOfSeedsInStorage = 2;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
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
            if (isOpened)
                CloseChest();
        }
    }


    void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            if (!isOpened)
                OpenChest();
            else
                CloseChest();
        }
    }

    void OpenChest()
    {
        isOpened = true;
        animator.Play("Open");
        animator.SetTrigger("OpenTrigger");

        if (!chestOpen.isPlaying)
        {
            chestOpen.Play();
            chestClose.Stop();
        }
        if (chestClose.isPlaying) chestClose.Stop();

        CanvasController.Instance.ShowChestUI();
    }

    public void CloseChest()
    {
        chestClose.Stop();
        animator.Play("Close");
        animator.SetTrigger("CloseTrigger");

        if (!chestClose.isPlaying)
        {
            chestClose.Play();
            chestOpen.Stop();
        }
        if (chestOpen.isPlaying) chestOpen.Stop();

        CanvasController.Instance.HideChestUI();
        isOpened = false;
    }

    public void GiveSeedToPlayer(Crop cropToGive)
    {
        if (cropToGive.amountOfSeedsInStorage > 0)
        {
            if (player != null)
                player.GrabSeed(cropToGive);
        }
    }


}
