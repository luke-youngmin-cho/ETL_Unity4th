using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Test_NetworkPingPong : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (!IsServer && IsOwner)
        {
            PingServerRPC();
        }
    }

    [ServerRpc]
    private void PingServerRPC(ServerRpcParams rpcParams = default)
    {
        Debug.Log($"Server received RPC from {rpcParams.Receive.SenderClientId}");
        PongClientRPC();
    }

    [ClientRpc]
    private void PongClientRPC(ClientRpcParams rpcParams = default)
    {
        Debug.Log($"Client {OwnerClientId} received RPC");
    }
}
