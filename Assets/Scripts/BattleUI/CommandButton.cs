using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField] BattleUI ui;
    [SerializeField] private int commandIndex;
    [SerializeField] private TextMeshProUGUI commandText;
    
    private void Start()
    {
        commandIndex = 0;
        commandText.text = ((Command) commandIndex).ToString();
    }

    public void OnClick()
    {
        ui.OnCommandChosen(commandIndex);
    }
}
