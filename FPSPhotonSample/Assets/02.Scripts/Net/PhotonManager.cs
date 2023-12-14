using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System;
using System.Collections.Generic;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager instance;
    private readonly string _version = "0.0.1";
    public string userID;
    public string userPW;
    public event Action<List<RoomInfo>> onRoomListUpdate;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        PhotonNetwork.GameVersion = _version;

        if (PhotonNetwork.IsConnected == false)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public bool TrySetUserID(string id, string pw)
    {
        if (string.IsNullOrEmpty(id) ||
            string.IsNullOrEmpty(pw))
        {
            Debug.LogWarning("[PhotonManager] : Failed to set user ID. id or pw is empty.");
            return false;
        }

        userID = id;
        userPW = pw;
        PhotonNetwork.NickName = id; // todo -> DB 에서 이 id 로 NickName 가져와야함. 없으면 최초로그인이므로 닉네임 설정창 띄워줘야함.
        return true;
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        if (PhotonNetwork.InLobby)
        {
            SetCustomProperty(PhotonNetwork.LocalPlayer, new Hashtable
            {
                { CustomPropertyKeys.PLAYER_STATE_IN_ROOM, PlayerStateInRoom.NotInRoom },
            });
            Debug.Log($"[PhotonNetwork] : {PhotonNetwork.NickName} is joined lobby.");
        }
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        // todo -> Move to local room.
        PhotonNetwork.CurrentRoom.SetMasterClient(PhotonNetwork.LocalPlayer);
        Debug.Log($"[PhotonManager] : Created Room. Master : {PhotonNetwork.LocalPlayer.NickName}");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if (PhotonNetwork.InRoom)
        {
            SetCustomProperty(PhotonNetwork.LocalPlayer, new Hashtable
            {
                { CustomPropertyKeys.PLAYER_STATE_IN_ROOM, PlayerStateInRoom.NotReady },
            });
            Debug.Log($"[PhotonManager] : Successfully joined room, Total players : {PhotonNetwork.CurrentRoom.PlayerCount}");
            // todo -> Move to local room.
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SetCustomProperty(PhotonNetwork.LocalPlayer, new Hashtable
            {
                { CustomPropertyKeys.PLAYER_STATE_IN_ROOM, PlayerStateInRoom.NotInRoom },
            });
        // todo -> Move to local lobby.
    }

    public override void OnLeftLobby()
    {
        base.OnLeftLobby();
        // todo -> Move to login.
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log($"[PhotonManager] : {newPlayer.NickName} entered room.");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log($"[PhotonManager] : {otherPlayer.NickName} left room.");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        onRoomListUpdate?.Invoke(roomList);
    }

    private void SetCustomProperty(Player player, Hashtable hashtable)
    {
        player.SetCustomProperties(hashtable);
    }
}
