using Platformer.FSM;

namespace Platformer.Controllers
{
    public class SlugController : EnemyController
    {
        protected override void Awake()
        {
            base.Awake();
            machine = new EnemyMachine(this);
            var machineData = StateMachineDataSheet.GetSlugData(machine);
            machine.Init(machineData);
            onHpDepleted += (amount) => machine.ChangeState(CharacterStateID.Hurt);
            onHpMin += () => machine.ChangeState(CharacterStateID.Die);
        }
    }
}