using System;
/*
 * Final Project: BattleEvents.cs
 * Name: Ka Bo Cheung
 * Date: 08/10/2026
 * Course: GAME-1377-001
 *
 * Script for the battle events
 * Source: UtsabKDas's Game1377_OOP
 */
public static class BattleEvents
{
    public static event Action BattleStarted;
    public static event Action<int> RoundStarted;
    public static event Action<Character> FriendActionSelectionStarted;
    public static event Action FriendActionSelectionDone;
    public static event Action<Character, string> ActionPerformed;
    public static event Action<Character, Character, int> DamageDealt;
    public static event Action<Character, Character, int> Healed;
    public static event Action<Character, Character, int> CinemaRestored;
    public static event Action<Character> CharacterDied;
    public static event Action<Team> BattleEnded;

    public static void RaiseBattleStarted() 
    { 
        BattleStarted?.Invoke(); 
    }
    public static void RaiseRoundStarted(int round) 
    { 
        RoundStarted?.Invoke(round); 
    }

    public static void RaiseFriendActionSelectionStarted(Character friend) 
    { 
        FriendActionSelectionStarted?.Invoke(friend); 
    }

    public static void RaiseFriendActionSelectionDone() 
    { 
        FriendActionSelectionDone?.Invoke(); 
    }

    public static void RaiseActionPerformed(Character actor, string label) 
    { 
        ActionPerformed?.Invoke(actor, label); 
    }

    public static void RaiseDamageDealt(Character source, Character target, int amount) 
    { 
        DamageDealt?.Invoke(source, target, amount); 
    }
    public static void RaiseHealed(Character source, Character target, int amount) 
    { 
        Healed?.Invoke(source, target, amount); 
    }
    public static void RaiseCinemaRestored(Character source, Character target, int amount) 
    { 
        CinemaRestored?.Invoke(source, target, amount); 
    }
    public static void RaiseCharacterDied(Character character) 
    { 
        CharacterDied?.Invoke(character); 
    }
    
    public static void RaiseBattleEnded(Team winner) 
    { 
         BattleEnded?.Invoke(winner); 
    }
}