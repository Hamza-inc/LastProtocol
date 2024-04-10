using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class pickUpObj : MonoBehaviour
{
    Rigidbody rb;
    bool istake= false;
    public KeyCode interaction;
    [SerializeField] private Transform pivot;

    private Transform grabbedobj;

    private void LateUpdate()
    {
        if (Input.GetKeyDown(interaction)&& grabbedobj == null)
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward, out hit, 2.5f))
            {
                if (hit.collider.GetComponent<Rigidbody>())
                {

                    if (grabbedobj != null) return;

                    rb = hit.collider.GetComponent<Rigidbody>();
                    grabbedobj = hit.transform;
                    grabbedobj.SetParent(pivot);
                    rb.isKinematic = true;
                    rb.isKinematic = false;
                    rb.useGravity = false;
                    grabbedobj.localPosition = Vector3.zero;
                    grabbedobj.localRotation = Quaternion.Euler(Vector3.zero);
                    rb.gameObject.GetComponent<Tobj>().isPickup = true;
                }
            }
        }
        else if (Input.GetKeyDown(interaction) && grabbedobj != null)
        {
            grabbedobj.SetParent(null);
            rb.useGravity = true;
            rb.isKinematic = true;
            rb.isKinematic = false;
            rb.gameObject.GetComponent<Tobj>().isPickup = false;
            rb = null;
            grabbedobj = null;
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && grabbedobj  != null)
        {
            
            grabbedobj.SetParent(null);
            rb.useGravity = true;
            rb.isKinematic = true;
            rb.isKinematic = false;
            rb.AddForce(transform.forward*300);
            rb.gameObject.GetComponent<Tobj>().isPickup = false;
            rb = null;
            grabbedobj = null;

        }
        else if (grabbedobj != null && grabbedobj.gameObject.GetComponent<Tobj>().isPickup == false)
        {
            grabbedobj.SetParent(null);
            rb.useGravity = true;
            rb.isKinematic = true;
            rb.isKinematic = false;
            rb.gameObject.GetComponent<Tobj>().isPickup = false;
            rb = null;
            grabbedobj = null;
        }
    }
}
