using System;
using KBCore.Refs;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Platformer
{
    [RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof(PlayerDetector))]
   public class Enemy : Entity,IDamageable
   {
       [SerializeField, Self] UnityEngine.AI.NavMeshAgent agent;
       [SerializeField, Self] PlayerDetector playerDetector;
       [SerializeField, Child] Animator animator;
       [SerializeField] AudioSource audioSource;
       
       [SerializeField] float wanderRadius = 10f;
       [FormerlySerializedAs("delayBetweenAttacks")] [SerializeField] float attackDelay = 0.5f;
       [SerializeField] int damage = 10;
       [SerializeField] int health = 100;
       [SerializeField] Transform healthBarObj;

       private Vector3 startPos;
       
       StateMachine stateMachine;
       CountdownTimer attackTimer;
       Image healthBar;
       private void OnValidate() => this.ValidateRefs();
   
       private void Start()
       {
           startPos = transform.position;
           //healthbar setup
           attackTimer = new CountdownTimer(attackDelay);
           //healthBar = healthBarObj.GetComponent<Image>();
           
           stateMachine = new StateMachine();
           
           EnemyWanderState wanderState = new EnemyWanderState(this, animator, agent, wanderRadius);
           EnemyChaseState chaseState = new EnemyChaseState(this, animator, agent, playerDetector.Player);
           EnemyAttackState attackState = new EnemyAttackState(this, animator, agent, playerDetector.Player);
           
           At(wanderState, chaseState, new FuncPredicate(() => playerDetector.CanDetectPlayer()));
           At(chaseState, wanderState, new FuncPredicate(() => !playerDetector.CanDetectPlayer()));
           At(chaseState, attackState, new FuncPredicate(() => playerDetector.CanAttackPlayer()));
           At(attackState, chaseState, new FuncPredicate(() => !playerDetector.CanAttackPlayer()));
           stateMachine.SetState(wanderState);
       }

       private void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
       private void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);
       private void Update()
       {
           stateMachine.Update();
           attackTimer.Tick(Time.deltaTime);
       }

       private void FixedUpdate()
       {
           if (stateMachine == null)
               Debug.Log("No statemachine in enemy" );
           stateMachine.FixedUpdate();
       }

       public void Attack()
       {
           if (attackTimer.IsRunning)
           {
               return;
           }
           
           audioSource.Play();
           playerDetector.PlayerHealth.TakeDamage(damage);
           attackTimer.Start();
           Debug.Log("Attacking");
       }

       public void TakeDamage(int damageAmount)
       {
           health -= damageAmount;
       }

       private void OnDrawGizmos()
       {
           Handles.color = Color.green;
           Vector3 center;
           if(startPos != Vector3.zero)
               center = startPos;
           else
                center = transform.position;
           Handles.DrawWireDisc(center, transform.up, wanderRadius);
       }
   }
}
