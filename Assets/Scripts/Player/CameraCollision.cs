using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform; // Камера як дочірній об'єкт
    [SerializeField] private float maxDistance = 1f;        // Бажана відстань камери від голови
    [SerializeField] private float minDistance = 0.1f;      // Мінімальна дозволена відстань
    [SerializeField] private float smoothSpeed = 10f;       // Швидкість інтерполяції
    [SerializeField] private LayerMask collisionLayers;     // Шари, з якими перевіряти колізії
    [SerializeField] private float cameraRadius = 0.2f;       // Радіус сфери для SphereCast

    private Vector3 defaultLocalPosition;

    private void Start()
    {
        // Запам'ятовуємо початкове положення камери відносно "pivot" (голови)
        defaultLocalPosition = cameraTransform.localPosition;
    }

    private void LateUpdate()
    {
        // Обчислюємо бажану позицію камери в світових координатах 
        Vector3 desiredWorldPos = transform.TransformPoint(defaultLocalPosition);
        Vector3 direction = desiredWorldPos - transform.position;
        float desiredDistance = direction.magnitude;
        direction.Normalize();

        RaycastHit hit;
        // Виконуємо SphereCast від позиції pivot у напрямку бажаної позиції
        if (Physics.SphereCast(transform.position, cameraRadius, direction, out hit, desiredDistance, collisionLayers))
        {
            // Якщо зіткнулися – обчислюємо нову відстань, щоб камера була перед стіною
            float newDistance = Mathf.Clamp(hit.distance - 0.1f, minDistance, maxDistance);
            // Оновлюємо локальну позицію камери вздовж осі Z (якщо камера розміщена позаду pivot)
            Vector3 newLocalPos = defaultLocalPosition;
            newLocalPos.z = -newDistance;
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newLocalPos, Time.deltaTime * smoothSpeed);
        }
        else
        {
            // Якщо перешкоди немає – повертаємо камеру у її стандартну позицію
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, defaultLocalPosition, Time.deltaTime * smoothSpeed);
        }
    }
}
