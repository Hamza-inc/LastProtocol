using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class force : MonoBehaviour
{
    public Quaternion restingAngle = Quaternion.identity;
    public float sforce = 750f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, restingAngle, sforce * Time.deltaTime));
    }
}
