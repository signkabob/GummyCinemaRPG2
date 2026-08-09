using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public string CharacterName;
    public int MaxHealth = 20;

    public event Action HealthChanged;
    public event Action Died;
    
    private BattleStage battleStage;
    private int currentHealth;
    private string plannedLabel;
    private Character plannedTarget;
    private Action onEndOfTurn;
    
    public int CurrentHealth { get { return currentHealth; } }
    
    protected BattleStage BattleStage { get { return battleStage; } }

    public Team Team;
    
    public bool IsAlive
    {
        get { return currentHealth > 0; }
    }
    
    protected virtual void Awake()
    {
        currentHealth = MaxHealth;
        RegisterActions();
    }

    public void Initialize(BattleStage battleStage)
    {
        this.battleStage = battleStage;
    }

    protected virtual void RegisterActions()
    {
        // Register actions here
    }

    public void PlanAction(ActionChoice choice, Character target)
    {
        onEndOfTurn = choice.Perform;
        plannedLabel = choice.Label;
        plannedTarget = target;
    }
    
    public void RaiseEndofTurn()
    {
        if (!IsAlive)
        {
            return;
        }
        if (onEndOfTurn != null)
        {
            return;
        }
        BattleEvents.RaiseActionPerformed(this, plannedLabel);
        onEndOfTurn.Invoke();
        onEndOfTurn = null;
    }

    protected Character ResolveEnemytarget()
    {
        if (plannedTarget != null && plannedTarget.IsAlive)
        {
            return plannedTarget;
        }

        return BattleStage.GetRandomLivingOpponent(this);
    }

    public void TakeDamage(int damageAmount, Character source)
    {
        if (!IsAlive)
        {
            return;
        }
        currentHealth -=  damageAmount;
        BattleEvents.RaiseDamageDealt(source, this, damageAmount);
        HealthChanged?.Invoke();
        if (!IsAlive)
        {
            Died?.Invoke();
            BattleEvents.RaiseCharacterDied(this);
        }
    }

    public void Heal(int healAmount, Character source)
    {
        if (!IsAlive)
        {
            return;
        }

        currentHealth = Math.Min(currentHealth + healAmount, MaxHealth);
        BattleEvents.RaiseHealed(source, this, healAmount);
        HealthChanged?.Invoke();
    }
}
