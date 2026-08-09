using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public string CharacterName;
    public int MaxHealth = 20;

    public event Action HealthChanged;
    public event Action Died;
    
    // private BattleStage battleStage;
    private int currentHealth;
    private string plannedLabel;
    private Character plannedTarget;
    private Action onEndOfTurn;
    
    public int CurrentHealth { get { return currentHealth; } }
    
    // protected BattleStage Battlefield { get { return battleStage; } }

    public bool isAlive
    {
        get { return currentHealth > 0; }
    }
    
    protected virtual void Awake()
    {
        currentHealth = MaxHealth;
        RegisterActions();
    }
    
    //public void Initialize(BattleStage battleStage)

    protected virtual void RegisterActions()
    {
        // Register actions here
    }
}
