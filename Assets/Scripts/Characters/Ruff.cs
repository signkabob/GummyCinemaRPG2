using System.Collections;
using UnityEngine;
/*
 * Final Project: Ruff.cs
 * Name: Ka Bo Cheung
 * Date: 08/10/2026
 * Course: GAME-1377-001
 *
 * Script for the player character derived from friend class
 */
public class Ruff : Friend
{
    public enum State
    {
        Idle,
        Attack,
        Item,
        Jump,
        Fall,
        Parry,
        Damaged,
        Dying
    }
    private State currentState;
    
    [Header("Ruff's Attacks")]
    [SerializeField] private int baseDamage;
    
    [Header("Ruff's Defending Moves")]
    [SerializeField] private float jumpForce = 13.0f;
    [SerializeField] private float gravityModifer = 1.5f;
    [SerializeField] private float parryCooldownTime = 1.5f;
    [SerializeField] private float nextParryTime = 0.0f;
    
    [Header("Ruff's Animation")]
    [SerializeField] private string idleAnimationState = "Ruff_Idle";
    [SerializeField] private string attackAnimationState = "Ruff_Attack";
    [SerializeField] private string giantAnimationState = "Ruff_Giant";
    [SerializeField] private string jumpAnimationState = "Ruff_Jump";
    [SerializeField] private string fallAnimationState = "Ruff_Fall";
    [SerializeField] private string parryAnimationState = "Ruff_Parry";
    [SerializeField] private string damageAnimationState = "Ruff_Damaged";
    [SerializeField] private string dyingAnimationState = "Ruff_Dying";
    [SerializeField] private string reviveAnimationState = "Ruff_Revive";
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = State.Idle;
    }

    /// <summary>
    /// Register the actions
    /// </summary>
    protected override void RegisterActions()
    {
        possibleActions.Add(new ActionChoice("Attack", TargetKind.SingleEnemy, BasicAttack));
    }

    /// <summary>
    /// Use the basic attack
    /// </summary>
    public override void BasicAttack()
    {
        Character target = ResolveEnemytarget();
        if (target == null)
        {
            return;
        }
        StartCoroutine(PlayAttackAnimation(target));
    }
    
    /// <summary>
    /// Play the basic attack animation
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private IEnumerator PlayAttackAnimation(Character target)
    {
        animator.Play(attackAnimationState);
        yield return null;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length/2.0f);
        target.TakeDamage(baseDamage, this);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length/2.0f);
    }
}
