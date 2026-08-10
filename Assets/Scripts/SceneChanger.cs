using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
 * Excercise 03.4: SceneChanger.cs
 * Name: Ka Bo Cheung
 * Date: 08/10/2026
 * Course: GAME-1377-001
 *
 * Script for changing the scene
 */
public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string titleSceneName = "Title";
    [SerializeField] private string overworldSceneName = "Overworld";  
    [SerializeField] private string battleSceneName = "Battle";
    [SerializeField] private Curtains curtains;

    private IEnumerator CinematicLoading(string sceneName)
    {
        yield return StartCoroutine((curtains.CloseCurtains()));
        yield return null;
        SceneManager.LoadScene(sceneName);
    }
    
    /// <summary>
    /// Load the overworld game scene
    /// </summary>
    public void GoToOverworld()
    {
        StartCoroutine(CinematicLoading(overworldSceneName));
    }

    /// <summary>
    /// Load the battle game scene
    /// </summary>
    public void InitiateBattle()
    {
        StartCoroutine(CinematicLoading(battleSceneName));
    }

    /// <summary>
    /// Load the title screen scene
    /// </summary>
    public void BackToTitleScreen()
    {
        GameManager.Instance.Unpause();
        StartCoroutine(CinematicLoading(titleSceneName));
    }

    /// <summary>
    /// Quit the game application
    /// </summary>
    public void QuitGame()
    {
        // Logs a message in the console to confirm it works in the editor
        Debug.Log("Game is exiting...");

        // Quits the actual application build
        Application.Quit();

        // Quits the play mode in the editor 
        UnityEditor.EditorApplication.ExitPlaymode();
    }
}