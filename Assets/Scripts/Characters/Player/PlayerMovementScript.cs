using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MoveableCharacter
{

    #region MovementVariables
    private Rigidbody PlayerRigidbody;
    private CapsuleCollider PlayerCollider;
    private float Speed;

    [Header("Ground movement settings")]
    public float WalkSpeed;
    public float RunSpeed;
    private float AirSpeed;
    [SerializeField] private float GroundDrag;

    [Space]
    [Header("Jump Settings")]
    public float JumpForce;
    private bool IsGrounded = false;
    [SerializeField] Transform GroundCheckPos;
    [SerializeField] private float GroundCheckRadius;
    [SerializeField] private LayerMask WhatIsGround;
    [SerializeField] private float AirDrag;

    [Space]
    [Header("Slope movement settings")]
    [SerializeField] private float SlopeRayDistance;
    [SerializeField] private float MaxSlopeAngle;
    
    
    #endregion
    private PlayerInputActions Input;

    // player states
    [HideInInspector]
    public MoveState State;
    public enum MoveState
    {
        Walking,
        Running,
        InAir,
        Swimming
    }

    private void StateHandler()
    {
        if(IsGrounded && Input.Player.Run.IsPressed())
        {
            State = MoveState.Running;

            PlayerRigidbody.drag = GroundDrag;
            Speed = RunSpeed;
            // Air speed = / 2 current speed
            AirSpeed = RunSpeed / 2;
        }
        else if (IsGrounded)
        {
            State = MoveState.Walking;

            PlayerRigidbody.drag = GroundDrag;
            Speed = WalkSpeed;
            // Air speed = / 2 current speed
            AirSpeed = WalkSpeed / 2;
        }
        else
        {
            State = MoveState.InAir;

            PlayerRigidbody.drag = AirDrag;
            Speed = AirSpeed;  
        }
    }

    private void Awake()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();
        PlayerCollider = GetComponentInChildren<CapsuleCollider>();

        // Setup input
        InputHandler.InputManager.SetPlayerControls();
        Input = InputHandler.InputManager.Input;
        Input.Player.Jump.performed += ctx => Jump();
    }
    private void Jump()
    {
        if (IsGrounded)
        {
            PlayerRigidbody.velocity = new Vector3(PlayerRigidbody.velocity.x, 0f, PlayerRigidbody.velocity.z);
            PlayerRigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }
    }
    private void Move(float MoveSpeed)
    {
        Vector2 direction = Input.Player.Move.ReadValue<Vector2>();
        // default move vector
        Vector3 moveVector = (transform.forward * direction.y) + (transform.right * direction.x);

        if (IsOnSlope(PlayerRigidbody, MaxSlopeAngle, IsGrounded, PlayerCollider))
        { 
            // slope move vector
            moveVector = Vector3.ProjectOnPlane(moveVector, SlopeHit.normal);
            MoveSpeed *= 2;
           
        }
        PlayerRigidbody.AddForce(moveVector.normalized * MoveSpeed * 10, ForceMode.Force);
    }


    private void FixedUpdate()
    {
        IsGrounded = CheckIfGrounded(GroundCheckPos, GroundCheckRadius, WhatIsGround);
        StateHandler();
        Move(Speed);
        SpeedLimit(PlayerRigidbody, Speed);
        
    }


}
