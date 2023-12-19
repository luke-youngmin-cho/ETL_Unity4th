using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace CopycatOverCooked.GamePlay
{
    public interface IInteractable
    {
        InteractableType type { get; }
        void BeginInteraction(Interactor interactor);
        void EndInteraction(Interactor interactor);
    }
}