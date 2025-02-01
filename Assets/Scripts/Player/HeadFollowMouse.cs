using UnityEngine;
using UnityEngine.InputSystem;

public class HeadFollowMouse : MonoBehaviour
{
    [SerializeField] private Transform headBone;
    [SerializeField] private float sensitivity = 2f;
    [SerializeField] private float maxXRotation = 30f;
    [SerializeField] private float maxYRotation = 45f;
    [SerializeField] private float smoothSpeed = 5f;

    private float currentXRotation = 0f;
    private float currentYRotation = 0f;
    private Quaternion lastHeadRotation; 
    private Animator animator;

    private PlayerInput playerInput;
    private InputAction lookAction;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        playerInput = GetComponent<PlayerInput>();
        lookAction = playerInput.actions["Look"]; // Assumes you have a "Look" action in your Input Actions
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        lastHeadRotation = headBone.localRotation;
    }

    private void Update()
    {
        RotateHeadWithMouse();
    }

    private void RotateHeadWithMouse()
    {
        if (headBone == null) return;

        Vector2 mouseDelta = lookAction.ReadValue<Vector2>(); // Get mouse delta using input actions

        if (mouseDelta.magnitude > 0.01f) // Check if mouse has moved
        {
            currentYRotation += mouseDelta.x * sensitivity; // Apply delta to Y (horizontal)
            currentXRotation -= mouseDelta.y * sensitivity; // Apply delta to X (vertical)

            currentYRotation = Mathf.Clamp(currentYRotation, -maxYRotation, maxYRotation); // Clamp Y rotation
            currentXRotation = Mathf.Clamp(currentXRotation, -maxXRotation, maxXRotation); // Clamp X rotation

            Quaternion targetRotation = Quaternion.Euler(currentXRotation, currentYRotation, 0f);
            headBone.localRotation = Quaternion.Slerp(headBone.localRotation, targetRotation, Time.deltaTime * smoothSpeed);

            lastHeadRotation = headBone.localRotation; // Update last head rotation when the mouse moves
        }
        else // When the mouse stops moving
        {
            headBone.localRotation = Quaternion.Slerp(headBone.localRotation, lastHeadRotation, Time.deltaTime * smoothSpeed); // Smooth transition to last rotation
        }
    }
}
