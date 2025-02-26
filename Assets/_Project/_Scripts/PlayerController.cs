using KBCore.Refs;
using UnityEngine;
using Cinemachine;
using System.Collections.Generic;

namespace Platformer
{
    public class PlayerController : ValidatedMonoBehaviour
    {
        [SerializeField, Self] Rigidbody rb;
        [SerializeField, Self] GroundChecker groundChecker;
        [SerializeField, Self] Animator animator;
        [SerializeField, Anywhere] InputReader input;
        [SerializeField, Anywhere] CinemachineFreeLook freeLookCamera;

        [Header("Movement Settings")]
        [SerializeField] float moveSpeed = 700f;
        [SerializeField] float drag = 2f;
        [SerializeField] float rotationSpeed = 15f;
        [SerializeField] float smoothSpeed = 1.0f;
    
        [Header("Fly Setting")]
        [SerializeField] float flyDuration = 5f;
        [SerializeField] float flyCooldown = 2f;
        [SerializeField] float maxFlyHeight = 3f;
        [SerializeField] float flySpeed = 1000f;
        [SerializeField] float gravityMultiplier = 3f;
        [SerializeField] float flyDrag = 4f;
        
        
        private bool canShooterRotate = false;
        private float targetProjectileSpeed;
        private Vector3 projectileVelocity = new();
        private ProjectileShooter projectileShooter;
        
        const float ZeroF = 0f;
        const float AlmostZero = 0.01f;
    
        static readonly int SpeedHash = Animator.StringToHash("Speed");
    
        Transform mainCamera;
    
        List<Timer> timers;
        CountdownTimer flyTimer;
        CountdownTimer flyCooldownTimer;
    
        float currentSpeed;
        float flyVerticalVelocity;
        float velocity = 0f; // tells how quickly some value is changing
    
        
        StateMachine stateMachine;
        void Awake()
        {
            mainCamera = Camera.main.transform;
            rb.freezeRotation = true;
            
            SetupTimer();
            SetUpStateMachine();
        }
        
        void SetupTimer()
        {
            flyTimer = new CountdownTimer(flyDuration);
            flyCooldownTimer = new CountdownTimer(flyCooldown);
            timers = new List<Timer> (2) { flyTimer, flyCooldownTimer};
            
            flyTimer.OnTimerStopped += () => flyCooldownTimer.Start();
        }

        void SetUpStateMachine()
        {
            //setting up StateMachine
            stateMachine = new StateMachine();
            
            //Declare States
            var locomotionState = new LocomotionState(this, animator);
            var flyState = new FlyState(this, animator);
            
            //Adding Trasitions
            At(locomotionState, flyState, new FuncPredicate( () => flyTimer.IsRunning));
            At(flyState, locomotionState, new FuncPredicate( () => groundChecker.IsGrounded && !flyTimer.IsRunning));
            
            //Define initial State
            stateMachine.SetState(locomotionState);
        }

        private void At(IState from, IState to, IPredicate predicate) => stateMachine.AddTransition(from, to, predicate);
        private void Any(IState to, IPredicate predicate) => stateMachine.AddAnyTransition(to, predicate);
    
        void Start()
        {
            input.EnabelInputAction();
            ToggleCursorVisiblity();
            projectileShooter = GetComponentInChildren<ProjectileShooter>();
        }
        
        void OnEnable()
        {
            input.Fly += OnFly;
        }


        void OnDisable()
        {
            input.Fly -= OnFly;
        }

        void OnFly(bool performed)
        {
            if (performed && !flyTimer.IsRunning && !flyCooldownTimer.IsRunning && groundChecker.IsGrounded)
            {
                flyTimer.Start();
            }
            else if(!performed && flyTimer.IsRunning)
            {
                flyTimer.Stop();
                flyCooldownTimer.Start();
            }
        }
        void Update()
        {
            stateMachine.Update();
            
            HandleTimers(); 
            UpdateAnimator();
        }

        
        void FixedUpdate() 
        { 
            stateMachine.FixedUpdate();

            if (Input.GetMouseButtonDown(4))
            {
                Debug.Log("Mouse Button was pressed");
                ToggleCursorVisiblity();
            };
        }

        public void HandleFly()
        {
            if (flyTimer.IsRunning)
            {
                float currentMaxFlyHeight = maxFlyHeight + groundChecker.GroundDistance();
                // Check if the player has reached the maximum height
                if (rb.position.y < currentMaxFlyHeight)
                {
                    // Calculate the velocity required to reach the max height using physics equations v = sqrt(2gh)
                    flyVerticalVelocity = Mathf.Sqrt(2 * (currentMaxFlyHeight - rb.position.y) * Mathf.Abs(Physics.gravity.y));
                }
                else if (rb.position.y > currentMaxFlyHeight)
                {
                    flyVerticalVelocity += Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
                }
                else
                {
                    // Stop vertical velocity once the max height is reached
                    flyVerticalVelocity = 0f;
                }
            }
            else
            {
                flyVerticalVelocity += Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
            }
            
            rb.velocity = new Vector3(rb.velocity.x, flyVerticalVelocity, rb.velocity.z);
        }

        public void HandleMovement()
        {
            Vector3 horizontalInput = new Vector3(input.Direction.x, ZeroF, input.Direction.y);
            Vector3 adjustedDirection = Quaternion.AngleAxis(mainCamera.eulerAngles.y, Vector3.up) * horizontalInput;
            adjustedDirection.Normalize();
            
            if (horizontalInput.magnitude <= AlmostZero)
            {
                if (rb.velocity.magnitude > AlmostZero)
                {
                    float currentDrag = flyTimer.IsRunning ? flyDrag : drag;
                    Vector3 velocity = -rb.velocity;
                    Vector3 dampingForce = velocity.normalized * currentDrag;
                    rb.AddForce(dampingForce, ForceMode.Force);
                }
                return;
            }
            
            // Determine target speed based on flying or walking
            float targetSpeed = flyTimer.IsRunning ? flySpeed : moveSpeed;
            SmoothSpeed(targetSpeed);
            HandleHorizontalMovement(adjustedDirection);
            HandleRotation(adjustedDirection);
        }

        void HandleHorizontalMovement(Vector3 adjustedDirection)
        {
            Vector3 velocity = adjustedDirection * currentSpeed ; 
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
            print($"Horizontal Movement: {velocity} && RB Velocity: {rb.velocity}");
        }

        public void HandleRotation(Vector3 adjustedDirection)
        {
            Vector3 cameraForward = new Vector3(mainCamera.forward.x, 0f, mainCamera.forward.z).normalized;

            // Use the camera's forward direction as the target rotation
            Quaternion vector2Look = Quaternion.LookRotation(adjustedDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, vector2Look, rotationSpeed * Time.deltaTime);
        }

        void SmoothSpeed(float targetSpeed)
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref velocity, smoothSpeed);
        }

        void HandleTimers()
        {
            foreach (var timer in timers)
            {
                timer.Tick(Time.deltaTime);
            }
        }

        void UpdateAnimator()
        {
            animator.SetFloat(SpeedHash, currentSpeed);
        }
        
        void ToggleCursorVisiblity()
        {
            if (Cursor.visible)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
                
        }
    }
}
