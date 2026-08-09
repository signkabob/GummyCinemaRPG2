using UnityEngine;
using UnityEngine.UI;

/**
 * GummyCinemaRPG - CommandButton.cs
 * Name: Ka Bo Cheung
 * Date: 05/11/2026
 * Course: GAME-2341-001
 *
 * Script for the button to select the current command
 */
public class CommandButton : MonoBehaviour
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

    // Select the current command once clicked
    private void OnClick()
    {
        battleManager.SelectCommand();
    }
}
