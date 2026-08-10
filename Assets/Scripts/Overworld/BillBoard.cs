using UnityEngine;
/*
 * Final Project: BillBoard.cs
 * Name: Ka Bo Cheung
 * Date: 08/10/2026
 * Course: GAME-1377-001
 *
 * Script for the billboarding in which the 2D sprites always look at the camera.
 */
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