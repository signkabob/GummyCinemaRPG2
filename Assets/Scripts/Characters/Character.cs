using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Character : MonoBehaviour
{
    [Header("Base")]
    public string CharacterName;
    public int MaxHealth = 20;

    public event Action HealthChanged;
    public event Action Died;
    
    public Team Team;
    protected List<ActionChoice> possibleActions = new List<ActionChoice>();

    protected Rigidbody2D rigidbody;
    protected Animator animator;
    protected ParticleSystem kaboomVFX;
    protected TextMeshPro damageIndicator;
    [SerializeField] protected string damageIndicatorName = "Damage Popup";
    private BattleStage battleStage;
    
    [Header("Status")]
    [SerializeField] private int currentHealth;
    [SerializeField] private string plannedLabel;
    [SerializeField] private Character plannedTarget;
    private Action onEndOfTurn;
    
    public int CurrentHealth { get { return currentHealth; } }
    
    protected BattleStage BattleStage { get { return battleStage; } }

    
    
    public bool IsAlive
    {
        get { return currentHealth > 0; }
    }
    
    protected virtual void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        damageIndicator = transform.Find(damageIndicatorName).GetComponent<TextMeshPro>();
        currentHealth = MaxHealth;
        RegisterActions();
    }

    public void Initialize(BattleStage battleStage)
    {
        this.battleStage = battleStage;
    }

    protected virtual void RegisterActions()
    {
        Debug.Log("Registering Actions");
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

    public virtual void BasicAttack()
    {
        Debug.Log("Basic Attack!");
    }

    public virtual void SpecialAttack()
    {
        Debug.Log("Special Attack!");
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
