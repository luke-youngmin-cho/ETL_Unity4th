using RPG.Controllers;
using RPG.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusUI : UIMonoBehaviour
{
    [SerializeField] private PlayerStatusSlot _slotPrefab;
    [SerializeField] private Transform _content;
    private Dictionary<ulong, PlayerStatusSlot> _slots;

    public void ShowChatBalloon(ulong clientID, string message)
    {
        _slots[clientID].chatBallon.Show(message);
    }

    public override void Init()
    {
        base.Init();
        _slots = new Dictionary<ulong, PlayerStatusSlot>();

        foreach (ulong clientID in PlayerController.GetAllSpawnedID)
        {
            OnSpawned(clientID);
        }

        PlayerController.onSpawned += OnSpawned;
        PlayerController.onDespawned += OnDespawned;
    }

    private void OnDestroy()
    {
        PlayerController.onSpawned -= OnSpawned;
        PlayerController.onDespawned -= OnDespawned;
    }

    private void OnSpawned(ulong clientID, PlayerController controller = null)
    {
        PlayerStatusSlot slot = Instantiate(_slotPrefab, _content);
        slot.SetUp(clientID);
        _slots.Add(clientID, slot);
    }

    private void OnDespawned(ulong clientID, PlayerController controller = null)
    {
        Destroy(_slots[clientID].gameObject);
        _slots.Remove(clientID);
    }
}
