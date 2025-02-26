using UnityEngine;

namespace Platformer
{
    public class LocomotionState : BaseState
    {
        public LocomotionState(PlayerController playerController, Animator animator) : base(playerController, animator){}

        public override void OnEnter()
        {
            animator.CrossFade(LocomotionHash, crossFadeTime);
        }

        public override void FixedUpdate()
        {
            player.HandleMovement();
        }
    }
}