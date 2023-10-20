using Platformer.FSM;
using System.Linq;
using UnityEngine;

namespace Platformer.Controllers
{
    public class PlayerController : CharacterController
    {
        public override float horizontal => Input.GetAxis("Horizontal");

        public override float vertical => Input.GetAxis("Vertical");


        private void Start()
        {
            machine = new PlayerMachine(this);
            var machineData = StateMachineDataSheet.GetPlayerData(machine);
            machine.Init(machineData);
        }

        protected override void Update()
        {
            base.Update();
            
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                if (machine.ChangeState(CharacterStateID.DownJump) == false &&
                    machine.ChangeState(CharacterStateID.Jump) == false &&
                    Input.GetKeyDown(KeyCode.LeftAlt))
                {
                    machine.ChangeState(CharacterStateID.DoubleJump);
                }
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                machine.ChangeState(CharacterStateID.Crouch);
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                if (machine.currentStateID == CharacterStateID.Crouch)
                    machine.ChangeState(CharacterStateID.Idle);
            }
        }
    }
}