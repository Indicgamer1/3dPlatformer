using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Platformer
{
    public abstract class BaseBullet : MonoBehaviour
    {
        [SerializeField] protected GameObject BlastEffect;
        [SerializeField] protected int damage = 5;
        [SerializeField] protected float damageRadius = 1f;

        protected Rigidbody rb;
        protected AudioSource audioSource;
        protected Vector3 startPos;

        protected virtual void Start()
        {
            startPos = transform.position;
            rb = GetComponent<Rigidbody>();
            audioSource = GetComponent<AudioSource>();
        }

        protected virtual void Update()
        {
            print($" {this.name} velocity: {rb.velocity}");
            float movedDistance = (transform.position - startPos).magnitude;
            if (movedDistance > 100f)
            {
                DestroyObj();
            }
        }

        protected virtual void OnCollisionEnter(Collision other)
        {
            audioSource.Play();
            Collider[] colliders = Physics.OverlapSphere(other.contacts[0].point, damageRadius);
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<Health>(out Health health))
                    health.TakeDamage(damage);
            }
            DestroyObj();
        }

        protected void DestroyObj()
        {
            Destroy(gameObject);
            if (BlastEffect != null)
                Instantiate(BlastEffect, transform.position, transform.rotation);
        }
        
        public abstract void Shoot(Vector3 direction, Transform target = null);
    }
}