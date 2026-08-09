using System;

public static class BattleEvents
{
    public static event Action BattleStarted;
    public static event Action<int> RoundStarted;
    public static event Action<Character> HeroActionSelectionStarted;
    public static event Action HeroActionSelectionDone;
    public static event Action<Character, string> ActionPerformed;
    public static event Action<Character, Character, int> DamageDealt;
    public static event Action<Character, Character, int> Healed;
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

    public static void RaiseHeroActionSelectionStarted(Character hero) 
    { 
        HeroActionSelectionStarted?.Invoke(hero); 
    }

    public static void RaiseHeroActionSelectionDone() 
    { 
        HeroActionSelectionDone?.Invoke(); 
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
    public static void RaiseCharacterDied(Character character) 
    { 
        CharacterDied?.Invoke(character); 
    }
    
    public static void RaiseBattleEnded(Team winner) 
    { 
         BattleEnded?.Invoke(winner); 
    }
}