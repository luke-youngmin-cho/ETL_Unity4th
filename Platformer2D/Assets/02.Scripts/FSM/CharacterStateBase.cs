using System;
using UnityEngine;
using CharacterController = Platformer.Controllers.CharacterController;

namespace Platformer.FSM
{
    public enum CharacterStateID
    {
        None,
        Idle,
        Move,
        Jump,
        DownJump,
        DoubleJump,
        Fall,
        Land,
        Crouch,
        Hurt,
        Die,
        Attack,
        WallSlide,
        Edge,
        EdgeClimb,
        Ladder,
    }

    public class CharacterStateBase : IState<CharacterStateID>
    {
        public virtual CharacterStateID id { get; }

        public virtual bool canExecute => true;

        protected StateMachine<CharacterStateID> machine;
        protected CharacterController controller;
        protected Transform transform;
        protected Rigidbody2D rigidbody;
        protected Animator animator;

        public CharacterStateBase(StateMachine<CharacterStateID> machine)
        {
            this.machine = machine;
            this.controller = machine.owner;
            this.transform = machine.owner.transform;
            this.rigidbody = machine.owner.GetComponent<Rigidbody2D>();
            this.animator = machine.owner.GetComponentInChildren<Animator>();
        }

        public virtual void OnStateEnter()
        {
            Debug.Log($"Entered in {id}");
        }

        public virtual void OnStateExit()
        {
        }

        public virtual CharacterStateID OnStateUpdate()
        {
            return id;
        }

        public virtual void OnStateFixedUpdate()
        {
        }
    }
}
