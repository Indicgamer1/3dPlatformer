using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Platformer
{
    public class Bullet : MonoBehaviour, IBullet
    {
        [SerializeField] GameObject BlastEffect;
        [SerializeField] int damage = 5;
        [SerializeField] float damageRadius = 1f;

        Rigidbody rb;
        Vector3 startPos;
        private void Awake()
        {
            startPos = transform.position;
            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            float movedDistance = (transform.position - startPos).magnitude;
            if (movedDistance > 100f)
            {
                DestroyObj();
            }
        }
        
        private void OnCollisionEnter(Collision other)
        {
            Collider[] colliders = Physics.OverlapSphere(other.contacts[0].point, damageRadius);
            foreach (Collider collider in colliders)
            {
                if(collider.TryGetComponent<Health>(out Health health))
                    health.TakeDamage(damage);
            }
            DestroyObj();
        }
        
        public void DestroyObj()
        {
            if (BlastEffect != null)
                Instantiate(BlastEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        
        public void Launch(Vector3 velocity, Transform target = null)
        {
            rb.velocity = velocity;
        }
    }
}
