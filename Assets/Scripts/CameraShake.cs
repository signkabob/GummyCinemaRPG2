using UnityEngine;
using System.Collections;

/**
 * GummyCinemaRPG - CameraShake.cs
 * Name: Ka Bo Cheung
 * Date: 05/11/2026
 * Course: GAME-2341-001
 *
 * Script for the button to shake the camera for a specific duration;
 * Supported by Google Search AI overview
 */
public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float duration, float magnitude)
    {
        // Store the original local position to return to it later
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // Generate a random offset for X and Y
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            // Apply the offset while maintaining the original Z
            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;

            // Wait until the next frame
            yield return null;
        }

        // Reset the camera position back to the start
        transform.localPosition = originalPos;
    }
}

