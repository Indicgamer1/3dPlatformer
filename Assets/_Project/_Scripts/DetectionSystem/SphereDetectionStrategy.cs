using UnityEngine;

namespace Platformer
{
    public class SphereDetectionStrategy : IDetectionStrategy
    {
        readonly float detectionRadius;
        
        public SphereDetectionStrategy(float detectionRadius) => this.detectionRadius = detectionRadius;

        public bool Execute(Transform player, Transform detector, CountdownTimer timer)
        {
            if(timer.IsRunning) return false;
            
            Vector3 vectorToPlayer = player.position - detector.position;
            
            if(vectorToPlayer.magnitude > detectionRadius) 
                return false;
            
            timer.Start();
            return true;
        }
    }
}