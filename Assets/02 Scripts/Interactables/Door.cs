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
        if (!doorOpen.isPlaying)
        {
            doorOpen.Play();
            doorClose.Stop();
        }
        if (doorClose.isPlaying) doorClose.Stop();
    }

    void CloseDoor()
    {
        animator.SetBool("DoorOpen",false);

        // Audio
        if (!doorClose.isPlaying)
        {
            doorClose.Play();
            doorOpen.Stop();
        }
        if (doorOpen.isPlaying) doorOpen.Stop();
    }
}
