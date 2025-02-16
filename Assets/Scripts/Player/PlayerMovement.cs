using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 moveDirection = Vector2.zero;
    private Rigidbody rb;
    private Animator _animator;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private bool isMoving = false;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            moveAction = playerInput.actions["Move"];
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        moveDirection = moveAction.ReadValue<Vector2>();
        bool currentlyMoving = moveDirection.magnitude > 0;
        Vector3 moveVelocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.y * moveSpeed);
        rb.velocity = moveVelocity;

        if (currentlyMoving && !isMoving)
        {
            _animator.SetTrigger("Walk");
            isMoving = true;
        }
        else if (!currentlyMoving && isMoving)
        {
            _animator.SetTrigger("Idle");
            isMoving = false;
        }
        
    }

}
