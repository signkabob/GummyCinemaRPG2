using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/**
 * GummyCinemaRPG - BattleManager.cs
 * Name: Ka Bo Cheung
 * Date: 05/11/2026
 * Course: GAME-2341-001
 *
 * Script for the battle manager with the game states, UI, and updates
 */
public class BattleManager : MonoBehaviour
{
    private Player player;
    public Enemy GangsterPrefab;
    public Enemy GangsterElitePrefab;
    public Enemy GangsterBossPrefab;
    public List<Enemy> enemies = new List<Enemy>();

    private string[] commands = { "Attack", "Cinema", "Items", "Run" };
    private int currentCommand = 0;
    public int currentTarget = 0;
    public int currentCinemaMove = 0;
    public int currentItem = 0;
    private int wave = 1;
    
    public Button commandButton;
    public Button leftButton;
    public Button rightButton;
    public Button upButton;
    public Button downButton;
    public Button backButton;
    public Button runButton;
    public Button targetButtonPrefab;
    private List<Button> targetButtons = new List<Button>();
    public Button selectCinemaMoveButton;
    public Button selectItemButton;
    
    public TextMeshProUGUI playerHPText;
    public TextMeshProUGUI playerCPText;
    public GameObject cinemaMoves;
    public GameObject items;
    public TextMeshProUGUI potionText;
    public TextMeshProUGUI gelText;
    public TextMeshProUGUI reviveText;
    public TextMeshProUGUI enemyHPTextPrefab;
    private List<TextMeshProUGUI> enemiesHPText = new List<TextMeshProUGUI>();
    public TextMeshProUGUI commandText;
    public TextMeshProUGUI announcerText;
    public GameObject curtains;
    private Animator curtainsAnim;
    
    public bool isPlayerTurn;
    public bool attackMode;
    public bool targetMode;
    public bool targetAllMode;
    public bool cinemaMode;
    public bool itemsMode;
    public bool combatMode;
    public bool runMode;
    public bool battleOver;

