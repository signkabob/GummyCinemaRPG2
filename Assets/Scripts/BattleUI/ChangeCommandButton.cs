using UnityEngine;
using UnityEngine.UI;

/**
 * GummyCinemaRPG - ChangeCommandButton.cs
 * Name: Ka Bo Cheung
 * Date: 05/11/2026
 * Course: GAME-2341-001
 *
 * Script for the button to swap the current command
 */
public class ChangeCommandButton : MonoBehaviour
{
    // Defined global variables
    private Button button;
    private BattleManager battleManager;
    public bool rightArrow;
    
    // Initialize
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    // Swap the command based on the direction once clicked 
    private void OnClick()
    {
        // battleManager.ChangeCommand(rightArrow);
    }
}
