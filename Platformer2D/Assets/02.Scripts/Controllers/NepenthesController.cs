using Platformer.FSM;

namespace Platformer.Controllers
{
    public class NepenthesController : EnemyController
    {
        protected override void Awake()
        {
            base.Awake();
            machine = new EnemyMachine(this);
            var machineData = StateMachineDataSheet.GetNepenthesData(machine);
            machine.Init(machineData);
            onHpDepleted += (amount) => machine.ChangeState(CharacterStateID.Hurt);
            onHpMin += () => machine.ChangeState(CharacterStateID.Die);
        }
    }
}