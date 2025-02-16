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

    private void Awake()
    {
        headTarget.localRotation = Quaternion.Euler(0f, 0f, 0f);
        playerInput = GetComponent<PlayerInput>();
        lookAction = playerInput.actions["Look"];
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    

    private void Update()
    {
        Vector2 mouseDelta = lookAction.ReadValue<Vector2>();

        currentYRotation += mouseDelta.x * sensitivity;
        currentXRotation -= mouseDelta.y * sensitivity;
        currentYRotation = Mathf.Clamp(currentYRotation, -maxYRotation, maxYRotation);
        headTarget.localRotation = Quaternion.Euler(currentXRotation, currentYRotation, 0f);

        if (currentYRotation >= maxYRotation || currentYRotation <= -maxYRotation)
        {
            float turnAmount = mouseDelta.x * sensitivity;
            transform.Rotate(0f, turnAmount, 0f);
        }
    }
}