    // Initialize
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        StartNewBattle();
        currentCommand = 0;
        commandText.text = commands[currentCommand];
        attackMode = false;
        targetMode = false;
        targetAllMode = false;
        cinemaMode = false;
        itemsMode = false;
        runMode = false;
        curtainsAnim = curtains.GetComponent<Animator>();
        StartPlayerTurn();
    }
    
    // Refresh the battle setup
    private void StartNewBattle()
    {
        announcerText.text = "BATTLE " + wave;
        player.maxCP = 5*(wave);
        player.currentCP = player.maxCP;
        playerHPText.text = "RUFF HP: " + player.currentHP + "/" + player.maxHP;
        playerCPText.text = "CP: " + player.currentCP + "/" + player.maxCP;
        
        SpawnEnemies(wave);
        battleOver = false;
    }

    // Spawns the enemies based on the wave number
    private void SpawnEnemies(int spawn)
    {
        Enemy enemy;
        TextMeshProUGUI enemyHPText;
        Button targetButton;
        
        if (spawn > 3)
        {
            spawn = 3;
        }
        
        // Battle #1 - 1 minion
        // Battle #2 - 2 minions
        // Battle #3 - 2 minions and 1 elites
        // Battle #4 - 3 elites
        // Battle #5 - 2 elites and 1 boss
        // Initialize and store the enemies
        for (int i = 0; spawn > i; i++)
        {
           if (wave == 3 && i == 1)
            {
                enemy = Instantiate(GangsterElitePrefab, GangsterElitePrefab.transform.position + new Vector3(-2.5f * i, 0, 0),
                    GangsterElitePrefab.transform.rotation);
                enemy.name = "Elite G #" + (i + 1);
                enemy.transform.SetAsFirstSibling();
            }
            else if (wave == 5 && i == 1)
            {
                enemy = Instantiate(GangsterBossPrefab, GangsterBossPrefab.transform.position + new Vector3(-2.5f * i, 0, 0),
                    GangsterBossPrefab.transform.rotation);
                enemy.name = "Boss G";
            }else if (wave >= 4)
            {
                enemy = Instantiate(GangsterElitePrefab, GangsterElitePrefab.transform.position + new Vector3(-2.5f * i, 0, 0),
                    GangsterElitePrefab.transform.rotation);
                enemy.name = "Elite G #" + (i + 1);
            }
            else
            {
                enemy = Instantiate(GangsterPrefab, GangsterPrefab.transform.position + new Vector3(-2.5f * i, 0, 0),
                    GangsterPrefab.transform.rotation);
                enemy.name = "Minion G #" + (i + 1);
            }
            enemy.transform.SetAsFirstSibling();
            enemy.originalPos = enemy.transform.position;
            enemies.Add(enemy);

            // Initialize and store the enemy status text
            enemyHPText = Instantiate(enemyHPTextPrefab, GameObject.Find("Canvas").transform);
            enemyHPText.rectTransform.localPosition += new Vector3(0, i*-30, 0);
            enemyHPText.text = enemy.name + ": " + enemy.currentHP + "/" + enemy.maxHP;
            enemyHPText.transform.SetAsFirstSibling();
            enemiesHPText.Add(enemyHPText);
            
            // Initialize and store enemy target button   
            targetButton = Instantiate(targetButtonPrefab, GameObject.Find("Canvas").transform);
            targetButton.GetComponent<RectTransform>().localPosition = enemyHPText.rectTransform.localPosition + new Vector3(-120, 10, 0);
            targetButton.transform.SetAsFirstSibling();
            targetButton.gameObject.SetActive(false);
            targetButtons.Add(targetButton);
        }
    }
    
    // Available status once the player turn starts
    public void StartPlayerTurn()
    {   
        player.GetComponentInChildren<Animator>().enabled = true;
        isPlayerTurn = true;
        combatMode = false;
        currentTarget = 0;
        commandButton.gameObject.SetActive(true);
        leftButton.gameObject.SetActive(true);
        rightButton.gameObject.SetActive(true);
        announcerText.text = "Player's Turn";
    }
    
    // Deactivate all status and starts the enemy turn once the player action is done
    public IEnumerator StartEnemyTurn()
    {
        attackMode = false;
        targetMode = false;
        targetAllMode = false;
        cinemaMode = false;
        itemsMode = false;
        isPlayerTurn = false;
        currentTarget = 0;
        int previousCount;
        int maxCount = enemies.Count;
        
        // Enemies attack in turn
        for (int i = 0; i < maxCount; i++)
        {
            yield return StartCoroutine(enemies[currentTarget].Combat());
            
            // If the enemy HP is reduced to 0, take it out of the scene
            previousCount = enemies.Count;
            yield return StartCoroutine(CheckDownedEnemy());
            currentTarget += 1 - (previousCount - enemies.Count);
            
            if (currentTarget >= enemies.Count)
            {
                break;
            }
        }
        
        // If all enemies are out of the scene at the end of turn, the player wins the battle
        if (enemies.Count == 0)
        {
            yield return StartCoroutine(Victory());
        }
        
        yield return new WaitForSeconds(2.0f);
        StartPlayerTurn();
    }

    // Swap the current command based on the selected direction or control 
    public void ChangeCommand(bool rightArrow)
    {
        if (!targetMode && !targetAllMode)
        {
            if (rightArrow)
            {
                currentCommand = (currentCommand + 1) % commands.Length;
            }
            else
            {
                currentCommand = (currentCommand - 1 + commands.Length) % commands.Length;
            }
            commandText.text = commands[currentCommand];
        }
    }

    // Select the current command and show the next appropriate UI
    public void SelectCommand()
    {
        switch (currentCommand)
        {
            case 0:
                AttackMode();
                break;
            case 1:
                CinemaAnnouncer();
                CinemaMode();
                break;
            case 2:
                ItemAnnouncer();
                ItemMode();
                break;
            case 3:
                RunMode();
                break;
        }
    }
    
    // Navigate through the cinema move list. Max move count is 4.
    public void ChangeCinemaMove(bool down)
    {
        if (down)
        {
            currentCinemaMove = (currentCinemaMove + 1) % 4;
        }
        else
        {
            currentCinemaMove = (currentCinemaMove - 1 + 4) % 4;
        }
        selectCinemaMoveButton.GetComponent<RectTransform>().localPosition =
            new Vector3(-120, 30 - currentCinemaMove * 30, 0);
        CinemaAnnouncer();
    }

    // Navigate through the item list. Max item count is 3.
    public void ChangeItem(bool down)
    {
        if (down)
        {
            currentItem = (currentItem + 1) % 3;
        }
        else
        {
            currentItem = (currentItem - 1 + 3) % 3;
        }
        selectItemButton.GetComponent<RectTransform>().localPosition =
            new Vector3(-120, 30 - currentItem * 30, 0);
        ItemAnnouncer();
    }
    
    // Change the announcer text with the description for current cinema move
    public void CinemaAnnouncer()
    {
        switch (currentCinemaMove)
        {
            case 0:
                announcerText.text = "Choo Choo through all enemies!"; 
                break;
            case 1:
                announcerText.text = "Pray to RNGgesus!";
                break;
            case 2:
                announcerText.text = "God of War bless thou.";
                break;
            case 3:
                announcerText.text = "Look up!";
                break;
        }
    }

    // Change the announcer text with the description for current item
    public void ItemAnnouncer()
    {
        switch (currentItem)
        {
            case 0:
                announcerText.text = "Restore 15 HP"; 
                break;
            case 1:
                announcerText.text = "Restore 10 CP";
                break;
            case 2:
                announcerText.text = "Auto-revive once downed";
                break;
        }
    }
    
    // Change the target selection
    public void ChangeTarget(bool down)
    {
        targetButtons[currentTarget].gameObject.SetActive(false);
        if (down)
        {
            currentTarget = (currentTarget + 1) % targetButtons.Count;
        }
        else
        {
            currentTarget = (currentTarget - 1 + targetButtons.Count) % targetButtons.Count;
        }
        targetButtons[currentTarget].gameObject.SetActive(true);
    }
    
    // Select the current target and begin the player combat
    public void SelectTarget()
    {
        combatMode = true;
        StartCoroutine(player.Combat());
    }

    // Process to deal damage to the target enemy based on the encounter
    public void DamageEnemy(int damage)
    {
        Enemy enemy = enemies[currentTarget];
        enemy.DamageTaken(damage);
        if (enemiesHPText.Count != 0)
        {
            enemiesHPText[currentTarget].text = enemy.name + " " + Math.Max(0, enemy.currentHP) + "/" + enemy.maxHP;
        }
    }

    // Process to deal damage to all targets based on the encounter
    public IEnumerator DamageAllEnemies(int damage)
    {
        for (int i = 0; enemies.Count > i; i++)
        {
            currentTarget = i;
            DamageEnemy(damage);
        }

        currentTarget = 0;
        int previousCount;
        int maxCount = enemies.Count;
        
        // Check if each enemy is downed
        for (int j = 0; maxCount > j; j++)
        {
            previousCount = enemies.Count;
            yield return StartCoroutine(CheckDownedEnemy());
            currentTarget += 1 - (previousCount - enemies.Count);
            
            if (currentTarget >= enemies.Count)
            {
                break;
            }
        }
        yield return null;
    }

    // Process to check if the current enemy is downed to 0 HP
    public IEnumerator CheckDownedEnemy()
    {
        if (enemies[currentTarget].currentHP <= 0)
        {
            yield return StartCoroutine(DefeatEnemy());
        }
    }

    // Process to take out the current enemy out of the scene
    public IEnumerator DefeatEnemy()
    {
        
        yield return StartCoroutine(enemies[currentTarget].Dying());
        
        Destroy(targetButtons[currentTarget].gameObject);
        targetButtons.RemoveAt(currentTarget);
        
        Destroy(enemiesHPText[currentTarget].gameObject);  
        enemiesHPText.RemoveAt(currentTarget);
        
        Destroy(enemies[currentTarget].gameObject);
        enemies.RemoveAt(currentTarget);
        
        if (currentTarget != 0)
        {
            currentTarget -= 1;
        }
        yield return null;
    }

    // Process to deal damage to the player based on the encounter
    public IEnumerator DamagePlayer(int damage)
    {
        player.DamageTaken(damage);
        playerHPText.text = "RUFF HP: " + Math.Max(0, player.currentHP) + "/" + player.maxHP;
        
        // If the player HP is reduced to 0, then the game is over unless Phoenix Down is available
        if (player.currentHP <= 0)
        {
            // Downed process
            yield return StartCoroutine(player.Dying());
            if (player.reviveUsed == true)
            {
                yield return StartCoroutine(GameOver());
            }
            // Revive process
            else
            {
                yield return new WaitForSeconds(2.0f);
                announcerText.text = "ONE MORE CHANCE!";
                yield return StartCoroutine(player.Revive());
            }
        }
        yield return null;
    }

    // Victory screen
    private IEnumerator Victory()
    {
        battleOver = true;
        announcerText.text = "VICTORY";
        yield return new WaitForSeconds(2.0f);
        
        // If the final wave is finished, the player wins and returns to the title screen
        if (wave == 5)
        {
            battleOver = true;
            announcerText.color = Color.yellowNice;
            announcerText.text = "THANKS FOR PLAYING";
            yield return new WaitForSeconds(10f);
            SceneManager.LoadScene("Title");
        }

        // Process to the next battle wave
        wave++; 
        curtainsAnim.Play("Curtains_Down");
        yield return new WaitForSeconds(curtainsAnim.GetCurrentAnimatorStateInfo(0).length/2.0f);
        StartNewBattle();
        yield return null;
    }

    // Game Over screen and return to the title screen
    public IEnumerator GameOver()
    {
        battleOver = true;
        announcerText.color = Color.red;
        announcerText.text = "GAME OVER";
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("Title");
    }
    
    // Available status during the attack selection 
    private void AttackMode()
    {
        attackMode = true;
        commandButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(true);
        TargetMode();
    }
    
    // Available status during the target selection
    public void TargetMode()
    {
        targetMode = true;
        leftButton.gameObject.SetActive(false);
        rightButton.gameObject.SetActive(false);
        upButton.gameObject.SetActive(true);
        downButton.gameObject.SetActive(true);
        cinemaMoves.SetActive(false);
        targetButtons[currentTarget].gameObject.SetActive(true);
    }
    
    // Available status during the target-all selection
    public void TargetAllMode()
    {
        targetAllMode = true;
        leftButton.gameObject.SetActive(false);
        rightButton.gameObject.SetActive(false);
        upButton.gameObject.SetActive(false);
        downButton.gameObject.SetActive(false);
        cinemaMoves.SetActive(false);
        foreach (Button button in targetButtons)
        {
            button.gameObject.SetActive(true);
        }
    }

    // Available status during the cinema move selection
    private void CinemaMode()
    {
        cinemaMode = true;
        commandButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(true);
        leftButton.gameObject.SetActive(false);
        rightButton.gameObject.SetActive(false);
        upButton.gameObject.SetActive(true);
        downButton.gameObject.SetActive(true);
        cinemaMoves.SetActive(true);
    }
    
    // Available status during the item selection
    private void ItemMode()
    {
        itemsMode = true;
        commandButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(true);
        leftButton.gameObject.SetActive(false);
        rightButton.gameObject.SetActive(false);
        upButton.gameObject.SetActive(true);
        downButton.gameObject.SetActive(true);
        items.SetActive(true);
    }
    
    // Available status during the run selection
    private void RunMode()
    {
        runMode = true;
        commandButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(true);
        leftButton.gameObject.SetActive(false);
        rightButton.gameObject.SetActive(false);
        runButton.gameObject.SetActive(true);
    }

    // Available options and status once returning to the command list
    public void BackToCommand()
    {
        attackMode = false;
        targetMode = false;
        targetAllMode = false;
        cinemaMode = false;
        itemsMode = false;
        runMode = false;
        commandButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(false);
        leftButton.gameObject.SetActive(true);
        rightButton.gameObject.SetActive(true);
        upButton.gameObject.SetActive(false);
        downButton.gameObject.SetActive(false);
        runButton.gameObject.SetActive(false);
        cinemaMoves.SetActive(false);
        items.SetActive(false);
        foreach (Button targetButton in targetButtons)
        {
            targetButton.gameObject.SetActive(false);
        }

        announcerText.text = "Player's Turn";
    }

    // Turn off all UI objects
    public void TurnOffUI()
    {
        commandButton.gameObject.SetActive(false);
        leftButton.gameObject.SetActive(false);
        rightButton.gameObject.SetActive(false);
        upButton.gameObject.SetActive(false);
        downButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
        runButton.gameObject.SetActive(false);
        cinemaMoves.SetActive(false);
        items.SetActive(false);
        foreach (Button targetButton in targetButtons)
        {
            targetButton.gameObject.SetActive(false);
        }
    }
}
