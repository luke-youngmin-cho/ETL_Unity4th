using Platformer.FSM;
using System.Linq;
using UnityEngine;

namespace Platformer.Controllers
{
    public class PlayerController : CharacterController
    {
        public override float horizontal => Input.GetAxis("Horizontal");

        public override float vertical => Input.GetAxis("Vertical");

        private PlayerMachine _machine;

        private void Start()
        {
            _machine = new PlayerMachine(this);
            var machineData = StateMachineDataSheet.GetPlayerData(_machine);
            _machine.Init(machineData);
            _machine.currentStateID = machineData.First().Key; 
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKey(KeyCode.LeftAlt))
                _machine.ChangeState(CharacterStateID.Jump);

            _machine.UpdateState();
        }
    }
}