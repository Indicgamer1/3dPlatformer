using UnityEditor.Animations;
using UnityEngine;

namespace Platformer
{
    public abstract class BaseState : IState
    {
        protected readonly PlayerController player;
        protected readonly Animator animator;
        
        protected readonly int LocomotionHash = Animator.StringToHash("Locomotion");
        
        protected readonly float crossFadeTime = 0.1f;
        protected BaseState(PlayerController playerController, Animator animator)
        {
            this.player = playerController;
            this.animator = animator;
        }
        
        public virtual void OnEnter()
        {
            //noop
        }
    
        public virtual void Update()
        {
            //noop
        }
    
        public virtual void FixedUpdate()
        {
            //noop
        }
    
        public virtual void OnExit()
        {
            Debug.Log("BaseState.OnExit");
            //noop
        }
    }
}
