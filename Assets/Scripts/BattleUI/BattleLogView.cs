using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleLogView : MonoBehaviour
{
    public TMP_Text logText;
    public int maxLines = 6;

    private Queue<string> logLines = new Queue<string>();

    private void OnEnable()
    {
        BattleEvents.ActionPerformed += OnActionPerformed;
        BattleEvents.DamageDealt += OnDamageDealt;
        BattleEvents.Healed += OnHealed;
        BattleEvents.CharacterDied += OnCharacterDied;
    }

    private void OnDisable()
    {
        BattleEvents.ActionPerformed -= OnActionPerformed;
        BattleEvents.DamageDealt -= OnDamageDealt;
        BattleEvents.Healed -= OnHealed;
        BattleEvents.CharacterDied -= OnCharacterDied;
    }

    private void OnActionPerformed(Character actor, string label)
    {
        AddLog(actor.CharacterName + " uses " + label);
    }

    private void OnDamageDealt(Character source, Character target, int amount)
    {
        AddLog(target.CharacterName + " takes " + amount + " damage");
    }

    private void OnHealed(Character source, Character target, int amount)
    {
        AddLog(target.CharacterName + " heals " + amount);
    }

    private void OnCharacterDied(Character character)
    {
        AddLog(character.CharacterName + " has fallen");
    }

    private void AddLog(string line)
    {
        logLines.Enqueue(line);
        while (logLines.Count > maxLines)
        {
            logLines.Dequeue();
        }
        if (logText != null)
        {
            logText.text = string.Join("\n", logLines);
        }
    }
}