using System;
using System.Collections.Generic;
using System.IO;
using ProjectileCurveVisualizerSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Platformer
{
    public class ProjectileShooter : MonoBehaviour
    {
        [SerializeField] FloatEventChannel attackCooldownEventChannel;
        [SerializeField] ProjectileBall ball;
        [SerializeField] ProjectileCurveVisualizer projectileVisualizer;
        [SerializeField] Transform parentShooter;
        [SerializeField] float xRotationSpeedMultiplier = 0.5f;
        [SerializeField] float yRotationSpeedMultiplier = 0.2f;
        [SerializeField] float attackDelay = 1f;
        [SerializeField] float ballSpeed = 10f;

        [Header("rotation parameters")]
        public bool CanRotate = false;
        private float currentXRotation = 0f;
        private float currentYRotation = 0f;

        private Rigidbody ballRB;
        private PlayerController player;
        private Transform cameraTransform;
        private Vector3 projectileVelocity;

        CountdownTimer attackCooldownTimer;
        
        private void Awake() => ball.releasePoint = transform;

        private void OnEnable() => ball.OnHit += () =>
        {
            CanRotate = false;
            ResetShooter();
        };

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            cameraTransform = Camera.main.transform;
            SetupTimers();
            SetBallRB();
            ball.Reset();
        }

        private void SetBallRB()
        {
            ballRB = ball.GetComponent<Rigidbody>();
            if(ballRB == null)
                ballRB = ball.gameObject.AddComponent<Rigidbody>();
        }

        private void SetupTimers()
        {
            attackCooldownTimer = new CountdownTimer(attackDelay);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) 
                CanRotate = true;
            else if (Input.GetMouseButtonUp(0))
            {
                ball.transform.position = transform.position;
                HandleAttack();
            }

            if (CanRotate)
            {
                if (!projectileVisualizer.gameObject.activeSelf)
                {
                    projectileVisualizer.gameObject.SetActive(true);
                    ball.gameObject.SetActive(true);
                }
                
                if(!attackCooldownTimer.IsRunning)
                    SetDirection();
            }
            else
            {
                if (projectileVisualizer.gameObject.activeSelf)
                {
                    projectileVisualizer.gameObject.SetActive(false);
                    ball.gameObject.SetActive(false);
                }
            }

            attackCooldownTimer.Tick(Time.deltaTime);
        }

        private void FixedUpdate() => UpdateAttackCoolDownVisual();

        private void LateUpdate()
        {
            if(!ball.IsFlying)
                ball.transform.position = transform.position;
        }

        private void SetDirection()
        {
            RotatePlayer();

            Vector2 mouseInput = Mouse.current.delta.ReadValue();

            // X-Axis Rotation
            AdjustRotation(ref currentXRotation, -mouseInput.y * xRotationSpeedMultiplier, parentShooter.right, 45f);

            // Y-Axis Rotation
            AdjustRotation(ref currentYRotation, mouseInput.x * yRotationSpeedMultiplier, parentShooter.up, 30f);

            projectileVelocity = (transform.forward * ballSpeed);
            UpdateProjectileVisualizer();
        }

        private void RotatePlayer()
        {
            Vector3 targetRotation = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
            player.HandleRotation(targetRotation);
        }

        private void AdjustRotation(ref float currentRotation, float rotationAmount, Vector3 axis, float rotationLimit)
        {
            float newRotation = currentRotation + rotationAmount;

            if (newRotation > -rotationLimit && newRotation < rotationLimit)
            {
                transform.rotation = Quaternion.AngleAxis(rotationAmount, axis) * transform.rotation;
                currentRotation = newRotation;
            }
        }

        public void UpdateProjectileVisualizer()
        {
            projectileVisualizer.VisualizeProjectileCurve(
                transform.position,
                0f, 
                projectileVelocity,
                0.25f,
                0f,
                true,
                out _,
                out _ );
        }

        private void HandleAttack()
        {
            if (ball.IsFlying || attackCooldownTimer.IsRunning) return;

            Shoot(projectileVelocity);
            attackCooldownTimer.Start();

            
        }

        public void Shoot(Vector3 force)
        {
            if(ball.IsFlying) return;
            ballRB.isKinematic = false;
            ballRB.AddForce(force, ForceMode.Impulse);
            ballRB.AddTorque(transform.right * ball.Torque);
        }

        private void ResetShooter()
        {
            transform.rotation = parentShooter.rotation;
            currentXRotation = 0f;
            currentYRotation = 0f;
        }

        private void ToggleVisual(bool value)
        {
            projectileVisualizer.gameObject.SetActive(value);
            ball.ToggleCollider(value);
        }

        private void UpdateAttackCoolDownVisual()
        {
            attackCooldownEventChannel.Invoke(attackCooldownTimer.Progress);
        }
    }
}