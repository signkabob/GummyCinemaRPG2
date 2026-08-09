using System;
using System.Collections;
using UnityEngine;
using TMPro;
using Random = System.Random;

/**
 * GummyCinemaRPG - Player.cs
 * Name: Ka Bo Cheung
 * Date: 05/11/2026
 * Course: GAME-2341-001
 * 
 * Script for the player in the turn-based RPG battle
 */
public class Player : MonoBehaviour
{
    // Defined global variables
    private BattleManager battleManager;
    private CameraShake camera;
    private Rigidbody2D playerRb;
    private Animator playerAnim;
    private AudioSource playerAudio;
    public AudioClip jumpSound;
    public AudioClip parrySound;
    public AudioClip axeCrash;
    public AudioClip giantCrash;
    public ParticleSystem kaboom;
    private TextMeshPro textPopup;
    public GameObject minecartPrefab;
    public GameObject dicePrefab;
    private Animator axeAnim;

    public int maxHP = 20;
    public int currentHP = 20;
    public int maxCP;
    public int currentCP;
    private int baseDmg = 3;
    public int minecartDmg = 4;
    public int minecartCP = 5;
    public int diceCP = 7;
    public int axeDmg = 10;
    public int axeCP = 10;
    public int giantDmg = 10;
    public int giantCP = 14;
    public int potionHP = 15;
    public int potionCount = 3;
    public int gelCP = 10;
    public int gelCount = 3;
    public bool reviveUsed = false;

    private float jumpForce = 13.0f;
    private float gravityModifier = 1.5f;
    private bool isOnStage = true;
    private bool parryNow = false;
    private float parryCooldownTime = 1.5f;
    private float nextParryTime = 0.0f;

    // Initialize
    private void Start()
    {
        battleManager =  GameObject.Find("Battle Manager").GetComponent<BattleManager>();
        camera = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        playerRb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponentInChildren<Animator>();
        playerAudio = GetComponentInChildren<AudioSource>();
        textPopup = transform.Find("Text Popup").GetComponent<TextMeshPro>();
        axeAnim = GameObject.Find("Axe").GetComponentInChildren<Animator>();
        
        Physics.gravity *= gravityModifier;
    }

    // Active player controls
    private void Update()
    {
        // As long as the battle goes...
        if (!battleManager.battleOver)
        {
            // Player's turn
            if (battleManager.isPlayerTurn)
            {
                PlayerTurn();
            }
            // Enemy's turn
            else
            {
                EnemyTurn();
            }
        }

        // Animate properly for falling
        if (!isOnStage && playerRb.linearVelocityY < 0.1f && currentHP > 0)
        {
            playerAnim.Play("Ruff_Fall");
        }
    }

