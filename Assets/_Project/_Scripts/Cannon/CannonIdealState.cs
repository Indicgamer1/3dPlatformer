using UnityEngine;

namespace Platformer
{
    public class CannonIdealState : CannonBaseState
    {
        Vector3 initialForward;

        public CannonIdealState(Cannon cannon) : base(cannon)
        {
            initialForward = cannon.transform.forward;
        }
    }
}