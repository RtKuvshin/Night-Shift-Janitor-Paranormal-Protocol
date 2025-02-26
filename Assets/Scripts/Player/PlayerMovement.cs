using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform headTarget;

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

        if (headTarget != null)
        {
            
            Vector3 headForward = transform.forward;
            Vector3 headRight = transform.right;

            
            headForward.y = 0;
            headRight.y = 0;

            headForward.Normalize();
            headRight.Normalize();

            
            Vector3 moveVelocity = (headForward * moveDirection.y + headRight * moveDirection.x) * moveSpeed;
            moveVelocity.y = rb.velocity.y; 

            rb.velocity = moveVelocity;
        }

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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Walls"))
        {
            
        }
    }
}
