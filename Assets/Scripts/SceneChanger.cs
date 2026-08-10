using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Excercise 03.4: SceneChanger.cs
 * Name: Ka Bo Cheung
 * Date: 08/01/2026
 * Course: GAME-1377-001
 *
 * Script for changing the scene
 */
public class SceneChanger : MonoBehaviour
{
    [SerializeField] string gameSceneName = "Battle";

    /// <summary>
    /// Load the game scene
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    /// <summary>
    /// Load the main menu scene
    /// </summary>
    public void BackToMainMenu()
    {
        // Resets the time scale in case the game was paused 
        Time.timeScale = 1f;

        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Reload the game scene
    /// </summary>
    public void RestartGame()
    {
        // Resets the time scale in case the game was paused 
        Time.timeScale = 1f;

        // Reloads the currently active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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