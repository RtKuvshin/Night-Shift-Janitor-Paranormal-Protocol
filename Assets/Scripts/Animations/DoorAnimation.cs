using System;
using UnityEngine;
using UnityEngine.UI;

public class DoorAnimation : MonoBehaviour
{
    private Animator _animator;
    private bool isDoorOpen = false;
    private float holdTime = 0.5f;
    private float holdTimer = 0f;
    private bool isHolding = false;

    [SerializeField] private Slider doorProgressSlider;
    [SerializeField] private GameObject doorProgressPanel;
    [SerializeField] private float interactionDistance = 5f; // Max distance for raycast interaction
    [SerializeField] private LayerMask doorLayer; // Layer mask for doors
    [SerializeField] private Transform playerHead; // Reference to the player's head (or top of the player)
    private GameObject currentDoor = null; // The door the player is interacting with

    private void Awake()
    {
        if (doorProgressSlider != null)
            doorProgressPanel.gameObject.SetActive(false); // Hide slider initially
    }

    private void Update()
    {
        // Cast ray to detect the door
        RaycastHit hit;
        Ray ray = new Ray(playerHead.position, playerHead.forward); // Ray from player's head

        // Visualize the ray in the Scene view
        Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.red);

        if (Physics.Raycast(ray, out hit, interactionDistance, doorLayer))
        {
            // Check if the raycast hits a door
            if (hit.collider.CompareTag("Door")) // Assuming doors have a tag "Door"
            {
                GameObject detectedDoor = hit.collider.gameObject;

                // If this is a new door, reset previous door interaction
                if (detectedDoor != currentDoor)
                {
                    currentDoor = detectedDoor;
                    _animator = currentDoor.GetComponentInChildren<Animator>(); // Get the animator for the new door
                    isDoorOpen = false; // Reset door open state when switching doors
                    ResetSlider(); // Reset the slider for the new door interaction
                }
            }
        }
        else
        {
            currentDoor = null; // No door detected
            ResetSlider(); // Reset slider if no door is detected
        }

        // Only allow interaction if a door is detected
        if (currentDoor != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isHolding = true;
                holdTimer = 0f;
                if (doorProgressSlider != null)
                    doorProgressPanel.gameObject.SetActive(true); // Show slider
            }

            if (Input.GetKey(KeyCode.E) && isHolding)
            {
                holdTimer += Time.deltaTime;
                if (doorProgressSlider != null)
                    doorProgressSlider.value = holdTimer / holdTime;

                if (holdTimer >= holdTime)
                {
                    ToggleDoor();
                    isHolding = false;
                    ResetSlider();
                }
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                isHolding = false;
                ResetSlider();
            }
        }
    }

    private void ToggleDoor()
    {
        if (_animator != null)
        {
            // Toggle the door based on its current state
            if (isDoorOpen)
            {
                _animator.SetTrigger("Closed");
            }
            else
            {
                _animator.SetTrigger("Opened");
            }
            isDoorOpen = !isDoorOpen; // Toggle the door state
        }
    }

    private void ResetSlider()
    {
        if (doorProgressSlider != null)
        {
            doorProgressSlider.value = 0;
            doorProgressPanel.gameObject.SetActive(false); // Hide slider after interaction
        }
    }
}
