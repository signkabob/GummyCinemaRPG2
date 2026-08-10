using UnityEngine;
/*
 * Final Project: BattleLogger.cs
 * Name: Ka Bo Cheung
 * Date: 08/10/2026
 * Course: GAME-1377-001
 *
 * Script for the battle logger; Unused but for reference purposes
 * Source: UtsabKDas's Game1377_OOP
 */
public class BattleLogger : MonoBehaviour
{
    private void OnEnable()
    {
        BattleEvents.RoundStarted += OnRound;
        BattleEvents.ActionPerformed += OnAction;
        BattleEvents.DamageDealt += OnDamage;
        BattleEvents.Healed += OnHeal;
        BattleEvents.CharacterDied += OnDied;
        BattleEvents.BattleEnded += OnEnded;
    }

    private void OnDisable()
    {
        BattleEvents.RoundStarted -= OnRound;
        BattleEvents.ActionPerformed -= OnAction;
        BattleEvents.DamageDealt -= OnDamage;
        BattleEvents.Healed -= OnHeal;
        BattleEvents.CharacterDied -= OnDied;
        BattleEvents.BattleEnded -= OnEnded;
    }

    private void OnRound(int round)
    {
        Debug.Log("--- Round " + round + " ---");
    }

    private void OnAction(Character actor, string label)
    {
        Debug.Log(actor.CharacterName + " uses " + label);
    }

    private void OnDamage(Character source, Character target, int amount)
    {
        Debug.Log("   " + target.CharacterName + " takes " + amount + " (" + target.CurrentHealth + "/" + target.MaxHealth + ")");
    }

    private void OnHeal(Character source, Character target, int amount)
    {
        Debug.Log("   " + target.CharacterName + " heals " + amount + " (" + target.CurrentHealth + "/" + target.MaxHealth + ")");
    }

    private void OnDied(Character character)
    {
        Debug.Log("   " + character.CharacterName + " has fallen");
    }

    private void OnEnded(Team winner)
    {
        if (winner == Team.Party)
        {
            Debug.Log("=== Victory ===");
        }
        else
        {
            Debug.Log("=== Defeat ===");
        }
    }
}