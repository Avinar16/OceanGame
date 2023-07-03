using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementUtilityFuncs
{
    public void SpeedLimit(Rigidbody Rb, float Speed)
    {
        Vector3 flatVelocity = new Vector3(Rb.velocity.x, 0f, Rb.velocity.z);
        if (flatVelocity.magnitude > Speed)
        {
            Vector3 LimitedVelocity = flatVelocity.normalized * Speed;
            Rb.velocity = new Vector3(LimitedVelocity.x, Rb.velocity.y, LimitedVelocity.z);
        }
    }
    public bool OnSlope(Transform transform, RaycastHit SlopeHit, float distance)
    {
        if(Physics.Raycast(transform.position, Vector3.down, out SlopeHit, distance))
        {
            if (SlopeHit.normal != Vector3.up) { return true; }
        }
        return false;
    }
}
