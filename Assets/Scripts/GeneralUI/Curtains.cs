using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/**
 * GummyCinemaRPG - PlayButton.cs
 * Name: Ka Bo Cheung
 * Date: 05/11/2026
 * Course: GAME-2341-001
 *
 * Script for the button to start playing the game
 */
public class Curtains : MonoBehaviour
{
    [SerializeField] private string closedCurtainsStateName = "Curtains_Down";
    private Animator animator;
    
    // Initialize
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public IEnumerator CloseCurtains()
    {
        animator.Play(closedCurtainsStateName);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
    }
}
