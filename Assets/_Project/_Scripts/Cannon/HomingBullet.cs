using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Platformer
{
    public class HomingMissile : MonoBehaviour, IBullet
    {
        [SerializeField] float speed = 10f; // Speed of the missile
        [SerializeField] float turnSpeed = 2f; // How quickly the missile adjusts its direction
        [SerializeField] float gravity = 9.8f; // Gravity applied to the missile
        [SerializeField] float maxHommingTimer = 3f;
        [SerializeField] int damage = 5;
        [SerializeField] float damageRadius = 1f;
        [SerializeField] GameObject BlastEffect;
        

        private Transform target; // Target of the missile

        private bool isHoming = false; // Flag to determine if the missile is active

        Rigidbody rb;
        CountdownTimer homingTimer;
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            homingTimer = new CountdownTimer(maxHommingTimer);
            homingTimer.OnTimerStopped += () => DestroyObj();
            homingTimer.Start();
        }

        private void Update()
        {
            homingTimer.Tick(Time.deltaTime);
            
            if (!isHoming || target == null)
                return;

            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            // Scale turnSpeed based on proximity
            float adjustedTurnSpeed = turnSpeed;
            if (distanceToTarget < 5f) // Adjust threshold as necessary
            {
                adjustedTurnSpeed = Mathf.Lerp(turnSpeed, turnSpeed * 3f, (5f - distanceToTarget) / 5f);
            }
            
            // Calculate desired velocity towards the target
            Vector3 desiredVelocity = directionToTarget * speed;

            Vector3 currentVelocity = rb.velocity;
            // Gradually adjust the velocity towards the desired velocity
            Vector3 newVelocity = Vector3.Lerp(currentVelocity, desiredVelocity, adjustedTurnSpeed * Time.deltaTime);

            // Apply gravity
            newVelocity += Vector3.down * gravity * Time.deltaTime;

            // Move the missile
            rb.velocity = newVelocity;

            // Adjust rotation to face the movement direction
            if (rb.velocity.magnitude > 0.1f)
            {
                transform.LookAt(transform.position + rb.velocity);
            }
        }
        
        private void OnCollisionEnter(Collision other)
        {
            isHoming = false;
            Collider[] colliders = Physics.OverlapSphere(other.contacts[0].point, damageRadius);
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out Health health))
                    health.TakeDamage(damage);
            }

            homingTimer.Stop();
        }

        public void DestroyObj()
        {
            if (BlastEffect != null)
                Instantiate(BlastEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        public void Launch(Vector3 velocity, Transform target = null)
        {
            this.target = target;
            rb.velocity = velocity;
            isHoming = true;
        }
    }
}