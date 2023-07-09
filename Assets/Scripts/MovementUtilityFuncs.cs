using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementUtilityFuncs
{
    public RaycastHit SlopeHit;
    public void SpeedLimit(Rigidbody Rb, float Speed)
    {
        Vector3 flatVelocity = new Vector3(Rb.velocity.x, 0f, Rb.velocity.z);
        if (flatVelocity.magnitude > Speed)
        {
            Vector3 LimitedVelocity = flatVelocity.normalized * Speed;
            Rb.velocity = new Vector3(LimitedVelocity.x, Rb.velocity.y, LimitedVelocity.z);
        }
    }
    public bool OnSlope(Rigidbody _rb, float MaxSlopeAngle, bool isGrounded, CapsuleCollider _Collider)
    {
        //if (Physics.Raycast(transform.position, Vector3.down, out SlopeHit, SlopeRayDistance) && isGrounded)
        float sphereCastRadius = _Collider.radius * 1.35f;
        float sphereCastDistance = _Collider.bounds.extents.y - sphereCastRadius + 0.05f;
        if(Physics.SphereCast(_rb.position, sphereCastRadius, Vector3.down, out SlopeHit, sphereCastDistance))
        {
            float angle = Vector3.Angle(Vector3.up, SlopeHit.normal);
            if (angle < MaxSlopeAngle && angle != 0) { return true; }
        }
        return false;
    }
    public bool CheckIfGrounded(Transform GroundCheckPos, float GroundCheckRadius, LayerMask WhatIsGround)
    {
        return Physics.CheckSphere(GroundCheckPos.position, GroundCheckRadius, WhatIsGround);
    }

}
