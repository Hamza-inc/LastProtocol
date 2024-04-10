using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tobj : MonoBehaviour
{
    public bool isPickup = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (isPickup)
        {
            isPickup = false;
        }
    }
}
