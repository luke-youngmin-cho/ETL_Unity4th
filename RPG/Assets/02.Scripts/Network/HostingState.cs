using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Multiplayer;

public class HostingState : NetworkBehaviour
{
    public const int MAX_CONNECT_PAYLOAD = 1024;

    public void SetUp()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
        NetworkManager.Singleton.StartHost();
    }

    public void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        var connectionData = request.Payload;

        if (connectionData.Length > MAX_CONNECT_PAYLOAD)
        {
            response.Approved = false;
            return;
        }
    }
}
