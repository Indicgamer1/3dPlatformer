using UnityEngine;

namespace Platformer
{
    public abstract class CannonBaseState : IState
    {
        protected readonly Cannon cannon;

        public CannonBaseState(Cannon cannon)
        {
            this.cannon = cannon;
        }
        
        public void OnEnter()
        {
            //noop
        }

        public void Update()
        {
            //noop
        }

        public void FixedUpdate()
        {
            //noop
        }

        public void OnExit()
        {
            Debug.Log($"{this.GetType().Name} exited !!!");
        }
    }
}