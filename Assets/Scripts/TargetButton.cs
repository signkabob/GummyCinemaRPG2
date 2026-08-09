using UnityEngine;
using UnityEngine.UI;

/**
 * GummyCinemaRPG - TargetButton.cs
 * Name: Ka Bo Cheung
 * Date: 05/11/2026
 * Course: GAME-2341-001
 *
 * Script for the button to select the desired move, item, or target
 */
public class TargetButton : MonoBehaviour
{
    // Defined global variables
    private Button button;
    private BattleManager battleManager;
    private Player player;
    
    // Initialize
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        battleManager = GameObject.Find("Battle Manager").GetComponent<BattleManager>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Select the current move, item, or target once clicked
    private void OnClick()
    {
        if (battleManager.targetMode || battleManager.targetAllMode)
        {
            battleManager.SelectTarget();
        }
        else if (battleManager.cinemaMode)
        {
            if (battleManager.currentCinemaMove == 0 && (player.currentCP - player.minecartCP) >= 0)
            {
                battleManager.TargetMode();
            }
            else if (battleManager.currentCinemaMove == 1 && (player.currentCP - player.diceCP) >= 0)
            {
                battleManager.TargetAllMode();
            }
            else if (battleManager.currentCinemaMove == 2 && (player.currentCP - player.axeCP) >= 0)
            {
                battleManager.TargetMode();
            }
            else if (battleManager.currentCinemaMove == 3 && (player.currentCP - player.giantCP) >= 0)
            {
                battleManager.TargetAllMode();
            }
            else
            {
                battleManager.announcerText.text = "Not enough CP!";
            }
        }
        else if (battleManager.itemsMode)
        {
            if (battleManager.currentItem == 0 && player.potionCount > 0)
            {
                battleManager.SelectTarget();
            }
            else if (battleManager.currentItem == 1 && player.gelCount > 0)
            {
                battleManager.SelectTarget();
            }
            else
            {
                battleManager.announcerText.text = "Unable to use an item!";
            }
        }
    }
}