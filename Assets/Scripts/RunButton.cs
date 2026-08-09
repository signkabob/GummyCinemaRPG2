using UnityEngine;
using UnityEngine.UI;

/**
 * GummyCinemaRPG - RunButton.cs
 * Name: Ka Bo Cheung
 * Date: 05/11/2026
 * Course: GAME-2341-001
 *
 * Script for the button to end the battle early and return to the title screen
 */
public class RunButton : MonoBehaviour
{
    // Defined global variables
    private Button button;
    private BattleManager battleManager;
    
    // Initialize
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        battleManager = GameObject.Find("Battle Manager").GetComponent<BattleManager>();
    }

    // Run away from the battle once clicked
    private void OnClick()
    {
        StartCoroutine(battleManager.GameOver());
    }
}