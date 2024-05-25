using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator animator;

    public AudioSource doorOpen;
    public AudioSource doorClose;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("<color=yellow> Enter Trigger Door</color>");
        if (other.CompareTag("Player"))
        {
            OpenDoor();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log("<color=yellow> Exit Trigger Door</color>");
        if (other.CompareTag("Player"))
        {
            CloseDoor();
        }
    }

    void OpenDoor()
    {
        animator.SetBool("DoorOpen",true);

        // Audio
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("DoorOpen", false);
    }

    void CloseDoor()
    {
        animator.SetBool("DoorOpen",false);

        // Audio
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("DoorClose", false);
    }
}
