using UnityEngine;
using UnityEngine.UI;

/**
 * GummyCinemaRPG - BackToCommandButton.cs
 * Name: Ka Bo Cheung
 * Date: 05/11/2026
 * Course: GAME-2341-001
 *
 * Script for the button leading back to the commands
 */
public class BackToCommandButton : MonoBehaviour
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

    // Return to the command list once clicked
    private void OnClick()
    {
        battleManager.BackToCommand();
    }
}