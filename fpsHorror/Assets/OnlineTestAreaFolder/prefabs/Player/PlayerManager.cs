using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    [SerializeField] private GameObject everyone;

    

    private void Update()
    {
        if (!IsOwner){
           everyone.SetActive(false);    
        }
    }
}
