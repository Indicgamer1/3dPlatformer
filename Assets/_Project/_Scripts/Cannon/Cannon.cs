using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;

namespace Platformer
{
    public class Cannon : Shooter
    {
        protected override void Shoot()
        {
            var bulletTrans = Instantiate(bulletPrefab, barrelMouth.position, barrelMouth.rotation);
            /*StartCoroutine(GoTowardsPlayer(bulletTrans));*/
            
            var bullet = bulletTrans.gameObject.GetComponent<IBullet>();
            Vector3 velocity = bulletTrans.forward * bulletSpeed;
            bullet.Launch(velocity, player);
            if(shootParticle != null)
                shootParticle.Play();
            
            attackCoolDownTimer.Start();
            print("shooting");
        }

        
    }
}
