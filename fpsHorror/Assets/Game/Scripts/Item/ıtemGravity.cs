using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ıtemGravity : NetworkBehaviour
{
    [Header("Gravity")]
    [SerializeField] private Transform groundCh;
    [SerializeField] private float gSpeed;

    public bool isGround, hitted;
    public float velocityY;
    Vector3 ground;

    private void Update()
    {
        #region Gravity
        if (IsOwner){
            if (!isGround && !hitted)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, groundCh.forward, out hit, Mathf.Infinity))
                {
                    ground = hit.point;
                    hitted = true;
                }
            }

            if (transform.position.y > ground.y && hitted)
            {
                isGround = false;
                velocityY += gSpeed * Time.deltaTime;
                transform.position += new Vector3(0, velocityY, 0) * Time.deltaTime;
            }
            else { velocityY = 0; isGround = true; transform.position = ground; }


        }


        #endregion
    }
}
