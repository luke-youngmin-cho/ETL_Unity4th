using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour, IPunObservable
{
    private Vector3 _recvPos;
    private Quaternion _recvRot;
    private PhotonView _view;


    private void FixedUpdate()
    {
        if (_view.IsMine)
        {
            // todo -> move with axes.
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, _recvPos, 0.5f);
            transform.rotation = Quaternion.Lerp(transform.rotation, _recvRot, 0.5f);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // Send
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            _recvPos = (Vector3)stream.ReceiveNext();
            _recvRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
