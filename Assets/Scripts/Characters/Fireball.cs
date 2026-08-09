using UnityEngine;

/**
 * GummyCinemaRPG - Fireball.cs
 * Name: Ka Bo Cheung
 * Date: 05/11/2026
 * Course: GAME-2341-001
 *
 * Script for the fireball in the turn-based RPG battle
 */
public class Fireball : MonoBehaviour
{
    // Defined global variables
    public int damage = 3;
    private float speed = 5.0f;
    private float xOutOfBound = -10.0f;

    // Constantly moving to the left and checking for the out-of-bound to be destroyed
    void Update()
    {
        transform.Translate(Vector2.up * (Time.deltaTime * speed));
        
        if (transform.position.x < xOutOfBound)
        {
            Destroy(gameObject);
        }
    }
}
