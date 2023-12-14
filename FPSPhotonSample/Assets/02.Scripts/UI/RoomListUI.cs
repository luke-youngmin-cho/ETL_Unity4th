using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListUI : MonoBehaviour
{
    [SerializeField] Button _create;
    [SerializeField] Button _join;
    public RoomInfo selected
    {
        get
        {
            if (_selected != null &&
                _selected.RemovedFromList)
            {
                selected = null;
            }
            return _selected;
        }
        set
        {
            _selected = value;
            _join.interactable = value != null;
        }
    }
    private RoomInfo _selected;
    [SerializeField] RoomListSlot _slotPrefab;
    List<RoomListSlot> _slots = new List<RoomListSlot>();
    [SerializeField] Transform _content;

    private void OnEnable()
    {
        PhotonManager.instance.onRoomListUpdate += Refresh;
    }

    private void OnDisable()
    {
        PhotonManager.instance.onRoomListUpdate -= Refresh;
    }

    // Dont do this plz !! just for testing.
    // Make pooling system.
    public void Refresh(List<RoomInfo> roomInfos)
    {
        for (int i = _slots.Count - 1; i >= 0; i--)
        {
            Destroy(_slots[i]); ;
        }

        RoomListSlot slot;
        for (int i = 0; i < roomInfos.Count; i++)
        {
            slot = Instantiate(_slotPrefab, _content);
            slot.roomListUI = this;
            slot.Refresh(roomInfos[i]);
        }
    }
}
