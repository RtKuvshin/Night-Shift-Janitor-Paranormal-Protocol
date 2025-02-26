using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeadFollowMouse : MonoBehaviour
{
    [SerializeField] private Transform headTarget;
    [SerializeField] private float sensitivity = 2f;
    [SerializeField] private float maxXRotation = 30f;
    [SerializeField] private float maxYRotation = 45f;

    private float currentXRotation = 0f;
    private float currentYRotation = 0f;
    private PlayerInput playerInput;
    private InputAction lookAction;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        headTarget.localRotation = Quaternion.Euler(0f, 0f, 0f);
        playerInput = GetComponent<PlayerInput>();
        lookAction = playerInput.actions["Look"];
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /*private void Update()
    {
        Vector2 mouseDelta = lookAction.ReadValue<Vector2>();

        currentYRotation += mouseDelta.x * sensitivity;
        currentXRotation -= mouseDelta.y * sensitivity;
        currentXRotation = Mathf.Clamp(currentXRotation, -maxXRotation, maxXRotation);
        currentYRotation = Mathf.Clamp(currentYRotation, -maxYRotation, maxYRotation);

        headTarget.localRotation = Quaternion.Euler(currentXRotation, currentYRotation, 0f);
        
        if (Mathf.Abs(currentYRotation) >= maxYRotation)
        {
            float turnAmount = mouseDelta.x * sensitivity;
            transform.Rotate(0f, turnAmount, 0f);
        }
    }*/
    private void Update()
    {
        Vector2 mouseDelta = lookAction.ReadValue<Vector2>();
        currentXRotation -= mouseDelta.y * sensitivity;
        currentXRotation = Mathf.Clamp(currentXRotation, -maxXRotation, maxXRotation);
        headTarget.localRotation = Quaternion.Euler(currentXRotation, 0, 0f);
        currentYRotation += mouseDelta.x * sensitivity;
        
    }

    private void FixedUpdate()
    {
        Quaternion targetRotation = Quaternion.Euler(0, currentYRotation, 0);
        rb.MoveRotation(targetRotation);
    }
}