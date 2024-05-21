using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("<color=yellow> Enter Trigger Door</color>");
        if (other.CompareTag("Player"))
        {
            animator.SetBool("DoorOpen",true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("<color=yellow> Exit Trigger Door</color>");
        if (other.CompareTag("Player"))
        {
            animator.SetBool("DoorOpen",false);
        }
    }
}
