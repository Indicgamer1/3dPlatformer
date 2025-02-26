using UnityEngine;

namespace Platformer
{
    public class ArcDetectionStrategy : IDetectionStrategy
    {
        readonly float detectionAngle;
        readonly float detectionRadius;
        
        public ArcDetectionStrategy(float detectionAngle, float detectionRadius)
        {
            this.detectionAngle = detectionAngle;
            this.detectionRadius = detectionRadius;
        }


        public bool Execute(Transform player, Transform detector, CountdownTimer timer)
        {
            Vector3 playerPos = new Vector3(0f, player.position.y, player.position.z);
            Vector3 detectorPos = new Vector3(0f, detector.position.y, detector.position.z);
            
            Vector3 vectorToPlayer = playerPos - detectorPos;
            float angleToPlayer = Vector3.Angle(detector.forward, vectorToPlayer);
            
            if(angleToPlayer > detectionAngle/2f || vectorToPlayer.magnitude > detectionRadius)
                return false;
            
            timer.Start();
            return true;
        }
    }
}