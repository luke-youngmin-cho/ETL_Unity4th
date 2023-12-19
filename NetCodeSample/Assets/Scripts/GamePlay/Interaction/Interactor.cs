using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace CopycatOverCooked.GamePlay
{
    public class Interactor : NetworkBehaviour
    {
        public static Dictionary<ulong, Interactor> spawned = new Dictionary<ulong, Interactor>();

        public Transform hand;
        public IInteractable currentInteractable;


        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            spawned.Add(OwnerClientId, this);
        }

        private void Update()
        {
            if (!IsOwner)
                return;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                // 상호작용하고있는게 없을때
                if (currentInteractable == null)
                {
                    currentInteractable = DetectInteractable();
                    currentInteractable?.BeginInteraction(this);
                }
                // 상호작용중인게 있을때
                else
                {
                    currentInteractable.EndInteraction(this);
                    currentInteractable = null;
                }
            }
               
        }

        private IInteractable DetectInteractable()
        {
            // todo -> cast interactable.
            return null;
        }
    }
}
