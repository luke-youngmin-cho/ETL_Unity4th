using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class CreateRoomUI : MonoBehaviour
{
    [SerializeField] TMP_InputField _roomName;
    [SerializeField] Button _confirm;
    [SerializeField] Button _cancel;

    private void Start()
    {
        _confirm.onClick.AddListener(() =>
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsOpen = true;
            roomOptions.IsVisible = true;
            roomOptions.MaxPlayers = 8;
            roomOptions.CleanupCacheOnLeave = true;
            if (PhotonNetwork.CreateRoom(_roomName.text, roomOptions, TypedLobby.Default))
            {
                SceneManager.LoadScene("Room");
            }
        });
        _cancel.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
