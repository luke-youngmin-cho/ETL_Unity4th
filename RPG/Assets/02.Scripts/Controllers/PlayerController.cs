using RPG.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Controllers
{
    public class PlayerController : CharacterController
    {
        private void Start()
        {
            InputSystem.instance.maps["Player"]
                .RegisterAxisAction("Horizontal", (value) => horizontal = value);
            InputSystem.instance.maps["Player"]
                .RegisterAxisAction("Vertical", (value) => vertical = value);
            InputSystem.instance.maps["Player"]
                .RegisterKeyDownAction(KeyCode.LeftShift, () => moveGain = 4.0f);
            InputSystem.instance.maps["Player"]
                .RegisterKeyUpAction(KeyCode.LeftShift, () => moveGain = 2.0f);
            InputSystem.instance.maps["Player"]
                .RegisterKeyDownAction(KeyCode.Space, () => ChangeState(State.Jump));

            moveGain = 2.0f;
        }
    }
}