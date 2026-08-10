using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/*
 * Final Project: Character.cs
 * Name: Ka Bo Cheung
 * Date: 08/10/2026
 * Course: GAME-1377-001
 *
 * Script for the character class
 * Source: UtsabKas's GAME1377_OOP
 */
public abstract class Character : MonoBehaviour
{
    [Header("Base")]
    public string CharacterName;
    public int MaxHealth;

    public event Action HealthChanged;
    public event Action Died;
    
    public Team Team;
    protected List<ActionChoice> possibleActions = new List<ActionChoice>();

    protected Rigidbody2D rigidbody;
    protected Animator animator;
    [SerializeField] protected ParticleSystem kaboomVFX;
    [SerializeField] TextMeshPro damageIndicator;
    //[SerializeField] protected string damageIndicatorName = "Damage Popup";
    private BattleStage battleStage;
    
    [Header("Status")]
    [SerializeField] private int currentHealth;
    [SerializeField] private string plannedLabel;
    [SerializeField] private Character plannedTarget;
    private Action onEndOfTurn;
    
    public int CurrentHealth { get { return currentHealth; } }
    public List<ActionChoice> Actions {get {return possibleActions;}}
    
    protected BattleStage BattleStage { get { return battleStage; } }
    
    public bool IsAlive
    {
        get { return currentHealth > 0; }
    }
    
    protected virtual void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        //damageIndicator = transform.Find(damageIndicatorName).GetComponent<TextMeshPro>();
        currentHealth = MaxHealth;
        RegisterActions();
    }

    /// <summary>
    /// Initialized all characters set on the battle stage
    /// </summary>
    /// <param name="battleStage">the stage where the battle occurs</param>
    public void Initialize(BattleStage battleStage)
    {
        this.battleStage = battleStage;
    }

    /// <summary>
    /// Register the actions
    /// </summary>
    protected virtual void RegisterActions()
    {
        Debug.Log("Registering Actions");
    }

    /// <summary>
    /// Plan the action for the target in the battle
    /// </summary>
    /// <param name="choice"></param>
    /// <param name="target"></param>
    public void PlanAction(ActionChoice choice, Character target)
    {
        onEndOfTurn = choice.Perform;
        plannedLabel = choice.Label;
        plannedTarget = target;
    }
    
    /// <summary>
    /// Raise the end-of-turn event
    /// </summary>
    public void RaiseEndOfTurn()
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

    /// <summary>
    /// Use the basic attack
    /// </summary>
    public virtual void BasicAttack()
    {
        Debug.Log("Basic Attack!");
    }

    /// <summary>
    /// Use the special attack
    /// </summary>
    public virtual void SpecialAttack()
    {
        Debug.Log("Special Attack!");
    }

    /// <summary>
    /// Ensures the valid enemy target
    /// </summary>
    /// <returns>the planned target or random living opponent </returns>
    protected Character ResolveEnemytarget()
    {
        if (plannedTarget != null && plannedTarget.IsAlive)
        {
            return plannedTarget;
        }

        return BattleStage.GetRandomLivingOpponent(this);
    }

    /// <summary>
    /// Takes damage from the source
    /// </summary>
    /// <param name="damageAmount">damage point</param>
    /// <param name="source">damage source</param>
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

    /// <summary>
    /// Restore health from the source
    /// </summary>
    /// <param name="healAmount">health points</param>
    /// <param name="source">healing source</param>
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