    // Collision status checker
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the player touches the ground...
        if (collision.gameObject.CompareTag("Stage"))
        {
            isOnStage = true;
            playerAnim.Play("Ruff_Idle");
        }
    }

    // Trigger status checker
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // During the enemy turn...
        if (!battleManager.isPlayerTurn)
        {
            if (!parryNow)
            {
                // If colliding with the enemy...
                if (collider.gameObject.CompareTag("Enemy"))
                {
                    StartCoroutine(battleManager.DamagePlayer(collider.GetComponent<Enemy>().tackleDmg));
                }

                // If colliding with the fireball...
                if (collider.gameObject.CompareTag("Fireball"))
                {
                    StartCoroutine(battleManager.DamagePlayer(collider.GetComponent<Fireball>().damage));
                }
                // then the player takes damage
            }
            else
            {
                // If colliding with the enemy while parrying...
                if (collider.gameObject.CompareTag("Enemy"))
                {
                    playerAudio.PlayOneShot(parrySound, 1.0f);
                    battleManager.DamageEnemy(collider.GetComponent<Enemy>().tackleDmg);
                    RestoreCP(1);
                }
                // colliding with the fireball while parrying...
                if (collider.gameObject.CompareTag("Fireball"))
                {
                    playerAudio.PlayOneShot(parrySound, 1.0f);
                    RestoreCP(1);
                }
                // then the enemy takes damage on contact while the player restores 1 CP
            }
        }
    }

    // Available commands and controls during the player turn
    private void PlayerTurn()
    {
        // During the attack selection...
        if (battleManager.targetMode || battleManager.targetAllMode)
        {
            TargetCommand();
        }
        // During the cinema selection...
        else if (battleManager.cinemaMode)
        {
            CinemaCommand();
        }
        // During the item selection...
        else if (battleManager.itemsMode)
        {
            // item command goes here
            ItemsCommand();
        }
        else if (battleManager.runMode)
        {
            RunCommand();
        }
        // During the command selection...
        else
        {
            if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
            {
                battleManager.ChangeCommand(false);
            }

            if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
            {
                battleManager.ChangeCommand(true);
            }

            if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.KeypadEnter))
            {
                battleManager.SelectCommand();
            }
        }
    }
    
    // Player can only jump or parry during the enemy turn
    private void EnemyTurn()
    {
        // Check if the player is on the stage to prevent the double jump
        if (isOnStage && currentHP > 0)
        {
            // If the space bar, W, or left click is pressed, jump to dodge enemy attacks
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isOnStage = false;
                playerAnim.Play("Ruff_Jump");
                playerAudio.PlayOneShot(jumpSound, 1.0f);
            }
            // if the left arrow, A, or right click is pressed, parry enemy attacks at perfect timing
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Mouse1))
            {
                // Parry action
                if (Time.time >= nextParryTime)
                {
                    StartCoroutine(Parry());
                }
                // Parry cooldown
                else
                {
                    textPopup.text = "NOT YET!";
                    textPopup.color = Color.yellow;
                    textPopup.GetComponent<Animator>().Play("Damage_Popup");
                }
            }
        }
    }

    // Available options and controls during the target selection
    public void TargetCommand()
    {
        if (!battleManager.combatMode)
        {
            if (Input.GetKeyUp(KeyCode.Backspace) || Input.GetKeyUp(KeyCode.Escape) 
                                                  || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
            {
                battleManager.BackToCommand();
            }
            else if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.KeypadEnter))
            {
                battleManager.SelectTarget();
            }
            else if (!battleManager.targetAllMode)
            {
                if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
                {
                    battleManager.ChangeTarget(true);
                }
                else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
                {
                    battleManager.ChangeTarget(false);
                }
            }
        }
    }

    // Available options and control during the cinema selection
    public void CinemaCommand()
    {
        if (!battleManager.combatMode)
        {
            if (Input.GetKeyUp(KeyCode.Backspace) || Input.GetKeyUp(KeyCode.Escape)
                                                  || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
            {
                battleManager.BackToCommand();
            }
            else if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.KeypadEnter))
            {
                if (battleManager.currentCinemaMove == 0 && (currentCP - minecartCP) >= 0)
                {
                    battleManager.TargetAllMode();
                }
                else if (battleManager.currentCinemaMove == 1 && (currentCP - diceCP) >= 0)
                {
                    battleManager.TargetAllMode();
                }
                else if (battleManager.currentCinemaMove == 2 && (currentCP - axeCP) >= 0)
                {
                    battleManager.TargetMode();
                }
                else if (battleManager.currentCinemaMove == 3 && (currentCP - giantCP) >= 0)
                {
                    battleManager.TargetAllMode();
                }
                else
                {
                    battleManager.announcerText.text = "Not enough CP!";
                }
            }
            else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
            {
                battleManager.ChangeCinemaMove(true);
            }
            else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
            {
                battleManager.ChangeCinemaMove(false);
            }
        }
    }

    // Available options and control during the item selection
    public void ItemsCommand()
    {
        if (!battleManager.combatMode)
        {
            if (Input.GetKeyUp(KeyCode.Backspace) || Input.GetKeyUp(KeyCode.Escape)
                                                  || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
            {
                battleManager.BackToCommand();
            }
            else if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.KeypadEnter))
            {
                if (battleManager.currentItem == 0 && potionCount > 0)
                {
                    battleManager.SelectTarget();
                }
                else if (battleManager.currentItem == 1 && gelCount > 0)
                {
                    battleManager.SelectTarget();
                }
                else
                {
                    battleManager.announcerText.text = "Unable to use an item!";
                }
            }
            else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
            {
                battleManager.ChangeItem(true);
            }
            else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
            {
                battleManager.ChangeItem(false);
            }
        }
    }

    // Instant game over and return to the title screen once the run option is selected
    public void RunCommand()
    {
        if (!battleManager.combatMode)
        {
            if (Input.GetKeyUp(KeyCode.Backspace) || Input.GetKeyUp(KeyCode.Escape) 
                                                  || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
            {
                battleManager.BackToCommand();
            }
            else if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.KeypadEnter))
            {
                StartCoroutine(battleManager.GameOver());
            }
        }
    }
    
    // Begins the turn-based player combat on the selected command
    public IEnumerator Combat()
    {
        battleManager.TurnOffUI();
        if (battleManager.attackMode)
        {
            yield return StartCoroutine(Attack());
        }
        else if (battleManager.cinemaMode)
        {
            yield return StartCoroutine(Cinema());
        }
        else if (battleManager.itemsMode)
        {
            yield return StartCoroutine(Item());
        }
        else
        {
            Debug.Log("SOMETHING'S WRONG");
        }
        // Starts the enemy turn once the player action is done.
        yield return StartCoroutine(battleManager.StartEnemyTurn());
    } 

    // Cinema action
    private IEnumerator Cinema()
    {
        switch (battleManager.currentCinemaMove)
        {
            case 0:
                yield return StartCoroutine(Minecart());
                break;
            case 1:
                yield return StartCoroutine(Dices());
                break;
            case 2:
                yield return StartCoroutine(Axe());
                break;
            case 3:
                yield return StartCoroutine(Giant());
                break;
        }
        yield return null;
    }

    // Item action
    private IEnumerator Item()
    {
        playerAnim.Play("Ruff_Item");
        switch (battleManager.currentItem)
        {
            // Potion - Restore 15 HP
            case 0:
                RestoreHP(potionHP);
                potionCount -= 1;
                battleManager.potionText.text = "Potion x" + potionCount.ToString();
                break;
            // Gel - Restore 10 CP
            case 1:
                RestoreCP(gelCP);
                gelCount -= 1;
                battleManager.gelText.text = "Gel x" + gelCount.ToString();
                break;
            // Phoenix Down - Automatically used once the player is downed
        }
        yield return null;
    }

    // Base Attack - dealt 3 damage to one enemy
    private IEnumerator Attack()
    {
        playerAnim.Play("Ruff_Attack");
        yield return null;
        yield return new WaitForSeconds(playerAnim.GetCurrentAnimatorStateInfo(0).length/2.0f);
        battleManager.DamageEnemy(baseDmg);
        yield return new WaitForSeconds(playerAnim.GetCurrentAnimatorStateInfo(0).length/2.0f);
        yield return StartCoroutine(battleManager.CheckDownedEnemy());
    }
    
    // Parry - reflect back the tackle damage from the tackling enemy when parrying.
    private IEnumerator Parry()
    {
        parryNow = true;
        playerAnim.Play("Ruff_Parry");
        yield return new WaitForSeconds(playerAnim.GetCurrentAnimatorStateInfo(0).length);
        parryNow = false;
        nextParryTime = Time.time + parryCooldownTime;
        yield return null;
    }

    // Minecart Rush - dealt 4 damage to all enemies at once for 5 CP
    private IEnumerator Minecart()
    {
        usedCP(minecartCP);
        playerAnim.Play("Ruff_Item");
        Instantiate(minecartPrefab, transform.position, minecartPrefab.transform.rotation);
        yield return new WaitForSeconds(0.25f);
        yield return StartCoroutine(battleManager.DamageAllEnemies((minecartDmg)));
        yield return null;
    }

    // Lady Luck - dealt random damage from 1 to 6 to one random enemy and then repeat once more
    // for 7 CP. If the dual damages are the same, then repeat the process. 
    private IEnumerator Dices()
    {
        Random rng = new Random();
        GameObject dice1;
        GameObject dice2;
        int dice1Dmg;
        int dice2Dmg;
        
        usedCP(diceCP);
        while (true)
        {
            playerAnim.Play("Ruff_Item");
            dice1 = Instantiate(dicePrefab, dicePrefab.transform.position + new Vector3(-1,0,0), 
                Quaternion.Euler(rng.Next(0,361), rng.Next(0,361), rng.Next(0,361)));
            dice2 = Instantiate(dicePrefab, dicePrefab.transform.position + new Vector3(1,0,0), 
                Quaternion.Euler(rng.Next(0,361), rng.Next(0,361), rng.Next(0,361)));
            dice1Dmg = rng.Next(1, 6);
            dice2Dmg = rng.Next(1, 6);
            battleManager.announcerText.text = "Rolled " + dice1Dmg + " & " + dice2Dmg;
            yield return new WaitForSeconds(1.0f);
                    
            battleManager.currentTarget = rng.Next(0, battleManager.enemies.Count);
            battleManager.DamageEnemy((dice1Dmg));
            yield return StartCoroutine(battleManager.CheckDownedEnemy());
            yield return new WaitForSeconds(1.0f);

            if (battleManager.enemies.Count == 0)
            {
                break;
            }
                    
            battleManager.currentTarget = rng.Next(0, battleManager.enemies.Count);
            battleManager.DamageEnemy((dice2Dmg));
            yield return StartCoroutine(battleManager.CheckDownedEnemy());
            yield return new WaitForSeconds(1.0f);
            
            Destroy(dice1);
            Destroy(dice2);
            yield return new WaitForSeconds(1.0f);

            if ((dice1Dmg == dice2Dmg) && (battleManager.enemies.Count > 0))
            {
                battleManager.announcerText.text = "Doubled! Roll again!";
                yield return new WaitForSeconds(2.0f);
            }
            else
            {
                break;
            }
        }
        yield return null;
    }

    // Ragnarok - dealt 10 damage to one enemy for 10 CP
    private IEnumerator Axe()
    {
        usedCP(axeCP);
        playerAnim.Play("Ruff_Item");
        axeAnim.Play("Axe_Strike");
        yield return null;
        yield return new WaitForSeconds(axeAnim.GetCurrentAnimatorStateInfo(0).length/2.0f);
        StartCoroutine(camera.Shake(1.0f,0.1f));
        playerAudio.PlayOneShot(axeCrash, 1.0f);
        battleManager.DamageEnemy(axeDmg);
        yield return StartCoroutine(battleManager.CheckDownedEnemy());
        yield return new WaitForSeconds(axeAnim.GetCurrentAnimatorStateInfo(0).length/2.0f);
    }

    // HERE I COME - dealt 10 damage to all enemies at once for 14 CP
    private IEnumerator Giant()
    {
        usedCP(giantCP);
        playerAnim.Play("Ruff_Giant");
        yield return null;
        yield return new WaitForSeconds(playerAnim.GetCurrentAnimatorStateInfo(0).length/2.0f);
        StartCoroutine(camera.Shake(1.0f,0.1f));
        playerAudio.PlayOneShot(giantCrash, 1.0f);
        yield return StartCoroutine(battleManager.DamageAllEnemies((giantDmg)));
        yield return new WaitForSeconds(playerAnim.GetCurrentAnimatorStateInfo(0).length/2.0f);
    }
        
    // Player takes damage based on the encounter
    public void DamageTaken(int HP)
    {
        playerAnim.Play("Ruff_Damaged");
        currentHP = Math.Max(currentHP - HP, 0);
        textPopup.text = HP.ToString();
        textPopup.color = Color.red;
        textPopup.GetComponent<Animator>().Play("Damage_Popup");
    }

    // Player restores health points based on the encounter
    public void RestoreHP(int HP)
    {
        currentHP = Math.Min(currentHP + HP, maxHP);
        textPopup.text = "+" + HP.ToString() + " HP";
        textPopup.color = Color.green;
        textPopup.GetComponent<Animator>().Play("Damage_Popup");
        battleManager.playerHPText.text = "RUFF HP: " + currentHP + "/" + maxHP;
    }

    // Player uses up cinema points based on the encounter
    public void usedCP(int CP)
    {
        currentCP = Math.Max(currentCP - CP, 0);
        battleManager.playerCPText.text = "CP: " + currentCP + "/" + maxCP;
    }
    
    // Player restores cinema points based on the encounter 
    public void RestoreCP(int CP)
    {
        currentCP = Math.Min(currentCP + CP, maxCP);
        textPopup.text = "+" + CP.ToString() + " CP";
        textPopup.color = Color.blue;
        textPopup.GetComponent<Animator>().Play("Damage_Popup");
        battleManager.playerCPText.text = "CP: " + currentCP + "/" + maxCP;
    }
    
    // Process to die once player HP is down to 0
    public IEnumerator Dying()
    {
        playerAnim.Play("Ruff_Dying");
        yield return new WaitForSeconds(playerAnim.GetCurrentAnimatorStateInfo(0).length/2);
        kaboom.Play();
        yield return new WaitForSeconds(playerAnim.GetCurrentAnimatorStateInfo(0).length/2);
    }

    // Process to revive once during the game
    public IEnumerator Revive()
    {
        RestoreHP(maxHP);
        playerAnim.Play("Ruff_Revive");
        yield return new WaitForSeconds(playerAnim.GetCurrentAnimatorStateInfo(0).length);
        battleManager.reviveText.text = "Phoenix Down x0";
        reviveUsed = true;
        yield return null;
    }
}