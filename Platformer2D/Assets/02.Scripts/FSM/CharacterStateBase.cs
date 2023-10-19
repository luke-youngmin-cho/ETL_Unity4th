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

    public abstract class CharacterStateBase : StateBase<CharacterStateID>
    {
        protected StateMachine<CharacterStateID> machine;
        protected CharacterController controller;
        protected Transform transform;
        protected Rigidbody2D rigidbody;
        protected Animator animator;

        public CharacterStateBase(CharacterMachine machine)
            : base(machine)
        {
            this.machine = machine;
            this.controller = machine.owner;
            this.transform = machine.owner.transform;
            this.rigidbody = machine.owner.GetComponent<Rigidbody2D>();
            this.animator = machine.owner.GetComponentInChildren<Animator>();
        }
    }
}
