using Unity.Netcode;
using UnityEngine;

public class ClientBehaviour : NetworkBehaviour
{
    [SerializeField] private Transform _hand;
    private NetworkVariable<bool> _isPicking
        = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] LayerMask _interactableMask;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    private void Update()
    {
        if (!IsOwner)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            Collider[] cols =
                    Physics.OverlapSphere(transform.position, 1.0f, _interactableMask);

            if (cols.Length > 0)
            {
                if (cols[0].TryGetComponent(out IInteractable interactable))
                {
                    InteractionServerRPC(OwnerClientId, cols[0].GetComponent<NetworkObject>().NetworkObjectId);
                }
            }
        }
    }

    [ServerRpc]
    void InteractionServerRPC(ulong clientID, ulong interactableObjectID)
    {
        Pickable.spawned[interactableObjectID].Interaction(OwnerClientId, gameObject);
        Debug.Log($"{clientID} is picking {interactableObjectID}");
    }


    private void FixedUpdate()
    {
        // 주인만 위치조작가능해야하므로
        if (!IsOwner)
            return;

        transform.position += new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")) * Time.fixedDeltaTime;
    }
}
