using System.Collections;
using UnityEngine;
using TMPro;

/**
 * GummyCinemaRPG - Enemy.cs
 * Name: Ka Bo Cheung
 * Date: 05/11/2026
 * Course: GAME-2341-001
 * 
 * Script for the enemy in the turn-based RPG battle
 */
public class Enemy : MonoBehaviour
{
    // Defined global variables
    private BattleManager battleManager;
    private Rigidbody2D enemyRb;
    private Animator enemyAnim;
    public ParticleSystem kaboom;
    public GameObject fireballPrefab;
    private TextMeshPro dmgPopup;

    public Vector3 originalPos;
    public int maxHP;
    public int currentHP;
    public int tackleDmg;
    public int blastDmg;
    
    private System.Random random = new System.Random();
    private float speedForce = 20.0f;
    private float xOutOfBound = -10.0f;
    private bool walled = false;
    
    // Initialize
    private void Start()
    {
        battleManager =  GameObject.Find("Battle Manager").GetComponent<BattleManager>();
        enemyRb = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponentInChildren<Animator>();
        dmgPopup = transform.Find("Damage Popup").GetComponent<TextMeshPro>();
    }

    // Constantly checking for out-of-bound to be teleported to the other side and
    // stop at its original position
    private void Update()
    {
        if (transform.position.x < xOutOfBound)
        {
            transform.position = new Vector2(10.0f, transform.position.y);
            walled = true;
        }
        
        if (walled && (transform.position.x - originalPos.x < 0.5))
        {
            enemyRb.linearVelocity = Vector2.zero;
            enemyRb.angularVelocity = 0.0f;
            transform.position = originalPos;
            walled = false;
        }
    }
    
    // Begins the turn-based enemy combat with random attack
    public IEnumerator Combat()
    {
        battleManager.announcerText.text = name + "'s Turn";
        yield return new WaitForSeconds(1.0f);
        
        if (random.Next(2) == 0)
        {
            battleManager.announcerText.text = name + " used Tackle!";
            yield return new WaitForSeconds(2.0f);
            yield return StartCoroutine(Tackle());
        }
        else
        {
            battleManager.announcerText.text = name + " used Blaster!";
            yield return new WaitForSeconds(2.0f);
            yield return StartCoroutine(Blast());
        }
        yield return new WaitForSeconds(1.0f);
        enemyAnim.Play("Gangster_Idle");
    }

    // Tackle - rush toward the player, dealt damage on contact, and
    // then return to the original position 
    private IEnumerator Tackle()
    {
        enemyAnim.Play("Gangster_Run");
        enemyRb.AddForce(Vector2.left * speedForce, ForceMode2D.Impulse);
        walled = false;
        yield return null;
    }

    // Blaster - shoot a slow fireball toward the player and dealt damage on contact
    private IEnumerator Blast()
    {
        enemyAnim.Play("Gangster_Blast");
        GameObject fireball = Instantiate(fireballPrefab, transform.GetChild(0).transform.position + new Vector3(-1,0,0), fireballPrefab.transform.rotation);
        fireball.GetComponent<Fireball>().damage = blastDmg;
        yield return new WaitUntil(() => fireball.transform.position.x <= xOutOfBound + 1.0f);
    }

    // Enemy takes damage based on the encounter
    public void DamageTaken(int HP)
    {
        enemyAnim.Play("Gangster_Damaged");    
        currentHP -= HP;
        dmgPopup.text = HP.ToString();
        dmgPopup.GetComponent<Animator>().Play("Damage_Popup");
    }

    // Process to die once HP is down to 0
    public IEnumerator Dying()
    {
        enemyAnim.Play("Gangster_Dying");
        yield return new WaitForSeconds(enemyAnim.GetCurrentAnimatorStateInfo(0).length/2);
        kaboom.Play();
        yield return new WaitForSeconds(enemyAnim.GetCurrentAnimatorStateInfo(0).length/2);
    }
}