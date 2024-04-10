using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunkerDoor_otomation : MonoBehaviour
{
    Animator animator;
    bool isOpen;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Player")
        {
            if (!isOpen)
            {
                isOpen = true;
                animator.SetBool("IsOpen", isOpen);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isOpen)
        {
            isOpen = false;
            animator.SetBool("IsOpen", isOpen);
        }
    }
}
