using System.Collections;
using UnityEngine;

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

    protected override void RegisterActions()
    {
        possibleActions.Add(new ActionChoice("Attack", TargetKind.SingleEnemy, BasicAttack));
    }

    public override void BasicAttack()
    {
        Character target = ResolveEnemytarget();
        if (target == null)
        {
            return;
        }
        StartCoroutine(PlayAttackAnimation(target));
    }
    
    private IEnumerator PlayAttackAnimation(Character target)
    {
        animator.Play(attackAnimationState);
        yield return null;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length/2.0f);
        target.TakeDamage(baseDamage, this);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length/2.0f);
    }
}
