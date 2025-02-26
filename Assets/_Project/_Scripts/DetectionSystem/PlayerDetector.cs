using System;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Platformer
{
    public class PlayerDetector : MonoBehaviour
    {
        [SerializeField] float detectionAngle = 60f;
        [SerializeField] float detectionRadius = 10f;
        [SerializeField] float innerDetectionRadius = 3f;
        [SerializeField] float detectionCoolDownTime = 1f;
        [SerializeField] float attackRange = 1f;
        
        public Transform Player { get; private set; }
        public Health PlayerHealth { get; private set; }
        private CountdownTimer detectionTimer;
        
        IDetectionStrategy coneDetectionStrategy;
        IDetectionStrategy sphereDetectionStrategy;
        private void Awake()
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
            PlayerHealth = Player.gameObject.GetComponent<Health>();
            if(Player == null)
                Debug.Log("player is null");
            else if(PlayerHealth == null)
                Debug.Log("player health is null");
            else
                Debug.Log("player detector has player & playerhealth");
        }

        private void Start()
        {
            detectionTimer = new CountdownTimer(detectionCoolDownTime);
            coneDetectionStrategy = new ConeDetectionStrategy(detectionAngle, detectionRadius);
            sphereDetectionStrategy = new SphereDetectionStrategy(innerDetectionRadius);
        }

        private void Update() => detectionTimer.Tick(Time.deltaTime);

        public bool CanDetectPlayer()
        {
            return detectionTimer.IsRunning 
                   || coneDetectionStrategy.Execute(Player, transform, detectionTimer)
                   || sphereDetectionStrategy.Execute(Player, transform, detectionTimer);
        }

        public bool CanAttackPlayer()
        {
            Vector3 vectorToPlayer = Player.position - transform.position;

            return vectorToPlayer.magnitude <= attackRange;
        }
        
        public void SetDetectionStrategy(IDetectionStrategy strategy) => this.coneDetectionStrategy = strategy;

        private void OnDrawGizmos()
        {
            Vector3 center = transform.position;
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(center, innerDetectionRadius);

            Gizmos.color = Color.blue;
            //Euler rotates detector forward dir by half detection angle respect to Y-axis & radi makes the vector equally long
            Vector3 leftConeVector = Quaternion.Euler(0f, detectionAngle/2 , 0f) * transform.forward * detectionRadius;
            //rotates negatively respect to Y-axis
            Vector3 rightConeVector = Quaternion.Euler(0f, -detectionAngle/2 , 0f) * transform.forward * detectionRadius;
            
            Gizmos.DrawLine(center, center + leftConeVector);
            Gizmos.DrawLine(center, center + rightConeVector);
            
            // draw an arc connecting these lines
            Handles.color = Color.blue;
            Handles.DrawWireArc(center, transform.up, leftConeVector, -detectionAngle, detectionRadius);
            
        }
    }
}