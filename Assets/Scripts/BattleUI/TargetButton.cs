using UnityEngine;
/*
 * Final Project: TargetButton.cs
 * Name: Ka Bo Cheung
 * Date: 08/10/2026
 * Course: GAME-1377-001
 *
 * Script for the target button
 * Source: UtsabKDas's Game1377_OOP
 */
public class TargetButton : MonoBehaviour
{
    private BattleUI ui;
    private Character target;

    /// <summary>
    /// Assign the target and battle UI to this button
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="targetCharacter"></param>
    public void Setup(BattleUI owner, Character targetCharacter)
    {
        ui = owner;
        target = targetCharacter;
    }

    public void OnClick()
    {
        ui.OnTargetChosen(target);
    }
}