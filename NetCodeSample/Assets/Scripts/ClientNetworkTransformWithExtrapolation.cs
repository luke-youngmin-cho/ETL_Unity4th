using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;
using System;
using System.Net.Mail;


public class ClientNetworkTransformWithExtrapolation : NetworkTransformWithExtrapolation
{

    protected override void Update()
    {
        base.Update();

        if (IsOwner)
        {
            clientInput = new Vector3(Input.GetAxis("Horizontal"),
                                       0.0f,
                                       Input.GetAxis("Vertical"));
        }
    }
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }

}
