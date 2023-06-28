using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInputActions Input;
    private CharacterController Controller;
    public float JumpForce;
    public float MoveSpeed;
    private bool isGrounded;
    [SerializeField] Transform GroundCheckPos;
    [SerializeField] private float GroundCheckRadius;
    [SerializeField] private LayerMask WhatIsGround;

    Vector3 gravity;
    private float g = -20f;
    private void Awake()
    {
        Controller = GetComponent<CharacterController>();
        Input = new PlayerInputActions();

        Input.Player.Enable();

        Input.Player.Jump.performed += context => OnJump();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Gravity();
        Move();
    }
    private void Move()
    {

        Vector2 direction = Input.Player.Move.ReadValue<Vector2>();
        Vector3 moveVector = (transform.forward * direction.y) + (transform.right * direction.x);
        //Debug.Log(moveVector * Time.deltaTime * MoveSpeed);
        Controller.Move(moveVector * Time.deltaTime * MoveSpeed);

    }
    private void OnJump()
    {
        if (isGrounded)
        {
            gravity.y += Mathf.Sqrt(JumpForce * -2 * g);
        }
    }
    private void Gravity()
    {

        isGrounded = Physics.CheckSphere(GroundCheckPos.position, GroundCheckRadius, WhatIsGround);
        Debug.Log(isGrounded);
        if (!isGrounded)
        {
            gravity.y += g * Time.deltaTime;
        }
        else if (isGrounded && gravity.y <= 0)
        {
            gravity.y = -1f;
        }
        Controller.Move(gravity * Time.deltaTime);

    }
    /*
    private void OnEnable()
    {
        Input.Enable();
    }
    private void OnDisable()
    {
        Input.Disable();
    }
    */
}