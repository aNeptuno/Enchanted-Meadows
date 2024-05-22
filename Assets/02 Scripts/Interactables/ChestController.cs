using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    bool playerInTrigger;

    Animator animator;

    private bool isOpened = false;

    public AudioSource chestOpen;
    public AudioSource chestClose;

    [SerializeField] private PlayerController player;
    [SerializeField] private List<Crop> cropsInChest;

    // for testing
    [SerializeField] private int debugCropIndex = 0;

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

        GiveSeedToPlayer(debugCropIndex);
    }

    void CloseChest()
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

        isOpened = false;
    }

    void GiveSeedToPlayer(int cropIndex)
    {
        if (cropsInChest[cropIndex].amountOfSeedsInStorage > 0)
            player.GrabSeed(cropsInChest[cropIndex]);
    }


}
