using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Platformer
{
    public class EnemyWanderState : EnemyBaseState
    {
        readonly NavMeshAgent agent;
        readonly Vector3 startPosition;
        readonly float wanderRadius;
        
        public EnemyWanderState(Enemy enemy, Animator animator, NavMeshAgent agent, float wanderRadius) : base(enemy, animator)
        {
            this.agent = agent;
            this.startPosition = enemy.transform.position;
            this.wanderRadius = wanderRadius;
        }

        public override void OnEnter()
        {
            animator.CrossFade(walkHash, crossFadeTime);
        }

        public override void Update()
        {
            if (HasReachedDestination())
            {
                Vector3 randomDirection = Random.insideUnitSphere*wanderRadius;
                randomDirection += startPosition;
                NavMeshHit Hit;
                NavMesh.SamplePosition(randomDirection, out Hit, wanderRadius, 1);
                Vector3 finalPosition = Hit.position;
                agent.SetDestination(finalPosition);
            }
        }

        private bool HasReachedDestination()
        {
            return !agent.pathPending
                    && agent.remainingDistance <= agent.stoppingDistance
                    && (!agent.hasPath || agent.velocity.sqrMagnitude <= 0.01f);
        }
    }
}