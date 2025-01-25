using System;
using UnityEngine;

public class HeadFollowMouse : MonoBehaviour
{
    [SerializeField] private Transform headBone; 
    [SerializeField] private float sensitivity = 2f;
    [SerializeField] private float maxXRotation = 30f;
    [SerializeField] private float maxYRotation = 45f;
    [SerializeField] private float smoothSpeed = 5f;

    private float currentXRotation = 0f;
    private float currentYRotation = 0f;
    private bool isMouseMoving = false;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        if (Mathf.Abs(mouseX) > 0.01f || Mathf.Abs(mouseY) > 0.01f)
        {
            isMouseMoving = true;
            animator.enabled = false; 

            currentYRotation += mouseX * sensitivity;
            currentXRotation -= mouseY * sensitivity;

            currentYRotation = Mathf.Clamp(currentYRotation, -maxYRotation, maxYRotation);
            currentXRotation = Mathf.Clamp(currentXRotation, -maxXRotation, maxXRotation);

            if (headBone != null)
            {
                Quaternion targetRotation = Quaternion.Euler(currentXRotation, currentYRotation, 0f);
                headBone.localRotation = Quaternion.Slerp(headBone.localRotation, targetRotation, Time.deltaTime * smoothSpeed);
            }
        }
        else if (isMouseMoving)
        {
            isMouseMoving = false;
            animator.enabled = true; 
        }
    }
}
