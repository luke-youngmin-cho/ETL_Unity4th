using RPG.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace RPG.Controllers
{
    public class PlayerController : CharacterController
    {
        public static IEnumerable<ulong> GetAllSpawnedID => _spawned.Keys;
        static Dictionary<ulong, PlayerController> _spawned = new Dictionary<ulong, PlayerController>();
        public static event Action<ulong, PlayerController> onSpawned;
        public static event Action<ulong, PlayerController> onDespawned;

        public static PlayerController GetSpawned(ulong clientID)
        {
            if (_spawned.TryGetValue(clientID, out PlayerController playerController))
                return playerController;

            return null;
        }


        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (IsOwner)
            {
                InputSystem.instance.maps["Player"]
                .RegisterAxisAction("Horizontal", (value) => horizontal = value);
                InputSystem.instance.maps["Player"]
                    .RegisterAxisAction("Vertical", (value) => vertical = value);
                InputSystem.instance.maps["Player"]
                    .RegisterKeyDownAction(KeyCode.LeftShift, () => moveGain = 4.0f);
                InputSystem.instance.maps["Player"]
                    .RegisterKeyUpAction(KeyCode.LeftShift, () => moveGain = 2.0f);
                //InputSystem.instance.maps["Player"]
                //    .RegisterKeyDownAction(KeyCode.Space, () => ChangeState(State.Jump));

                moveGain = 2.0f;
            }
            _spawned.Add(OwnerClientId, this);
            onSpawned?.Invoke(OwnerClientId, this);
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            onDespawned?.Invoke(OwnerClientId, this);
            _spawned.Remove(OwnerClientId);
        }
    }
}