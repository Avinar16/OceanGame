using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{

    #region MovementVariables
    private Rigidbody PlayerRigidbody;
    private float Speed;

    [Header("Ground movement settings")]
    public float WalkSpeed;
    public float RunSpeed;
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
    public RaycastHit SlopeHit;
    [SerializeField]private float SlopeRayDistance;
    #endregion

    private PlayerInputActions Input;
    private MovementUtilityFuncs MoveFuncs;

    // player states
    [HideInInspector]
    public MoveState State;
    public enum MoveState
    {
        Walking,
        Running,
        InAir
    }

    private void StateHandler()
    {
        if(IsGrounded && Input.Player.Run.IsPressed())
        {
            PlayerRigidbody.drag = GroundDrag;
            State = MoveState.Running;
            Speed = RunSpeed;
        }
        else if (IsGrounded)
        {
            PlayerRigidbody.drag = GroundDrag;
            State = MoveState.Walking;
            Speed = WalkSpeed;
        }
        else
        {
            PlayerRigidbody.drag = AirDrag;

            if(State == MoveState.Running) { Speed = RunSpeed / 2; }
            else { Speed = WalkSpeed / 2; }

            State = MoveState.InAir;
            
        }
    }
    

    private void SetPlayerInput()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Input.Disable();
        Input.Player.Enable();
        // Connect Jump
        Input.Player.Jump.performed += ctx => Jump();
    }

    private void Awake()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();

        // Setup input
        Input = new PlayerInputActions();
        SetPlayerInput();

        // setup funcs
        MoveFuncs = new MovementUtilityFuncs();
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
        if (OnSlope())
        {
            // slope move vector
            moveVector = Vector3.ProjectOnPlane(moveVector, SlopeHit.normal);
            Debug.Log(SlopeHit.normal);
        }
        PlayerRigidbody.AddForce(moveVector.normalized * MoveSpeed * 10, ForceMode.Force);
    }

    private void CheckIfGrounded()
    {
        IsGrounded = Physics.CheckSphere(GroundCheckPos.position, GroundCheckRadius, WhatIsGround);
    }

    private void FixedUpdate()
    {
        CheckIfGrounded();
        StateHandler();
        Move(Speed);
        MoveFuncs.SpeedLimit(PlayerRigidbody, Speed);
        
    }
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out SlopeHit, SlopeRayDistance))
        {
            if (SlopeHit.normal != Vector3.up) { return true; }
        }
        return false;
    }



    /* In development
    private void SetUIiInput()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Input.Disable();
        Input.UI.Enable();
    }
    */
}
