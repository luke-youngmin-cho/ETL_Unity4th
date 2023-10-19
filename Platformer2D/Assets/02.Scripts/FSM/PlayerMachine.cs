using Platformer.Controllers;

namespace Platformer.FSM
{
    public class PlayerMachine : CharacterMachine
    {
        public PlayerMachine(CharacterController owner) : base(owner)
        {
        }
    }
}