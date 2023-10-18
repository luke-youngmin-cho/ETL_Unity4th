using System;

namespace Platformer.FSM
{
    public interface IState<T>
        where T : Enum
    {
        T id { get; }
        bool canExecute { get; }

        void OnStateEnter();
        void OnStateExit();
        T OnStateUpdate();
        void OnStateFixedUpdate();
    }
}