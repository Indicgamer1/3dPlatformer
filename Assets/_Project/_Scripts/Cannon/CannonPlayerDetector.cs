using System;
using UnityEngine;

namespace Platformer
{
    public class CannonPlayerDetector : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] Transform barrel;
        [SerializeField] Transform turret;

        [Header("Cannon Settings")]
        [SerializeField] float range = 15f;
        [SerializeField] float turretRotationSpeed = 100f;
        [SerializeField] float barrelRotationSpeed = 100f;
        [SerializeField] float barrelTopDownRange = 60f;
        [SerializeField] float offset = 45f;

        private Vector3 rotatedForward;
        //private Vector3 vectorToPlayer;
        private Vector3 initialForward;

        private const float ZeroF = 0f;

        private void Start()
        {
            initialForward = turret.forward;
        }

        private void Update()
        {
            Debug.DrawLine(barrel.position, barrel.position + initialForward * 20, Color.green);
        }

        /*bool CanHorizontalRotate()
        {
            Vector3 vectorToPlayer = target.position - barrel.position;
            if (vectorToPlayer.magnitude > range) return false;
            
            rotatedForward = Quaternion.AngleAxis(offset, turret.right) * turret.forward; //vector which will determine limit
            float angleToPlayer = Vector3.Angle(vectorToPlayer, rotatedForward);
            
            //Debug
            /*Debug.DrawLine(barrel.position, barrel.position + turret.up * range, Color.green);
            Debug.DrawLine(barrel.position, barrel.position + turret.forward * range, Color.red);
            Debug.DrawLine(barrel.position, barrel.position + turret.right * range, Color.blue);
            Debug.DrawLine(barrel.position, barrel.position + rotatedForward * range, Color.yellow);
            Debug.DrawLine(barrel.position, barrel.position + vectorToPlayer * range, Color.yellow);#1#
            
            if (angleToPlayer <= barrelTopDownRange / 2f)
                return true;
            else 
                return false;
        }*/
        
        void OnDrawGizmos()
        {
            if(target != null)
                Gizmos.DrawLine(barrel.position, target.position);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}