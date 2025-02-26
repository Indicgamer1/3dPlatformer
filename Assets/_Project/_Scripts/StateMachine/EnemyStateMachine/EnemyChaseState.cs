using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Platformer
{
    public class EnemyChaseState : EnemyBaseState
    {
        readonly NavMeshAgent agent;
        readonly Transform target;
        
        public EnemyChaseState(Enemy enemy, Animator animator, NavMeshAgent agent, Transform target) : base(enemy, animator)
        {
            this.agent = agent;
            this.target = target;
        }

        public override void OnEnter()
        {
            Debug.Log("Chasing");
            animator.CrossFade(runHash, crossFadeTime);
        }

        public override void Update()
        {
            if(target != null) 
                agent.SetDestination(target.position);
        }
    }
}