using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/*
 * Final Project: Curtain.cs
 * Name: Ka Bo Cheung
 * Date: 08/10/2026
 * Course: GAME-1377-001
 *
 * Script for the curtain animation
 */
public class Curtains : MonoBehaviour
{
    [SerializeField] private string closedCurtainsStateName = "Curtains_Down";
    private Animator animator;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Play the closing curtain animation
    public IEnumerator CloseCurtains()
    {
        animator.Play(closedCurtainsStateName);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
    }
}
