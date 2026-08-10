using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] Transform mainCameraTransform;

    void Start()
    {
        // Cache the main camera's transform for performance
        if (Camera.main != null)
        {
            mainCameraTransform = Camera.main.transform;
        }
    }

    void LateUpdate()
    {
        Vector3 direction = Camera.main.transform.position - transform.position;
        direction.y = 0; // Lock vertical tilt
        transform.rotation = Quaternion.LookRotation(-direction);


    }
}