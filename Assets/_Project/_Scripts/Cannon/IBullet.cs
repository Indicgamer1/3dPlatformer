using UnityEngine;

namespace Platformer
{
    public interface IBullet
    {
        public void Launch(Vector3 velocity, Transform target = null);
    }
}