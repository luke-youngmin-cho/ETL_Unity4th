using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LocalLobbyUser
{
    public bool isHost
    {
        get => userData.isHost;
        set
        {
            if (userData.isHost == value)
                return;

            userData.isHost = value;
            onChanged?.Invoke(this);
        }
    }

    public string displayName
    {
        get => userData.displayName;
        set
        {
            if (userData.displayName == value)
                return;

            userData.displayName = value;
            onChanged?.Invoke(this);
        }
    }

    public string id
    {
        get => userData.id;
        set
        {
            if (userData.id == value)
                return;

            userData.id = value;
            onChanged?.Invoke(this);
        }
    }

    public struct UserData
    {
        public bool isHost;
        public string displayName;
        public string id;

        public UserData(bool isHost, string displayName, string id)
        {
            this.isHost = isHost;
            this.displayName = displayName;
            this.id = id;
        }
    }
    public UserData userData
    {
        get => _userData;
        set
        {
            _userData = value;
            onChanged?.Invoke(this);
        }
    }

    private UserData _userData;

    public event Action<LocalLobbyUser> onChanged;

}
