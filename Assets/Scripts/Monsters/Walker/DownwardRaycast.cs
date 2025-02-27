using UnityEngine;

public class ArmRaycast : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform armTransform;
    [SerializeField] private float rayLength = 1f;

    private void Update()
    {
        Vector3 origin = armTransform.position;
        Vector3 direction = Vector3.back;

        Debug.DrawRay(origin, direction * rayLength, Color.red);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, rayLength, playerLayer))
        {
            Debug.Log("Collision with player!");
        }
    }
}