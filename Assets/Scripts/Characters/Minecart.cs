using UnityEngine;

/**
 * GummyCinemaRPG - Minecart.cs
 * Name: Ka Bo Cheung
 * Date: 05/11/2026
 * Course: GAME-2341-001
 *
 * Script for the minecart in the turn-based RPG battle
 */
public class Minecart : MonoBehaviour
{
    // Defined global variables
    private float speed = 30.0f;
    private float xOutOfBound = 15.0f;

    // Constantly moving to the right and checking for the out-of-bound to be destroyed
    void Update()
    {
        transform.Translate(Vector2.right * (Time.deltaTime * speed));
        
        if (transform.position.x > xOutOfBound)
        {
            Destroy(gameObject);
        }
    }
}