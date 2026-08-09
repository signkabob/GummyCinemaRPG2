using System.Collections;
using UnityEngine;

public class Gangster : Enemy
{
    [Header("Gangster's Attacks")]
    [SerializeField] private int tackleDamage;
    [SerializeField] private int blasterDamage;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Vector3 blasterAimOffset = new Vector3(-1,0,0);

    [Header("Gangster's Positioning")] 
    [SerializeField] private Transform spriteCenterOrigin;
    [SerializeField] private Vector3 originalPosition;
    [SerializeField] private float speedForce = 20.0f;
    [SerializeField] private float xOutOfBound = -10.0f;
    [SerializeField] private float entryFromWrap = 10.0f;
    [SerializeField] private float stoppingDistance = 5.0f;
    [SerializeField] private bool isWrapped = false;

    [Header("Gangster's Animation")]
    [SerializeField] private string idleAnimationState = "Gangster_Idle";
    [SerializeField] private string tackleAnimationState = "Gangster_Run";
    [SerializeField] private string blasterAnimationState = "Gangster_Blast";
    [SerializeField] private string dyingAnimationState = "Gangster_Dying";
    
    private void Start()
    {
        // The parent origin starts at the bottom while the child origin starts at the center
        spriteCenterOrigin = transform.GetChild(0).transform; 
        originalPosition = transform.position;
    }
    
    private void Update()
    {
        // If the enemy goes beyond the left bound of the screen...
        if (transform.position.x < xOutOfBound)
        {
            WrapToOtherSide();
        }
        
        // If the enemy is about to reach its spawning position after wrapping...
        if (isWrapped && (transform.position.x - originalPosition.x < stoppingDistance))
        {
            Brake();
        }
    }
    
    /// <summary>
    /// Register Gangster's battle actions
    /// </summary>
    protected override void RegisterActions()
    {
        possibleActions.Add(new ActionChoice("Tackle", TargetKind.SingleEnemy, Tackle));
        possibleActions.Add(new ActionChoice("Blaster", TargetKind.SingleEnemy, Blaster));
    }

    /// <summary>
    /// Play the idle animation
    /// </summary>
    public void Idle()
    {
        animator.Play(idleAnimationState);
    }

    /// <summary>
    /// Rush to the left across the stage
    /// </summary>
    public void Tackle()
    {
        animator.Play(tackleAnimationState);
        rigidbody.AddForce(Vector2.left * speedForce, ForceMode2D.Impulse);
    }
    
    /// <summary>
    ///  Spawn a fireball to the left across the stage 
    /// </summary>
    public void Blaster()
    {
        animator.Play(blasterAnimationState);
        GameObject fireball = Instantiate(fireballPrefab, spriteCenterOrigin.position + blasterAimOffset, fireballPrefab.transform.rotation);
        // set fireball damage and source
        // wait for the fireball to pass
    }

    /// <summary>
    /// Wrap from the left bound to the right bound of the stage
    /// </summary>
    private void WrapToOtherSide()
    {
        {
            transform.position = new Vector2(entryFromWrap, transform.position.y);
            isWrapped = true;
        }
    }

    /// <summary>
    /// Brake immediately on the original positon 
    /// </summary>
    private void Brake()
    {
        rigidbody.linearVelocity = Vector2.zero;
        rigidbody.angularVelocity = 0.0f;
        transform.position = originalPosition;
        isWrapped = false;
    }

    /// <summary>
    /// Play the dying animation and VFX
    /// </summary>
    /// <returns></returns>
    public IEnumerator PlayDyingAnimation()
    {
        animator.Play(dyingAnimationState);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length/2);
        kaboomVFX.Play();
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length/2);
    }
}
