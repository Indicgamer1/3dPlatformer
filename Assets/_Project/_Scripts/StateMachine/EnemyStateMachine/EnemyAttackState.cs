using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Platformer
{
    public class EnemyAttackState : EnemyBaseState
    {
        readonly NavMeshAgent agent;
        readonly Transform player;
            
        public EnemyAttackState(Enemy enemy, Animator animator, NavMeshAgent agent, Transform player) : base(enemy, animator)
        {
            this.agent = agent;
            this.player = player;
        }

        public override void OnEnter()
        {
            Debug.Log("Entered AttackState");
            animator.CrossFade(attackHash, crossFadeTime);
        }

        public override void Update()
        {
            agent.SetDestination(player.position);
            enemy.Attack();
        }
        
    }
}