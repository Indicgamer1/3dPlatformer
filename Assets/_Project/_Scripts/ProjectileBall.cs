using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace Platformer
{
    public class ProjectileBall : MonoBehaviour
    {
        [SerializeField] GameObject destroyParticles;
        [SerializeField] AudioSource audioSource;
        [SerializeField] int damage;
        [SerializeField] float damageRadius = 1f;
        [SerializeField] float torque;

        [HideInInspector] public Transform releasePoint;
        
        public UnityAction OnHit = delegate { };
        
        bool didHit;
        
        public float Torque { get; private set; }
        public Rigidbody RB { get; private set; }
        public bool IsFlying => RB.velocity.magnitude > 0.1f;

        private void Awake()
        {
            Torque = torque;
            RB = GetComponent<Rigidbody>();
            if(RB == null)
                RB = gameObject.AddComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision other)
        {
            if(other.gameObject.CompareTag("Player")) 
                return;
            
            OnHit.Invoke();
            TriggerSFX();
            GiveDamage();
            Reset();
            print("Collided");
        }

        private void TriggerSFX()
        {
            audioSource.Play();
            Instantiate<GameObject>(destroyParticles, transform.position, quaternion.identity);
        }

        private void GiveDamage()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
            foreach (var collider in colliders)
            {
                Health entityHealth = collider.GetComponent<Health>();
                if(entityHealth != null)
                    entityHealth.TakeDamage(damage);
            }
        }


        public void Reset()
        {
            transform.position = releasePoint.position;
            RB.isKinematic = true;
            RB.velocity = Vector3.zero;
            RB.angularVelocity = Vector3.zero;
        }

        public void ToggleCollider(bool value)
        {
            GetComponent<Renderer>().enabled = value;
            GetComponent<Collider>().enabled = value;
        }
    }
}