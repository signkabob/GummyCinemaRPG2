using UnityEngine;
using UnityEngine.UI;

/**
 * GummyCinemaRPG - VerticalButton.cs
 * Name: Ka Bo Cheung
 * Date: 05/11/2026
 * Course: GAME-2341-001
 *
 * Script for the button to navigate vertically
 */
public class VerticalButton : MonoBehaviour
{
    // Defined global variables
    private Button button;
    private BattleManager battleManager;
    public bool downArrow;
    
    // Initialize
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    // Navigate vertically based on the direction and certain mode once clicked 
    private void OnClick()
    {
        
    }
}