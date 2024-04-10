using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NMRManager : NetworkBehaviour
{
    public static NMRManager instance;

    [Range(50, 500)]
    [SerializeField] private int maxStringLenght = 100;

    [SerializeField] private Event cici;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void StringSenderFromServerRPC(string _string, ulong _clientId, bool security = true)
    {
        if (security)
        {
            if (!string.IsNullOrEmpty(_string))
            {
                if (_string.Length <= maxStringLenght)
                {
                    SendStringFromClientRPC(_string, _clientId);
                }
                else
                {
                    Debug.LogWarning($"{_clientId} : String lenght over limit");
                    return;
                }
            }
            else
            {
                Debug.LogWarning($"{_clientId} : string is null");
            }
        }
        else
        {
            SendStringFromClientRPC(_string, _clientId);
        }
    }

    [ClientRpc]
    private void SendStringFromClientRPC(string _string, ulong _clientId)
    {
        Debug.Log($"{_clientId} {_string}");
    }


}
