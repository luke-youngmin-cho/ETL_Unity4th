using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LocalLobby
{
    public string id
    {
        get => lobbyData.id;
        set
        {
            if (lobbyData.id == value)
                return;

            lobbyData.id = value;
            onChanged?.Invoke(this);
        }
    }

    public string code
    {
        get => lobbyData.lobbyCode;
        set
        {
            if (lobbyData.lobbyCode == value)
                return;

            lobbyData.lobbyCode = value;
            onChanged?.Invoke(this);
        }
    }

    public string relayJoinCode
    {
        get => lobbyData.relayJoinCode;
        set
        {
            if (lobbyData.relayJoinCode == value)
                return;

            lobbyData.relayJoinCode = value;
            onChanged?.Invoke(this);
        }
    }

    public string name 
    { 
        get => lobbyData.name;
        set
        {
            if (lobbyData.name == value)
                return;

            lobbyData.name = value;
            onChanged?.Invoke(this);
        }
    }

    public bool isPrivate
    {
        get => lobbyData.isPrivate;
        set
        {
            if (lobbyData.isPrivate == value)
                return;

            lobbyData.isPrivate = value;
            onChanged?.Invoke(this);
        }
    }

    public int maxPlayerCount
    {
        get => lobbyData.maxPlayers;
        set
        {
            if (lobbyData.maxPlayers == value)
                return;

            lobbyData.maxPlayers = value;
            onChanged?.Invoke(this);
        }
    }


    public struct LobbyData
    {
        public string id;
        public string lobbyCode;
        public string relayJoinCode;
        public string name;
        public bool isPrivate;
        public int maxPlayers;

        public LobbyData(string id, string code, string relayJoinCode, string name, bool isPrivate, int maxPlayerCount)
        {
            this.id = id;
            this.lobbyCode = code;
            this.relayJoinCode = relayJoinCode;
            this.name = name;
            this.isPrivate = isPrivate;
            this.maxPlayers = maxPlayerCount;
        }
    }
    public LobbyData lobbyData;
    public event Action<LocalLobby> onChanged;

    public Dictionary<string, LocalLobbyUser> _localLobbyUsers = new Dictionary<string, LocalLobbyUser>();

    /// <summary>
    /// Create LocalLobby Data with Remote Lobby
    /// </summary>
    /// <param name="lobby"> Remote lobby </param>
    /// <returns> local data </returns>
    public static LocalLobby Create(Lobby lobby)
    {
        LocalLobby localLobby = new LocalLobby();
        localLobby.ApplyRemoteData(lobby);
        return localLobby;
    }

    public void AddUser(LocalLobbyUser user)
    {
        if (_localLobbyUsers.ContainsKey(user.id) == false)
        { 
            _localLobbyUsers.Add(user.id, user);
            onChanged?.Invoke(this);
        }
    }

    public void RemoveUser(LocalLobbyUser user)
    {
        if (_localLobbyUsers.ContainsKey (user.id))
        {
            _localLobbyUsers.Remove(user.id);
            onChanged?.Invoke(this);
        }
    }


    public void ApplyRemoteData(Lobby lobby)
    {
        LobbyData tmpData = new LobbyData();
        tmpData.id = lobby.Id;
        tmpData.lobbyCode = lobby.LobbyCode;
        tmpData.relayJoinCode = lobby.Data != null ?
            (lobby.Data.ContainsKey("RelayJoinCode") ? lobby.Data["RelayJoinCode"].Value : null) : null;
        tmpData.isPrivate = lobby.IsPrivate;
        tmpData.name = lobby.Name;
        tmpData.maxPlayers = lobby.MaxPlayers;

        _localLobbyUsers.Clear();
        foreach (Player player in lobby.Players)
        {
            if (player.Data != null)
            {
                if (_localLobbyUsers.ContainsKey(player.Id) == false)
                {
                    _localLobbyUsers.Add(player.Id, new LocalLobbyUser()
                    {
                        userData
                            = new LocalLobbyUser.UserData(isHost: lobby.HostId == player.Id,
                                                          displayName: player.Data != null && player.Data.ContainsKey("DisplayName") ?
                                                                        player.Data["DisplayName"].Value : string.Empty,
                                                          id: player.Id)
                    });
                }
            }
        }

        lobbyData = tmpData;
        onChanged?.Invoke(this);
    }
}
