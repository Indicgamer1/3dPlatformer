using UnityEngine;

namespace Platformer
{
    public class LookAtCamera : MonoBehaviour
    {
        private Transform player;
        private void Start() => player = GameObject.FindGameObjectWithTag("Player").transform;
        
        private void Update() => LookAtPlayer();

        private void LookAtPlayer()
        {
            Vector3 playerDir = (player.position - transform.position).normalized;
            //to rotate the Targetdirection in XZ plane, Vec.Up is norml of Plane
            Vector3 targetDirection = Vector3.ProjectOnPlane(playerDir, Vector3.up).normalized;

            
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDirection), 360f * Time.deltaTime);
        }
    }
}