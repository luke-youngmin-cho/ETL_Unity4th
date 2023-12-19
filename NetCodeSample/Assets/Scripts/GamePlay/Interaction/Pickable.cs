using Unity.Netcode;
using UnityEngine;
using UnityEngine.Android;

namespace CopycatOverCooked.GamePlay
{
    public abstract class Pickable : NetworkBehaviour, IInteractable, IUsable
    {
        NetworkVariable<ulong> _pickingClientID = new NetworkVariable<ulong>();
        const ulong EMPTY_CLIENT_ID = ulong.MaxValue;

        public InteractableType type => throw new System.NotImplementedException();

        public void BeginInteraction(Interactor interactor)
        {
            if (_pickingClientID.Value != EMPTY_CLIENT_ID)
                return;

            PickUpServerRpc(interactor.OwnerClientId);
        }

        public void EndInteraction(Interactor interactor)
        {
            if (_pickingClientID.Value == EMPTY_CLIENT_ID)
                return;

            OnEndInteraction(DetectInteractable());
        }

        [ServerRpc(RequireOwnership = false)]
        public void PickUpServerRpc(ulong clientID)
        {
            Interactor interactor = Interactor.spawned[clientID];
            // Pick up
            if (NetworkObject.TrySetParent(interactor.NetworkObject))
            {
                transform.localPosition = interactor.hand.localPosition;
                _pickingClientID.Value = clientID;
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void DropServerRpc(ulong clientID = ulong.MaxValue)
        {
            NetworkObject.TrySetParent(default(Transform));
            _pickingClientID.Value = EMPTY_CLIENT_ID;
        }

        protected virtual void OnEndInteraction(IInteractable other)
        {
            DropServerRpc();
        }

        private IInteractable DetectInteractable()
        {
            // todo -> cast interactable.
            return null;
        }


        public abstract void Use(NetworkObject user);
    }
}
