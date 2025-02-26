using System;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

namespace Platformer
{
    public class EnemyBaseState : IState
    {
        protected readonly Enemy enemy;
        protected readonly Animator animator;
        
        protected const float crossFadeTime = 0.1f;
        
        //Animation Hashes
        protected static readonly int idleHash = Animator.StringToHash("IdleNormal");
        protected static readonly int walkHash = Animator.StringToHash("WalkFWD");
        protected static readonly int runHash = Animator.StringToHash("RunFWD");
        protected static readonly int attackHash = Animator.StringToHash("Attack01");
        protected static readonly int DeathHash = Animator.StringToHash("Die");

        public EnemyBaseState(Enemy enemy, Animator animator)
        {
            this.enemy = enemy;
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
            Debug.Log($"{this.GetType().Name} Exited"); 
        }
    }
}