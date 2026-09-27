using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [Header("Referencias")]
    public Transform cameraTransform;

    [Header("Configuración Parallax")]
    public float parallaxMultiplier;
    public Vector2 autoScrollSpeed;

    [Header("Repetición Infinita")]
    [Tooltip("El tamaño exacto de tu bloque de fondo (en unidades de Unity).")]
    public Vector2 repeatSize;

    private Vector3 lastCameraPosition;

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        Vector3 newPosition = transform.position + (deltaMovement * parallaxMultiplier);
        newPosition += (Vector3)(autoScrollSpeed * Time.deltaTime);
        transform.position = newPosition;
        lastCameraPosition = cameraTransform.position;

        float distanceX = cameraTransform.position.x - transform.position.x;
        float distanceY = cameraTransform.position.y - transform.position.y;

        if (Mathf.Abs(distanceX) >= repeatSize.x)
        {
            float offsetX = Mathf.Sign(distanceX) * repeatSize.x;
            transform.position = new Vector3(transform.position.x + offsetX, transform.position.y, transform.position.z);
        }

        if (Mathf.Abs(distanceY) >= repeatSize.y)
        {
            float offsetY = Mathf.Sign(distanceY) * repeatSize.y;
            transform.position = new Vector3(transform.position.x, transform.position.y + offsetY, transform.position.z);
        }
    }
}