using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Platformer
{
    public class FlyState : BaseState
    {
        public FlyState(PlayerController playerController, Animator animator) : base(playerController, animator){}

        public override void OnEnter()
        {
            Debug.Log("Flying");
        }
        public override void FixedUpdate()
        {
            player.HandleFly();
            player.HandleMovement();
        }

        public override void OnExit() => Debug.Log("Landed");
    }
}