using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;
using System;

public struct InputPayload : INetworkSerializable
{
    public uint tick;
    public DateTime timeMark;
    public ulong networkObjectID;
    public Vector3 current;
    public Vector3 input;
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref tick);
        serializer.SerializeValue(ref timeMark);
        serializer.SerializeValue(ref networkObjectID);
        serializer.SerializeValue(ref current);
        serializer.SerializeValue(ref input);
    }
}

public struct RigidbodyPayload : INetworkSerializable
{
    public uint tick;
    public ulong networkObjectID;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;
    public Vector3 angularVelocity;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref position);
        serializer.SerializeValue(ref rotation);
        serializer.SerializeValue(ref velocity);
        serializer.SerializeValue(ref angularVelocity);
    }
}


public class NetworkTransformWithExtrapolation : NetworkTransform
{
    [SerializeField] float _extraPolationTimeLimit = 0.2f;
    [SerializeField] float _extraPolationGain = 2.2f;
    CountdownTimer _extrapolationTimer;
    NetworkTimer _networkTimer;
    RigidbodyPayload _lastStateOfServer;
    RigidbodyPayload _extrapolateState;
    Queue<InputPayload> _serverInputQueue = new Queue<InputPayload>();
    protected Vector3 clientInput;
    Rigidbody _rigidbody;
    [SerializeField] float _moveSpeed = 2.0f;

    protected override void Awake()
    {
        base.Awake();
        _rigidbody = GetComponent<Rigidbody>();
        _networkTimer = new NetworkTimer(60.0f);
        _extrapolationTimer = new CountdownTimer(_extraPolationTimeLimit);
    }

    protected override void Update()
    {
        base.Update();

        if (IsServer)
        {
            _extrapolationTimer.Tick(Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (_networkTimer.TryUpdate(Time.fixedDeltaTime))
        {
            RefreshClientTick();
            RefreshServerTick();
        }

        Extrapolate();
    }


    private RigidbodyPayload Move(InputPayload inputPayload)
    {
        transform.position += Quaternion.LookRotation(transform.forward, transform.up) * inputPayload.input * _moveSpeed * Time.fixedDeltaTime;

        return new RigidbodyPayload()
        {
            tick = inputPayload.tick,
            networkObjectID = inputPayload.networkObjectID,
            position = transform.position,
            rotation = transform.rotation,
            velocity = _rigidbody.velocity,
            angularVelocity = _rigidbody.angularVelocity,
        };
    }

    private void RefreshServerTick()
    {
        if (!IsServer)
            return;

        InputPayload inputPayload = default;
        while (_serverInputQueue.Count > 0)
        {
            inputPayload = _serverInputQueue.Dequeue();
            // todo -> Handle with all server inputs in queue.
        }

        _lastStateOfServer = Move(inputPayload);
        SendServerStateClientRpc(_lastStateOfServer);
        HandleExtrapolateState(_lastStateOfServer, CalcLatency(inputPayload));
    }

    private void RefreshClientTick()
    {
        if (!IsOwner)
            return;

        if (clientInput.magnitude <= 0)
            return;

        InputPayload inputPayload = new InputPayload()
        {
            tick = _networkTimer.ticks,
            timeMark = DateTime.Now,
            networkObjectID = NetworkObjectId,
            input = clientInput,
            current = transform.position
        };

        SendInputServerRpc(inputPayload);
    }

    private void HandleExtrapolateState(RigidbodyPayload latest, float latency)
    {
        if (ShouldExtrapolate(latency))
        {
            _extrapolateState.position = latest.velocity * (1 + latency * _extraPolationGain) * Time.fixedDeltaTime
                                         + latest.position;
            _extrapolateState.rotation = Quaternion.AngleAxis(latency * latest.angularVelocity.magnitude * Mathf.Rad2Deg, latest.angularVelocity)
                                         * latest.rotation;
            _extrapolateState.velocity = latest.velocity;
            _extrapolateState.angularVelocity = latest.angularVelocity;
            _extrapolationTimer.Start();
        }
        else
        {
            _extrapolationTimer.Stop();
        }
    }

    private bool ShouldExtrapolate(float latency)
    {
        return latency < _extraPolationTimeLimit && latency > Time.fixedDeltaTime;
    }


    private void Extrapolate()
    {
        if (IsServer && _extrapolationTimer.isRunning)
            transform.position = _extrapolateState.position;
    }

    private float CalcLatency(InputPayload inputPayload) 
        => (DateTime.Now - inputPayload.timeMark).Milliseconds / 1000.0f;


    [ClientRpc]
    private void SendServerStateClientRpc(RigidbodyPayload serverState)
    {
        if (!IsOwner)
            return;

        _lastStateOfServer = serverState;
    }

    [ServerRpc]
    private void SendInputServerRpc(InputPayload inputPayload)
    {
        _serverInputQueue.Enqueue(inputPayload);
    }
}
