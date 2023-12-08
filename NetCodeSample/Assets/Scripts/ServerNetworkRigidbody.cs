using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class ServerNetworkRigidbody : NetworkRigidbody
{
    NetworkVariable<Vector3> _syncedPosition = new NetworkVariable<Vector3>();
    NetworkVariable<Quaternion> _syncedRotation = new NetworkVariable<Quaternion>();
    Rigidbody _rigidbody;
    bool _connected;
    float _interpolateGain = 30.0f;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = IsServer == false;
        _connected = true;
    }
    private void Update()
    {
        if (_connected == false)
            return;

        if (IsServer)
        {
            _syncedPosition.Value = _rigidbody.position;
            _syncedRotation.Value = _rigidbody.rotation;
        }
        else
        {
            _rigidbody.position = Vector3.Lerp(_rigidbody.position, _syncedPosition.Value, Time.fixedDeltaTime * _interpolateGain); //_syncedPosition.Value;
            _rigidbody.rotation = Quaternion.Lerp(_rigidbody.rotation, _syncedRotation.Value, Time.fixedDeltaTime * _interpolateGain);
        }
    }
}
