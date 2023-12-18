using System;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class NetworkTransformWithSimpleExtrapolate : NetworkBehaviour
{
    protected Rigidbody rigidbody;
    [SerializeField] private float _extrapolateGain = 1.5f;
    float predictedLatency;
    int cachedServerTick;
    Vector3 cachedServerPos;

    protected void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (IsServer)
        {
            SendToClientRpc(NetworkManager.Singleton.ServerTime.Tick, transform.position);
        }

        if (IsClient)
        {
            Extrapolate(predictedLatency);
        }
    }

    public void Extrapolate(float latency)
    {
        Vector3 predicted = rigidbody.position + rigidbody.velocity * latency * Time.fixedDeltaTime * _extrapolateGain;
        rigidbody.position = predicted;
    }

    [ClientRpc]
    private void SendToClientRpc(int serverTick, Vector3 position)
    {
        predictedLatency = (serverTick - cachedServerTick) / (float)NetworkManager.Singleton.ServerTime.TickRate;
        cachedServerTick = serverTick;
        cachedServerPos = position;
        rigidbody.position = position;
    }
}