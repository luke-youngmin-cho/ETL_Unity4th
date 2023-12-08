using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Pickable : NetworkBehaviour, IInteractable
{
    static Dictionary<ulong, Pickable> _picked = new Dictionary<ulong, Pickable>();
    public static Dictionary<ulong, Pickable> spawned = new Dictionary<ulong, Pickable>();
    NetworkVariable<bool> _isPicked = new NetworkVariable<bool>();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        _isPicked.OnValueChanged += OnIsInteractingChanged;
        spawned.Add(NetworkObjectId, this);
    }

    public void Interaction(ulong clientID, GameObject interactor)
    {
        if (_isPicked.Value)
            PutDown(clientID);
        else
            Pick(clientID);
    }

    
    void Pick(ulong clientID)
    {
        if (NetworkManager.Singleton.ConnectedClients
            .TryGetValue(clientID, out NetworkClient client))
        {
            if (GetComponent<NetworkObject>().TrySetParent(client.PlayerObject))
            {
                transform.position = client.PlayerObject.transform.Find("Hand").position;
                _picked.Add(clientID, this);
                _isPicked.Value = true;
            }
        }
    }

    void PutDown(ulong clientID)
    {
        if (_picked.ContainsKey(clientID))
        {
            if (_picked[clientID] == this)
            {
                if (GetComponent<NetworkObject>().TryRemoveParent())
                {
                    _picked.Remove(clientID);
                    _isPicked.Value = false;
                }
            }
        }
    }

    public void OnIsInteractingChanged(bool prev, bool current)
    {
        if (current)
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}