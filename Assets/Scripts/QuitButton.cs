using UnityEngine;
using UnityEngine.UI;

/**
 * GummyCinemaRPG - QuitButton.cs
 * Name: Ka Bo Cheung
 * Date: 05/11/2026
 * Course: GAME-2341-001
 *
 * Script for the button to quit the game
 */
public class QuitButton : MonoBehaviour
{
    // Defined global variables
    private Button button;
    private BattleManager battleManager;
    
    // Initialize
    void Start()
    {
        button = GetComponent<Button>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    // Quit the game once clicked
    private void OnClick()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}