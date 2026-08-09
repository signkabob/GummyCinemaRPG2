using System.Collections;
using UnityEngine;

public class Gangster : Enemy
{
    [Header("Gangster Attacks")]
    [SerializeField] private int tackleDamage;
    [SerializeField] private int blasterDamage;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Vector3 blasterAimOffset = new Vector3(-1,0,0);

    [Header("Positioning")] 
    [SerializeField] private Transform spriteCenterOrigin;
    [SerializeField] private Vector3 originalPosition;
    [SerializeField] private float speedForce = 20.0f;
    [SerializeField] private float xOutOfBound = -10.0f;
    [SerializeField] private float entryFromWrap = 10.0f;
    [SerializeField] private float stoppingDistance = 5.0f;
    [SerializeField] private bool isWrapped = false;

    [Header("Animation")]
    [SerializeField] private string idleAnimationState = "Gangster_Idle";
    [SerializeField] private string tackleAnimationState = "Gangster_Run";
    [SerializeField] private string blasterAnimationState = "Gangster_Blast";
    [SerializeField] private string dyingAnimationState = "Gangster_Dying";
    
    private void Start()
    {
        spriteCenterOrigin = transform.GetChild(0).transform;
        originalPosition = transform.position;
    }
    
    // Constantly checking for out-of-bound to be teleported to the other side and
    // stop at its original position
    private void Update()
    {
        if (transform.position.x < xOutOfBound)
        {
            WrapToOtherSide();
        }

        if (isWrapped && (transform.position.x - originalPosition.x < stoppingDistance))
        {
            Brake();
        }
    }
    
    protected override void RegisterActions()
    {
        possibleActions.Add(new ActionChoice("Tackle", TargetKind.SingleEnemy, Tackle));
        possibleActions.Add(new ActionChoice("Blaster", TargetKind.SingleEnemy, Blaster));
    }

    public void Idle()
    {
        animator.Play(idleAnimationState);
    }

    public void Tackle()
    {
        animator.Play(tackleAnimationState);
        rigidbody.AddForce(Vector2.left * speedForce, ForceMode2D.Impulse);
    }
    
    public void Blaster()
    {
        animator.Play(blasterAnimationState);
        GameObject fireball = Instantiate(fireballPrefab, spriteCenterOrigin.position + blasterAimOffset, fireballPrefab.transform.rotation);
        // set fireball damage and source
        // wait for the fireball to pass
    }

    private void WrapToOtherSide()
    {
        {
            transform.position = new Vector2(entryFromWrap, transform.position.y);
            isWrapped = true;
        }
    }

    private void Brake()
    {
        rigidbody.linearVelocity = Vector2.zero;
        rigidbody.angularVelocity = 0.0f;
        transform.position = originalPosition;
        isWrapped = false;
    }

    public IEnumerator Dying()
    {
        animator.Play(dyingAnimationState);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length/2);
        kaboomVFX.Play();
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length/2);
    }
}
