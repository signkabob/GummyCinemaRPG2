using UnityEngine;

/*
 * Final Project: ActionButton.cs
 * Name: Ka Bo Cheung
 * Date: 08/10/2026
 * Course: GAME-1377-001
 *
 * Script for the action button; Unused but for reference purposes
 * Source: UtsabKDas's Game1377_OOP
 */
public class ActionButton : MonoBehaviour
{
    private BattleUI ui;
    private int actionIndex;

    /// <summary>
    /// Assign the battle ui and index to this button
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="index"></param>
    public void Setup(BattleUI owner, int index)
    {
        ui = owner;
        actionIndex = index;
    }

    public void OnClick()
    {
        ui.OnActionChosen(actionIndex);
    }
}