using Platformer.FSM.Character;
using System.Collections.Generic;

namespace Platformer.FSM
{
    public static class StateMachineDataSheet
    {
        public static IDictionary<CharacterStateID, IState<CharacterStateID>> GetPlayerData(StateMachine<CharacterStateID> machine)
        {
            IState<CharacterStateID> state = new Move(machine);

            return new Dictionary<CharacterStateID, IState<CharacterStateID>>()
            {
                { CharacterStateID.Idle, new Idle(machine) },
                { CharacterStateID.Move, new Move(machine) },
            };
        }

    }
}
