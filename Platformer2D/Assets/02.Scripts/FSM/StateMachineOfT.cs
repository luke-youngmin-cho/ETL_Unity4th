using System;
using CharacterController = Platformer.Controllers.CharacterController;
using System.Collections.Generic;

namespace Platformer.FSM
{
    public class StateMachine<T>
        where T : Enum
    {
        public T currentStateID;
        protected Dictionary<T, IState<T>> states;

        public void Init(IDictionary<T, IState<T>> copy)
        {
            states = new Dictionary<T, IState<T>>(copy);
        }

        public void UpdateState()
        {
            ChangeState(states[currentStateID].OnStateUpdate());
        }

        public bool ChangeState(T newStateID)
        {
            // 바꾸려는 상태가 현재 상태와 동일하면 바꾸지않음
            if (Comparer<T>.Default.Compare(newStateID, currentStateID) == 0)
                return false;

            // 바꾸려는 상태가 실행가능하지 않다면 바꾸지 않음
            if (states[newStateID].canExecute == false)
                return false;

            states[currentStateID].OnStateExit(); // 기존 상태에서 탈출
            currentStateID = newStateID; // 상태 갱신
            states[currentStateID].OnStateEnter(); // 새로운 상태로 진입
            return true;
        }
    }
}