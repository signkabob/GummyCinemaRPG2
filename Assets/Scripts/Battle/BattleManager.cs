using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Final Project: BattleManager.cs
 * Name: Ka Bo Cheung
 * Date: 08/10/2026
 * Course: GAME-1377-001
 *
 * Script for the battle manager; 
 * Source: UtsabKDas's Game1377_OOP
 */
public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }
    
    [Header("Battle Members")]
    public Character Player;
    public List<Character> Party = new List<Character>();
    public List<Character> Enemies = new List<Character>();
    
    [Header("Positioning")]
    [SerializeField] private float enemyPositionOffset = -2.5f; 
    
    [Header("Pacing")]
    public float actionDelay = 0.7f;
    public float roundDelay = 0.4f;
    
    public BattleStage BattleStage { get; private set; }

    private int roundNumber;
    private bool battleOver;
    
    [SerializeField] SceneChanger sceneChanger;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (sceneChanger == null)
        {
            sceneChanger =  GameObject.Find("SceneChanger").GetComponent<SceneChanger>();
        }
        SpawnPlayer();
        SpawnEnemies();
        BattleStage = new BattleStage(Party, Enemies);
        InjectBattleStage();
        BattleEvents.RaiseBattleStarted();
        StartRound();
    }
    
    /// <summary>
    /// Spawn the player in the battle
    /// </summary>
    public void SpawnPlayer()
    {
        Player = Instantiate(GameManager.Instance.Player).GetComponent<Character>();
        Party.Add(Player);
    }

    /// <summary>
    /// Spawn the set of enemies in the battle
    /// </summary>
    public void SpawnEnemies()
    {
        List<GameObject> spawningEnemies = GameManager.Instance.SpawningEnemies;
        for (int i = 0; i < spawningEnemies.Count; i++)
        {
            GameObject enemy = spawningEnemies[i];
            Character spawnedEnemy = Instantiate(enemy, 
                enemy.transform.position + new Vector3(enemyPositionOffset* i, 0, 0), 
                enemy.transform.rotation).GetComponent<Character>();
            spawnedEnemy.transform.SetAsFirstSibling();
            Enemies.Add(spawnedEnemy);
        }
    }

    /// <summary>
    /// Initialize all characters on the battle stage
    /// </summary>
    private void InjectBattleStage()
    {
        List<Character> allCharactersOnStage = BattleStage.GetAllLivingMembers();
        for (int i = 0; i < allCharactersOnStage.Count; i++)
        {
            allCharactersOnStage[i].Initialize(BattleStage);
        }
    }
    
    /// <summary>
    /// Start the new round
    /// </summary>
    private void StartRound()
    {
        if (battleOver)
        {
            return;
        }

        roundNumber = roundNumber + 1;
        BattleEvents.RaiseRoundStarted(roundNumber);
        BattleEvents.RaiseFriendActionSelectionStarted(Player);
    }

    /// <summary>
    /// Submit the player action and target choice
    /// </summary>
    /// <param name="choice"></param>
    /// <param name="target"></param>
    public void SubmitPlayerActionChoice(ActionChoice choice, Character target)
    {
        Player.PlanAction(choice, target);
        BattleEvents.RaiseFriendActionSelectionDone();
        StartCoroutine(ResolveTurn());
    }
    
    /// <summary>
    /// Initiate the planned actions and resolve the turn
    /// </summary>
    /// <returns></returns>
    private IEnumerator ResolveTurn()
    {
        List<Character> livingEnemies = BattleStage.GetLivingMembers(Team.Enemy);
        for (int i = 0; i < livingEnemies.Count; i++)
        {
            Enemy enemy = (Enemy)livingEnemies[i];
            ActionChoice choice = enemy.ChooseRandomAction();
            enemy.PlanAction(choice, PickTargetFor(enemy, choice));
        }

        List<Character> order = BuildTurnOrder();
        for (int i = 0; i < order.Count; i++)
        {
            if (!order[i].IsAlive)
            {
                continue;
            }

            order[i].RaiseEndOfTurn();
            yield return new WaitForSeconds(actionDelay);
            if (CheckBattleOver())
            {
                break;
            }
        }

        if (!battleOver)
        {
            yield return new WaitForSeconds(roundDelay);
            StartRound();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="actor"></param>
    /// <param name="choice"></param>
    /// <returns></returns>
    private Character PickTargetFor(Character actor, ActionChoice choice)
    {
        if (choice.Targeting == TargetKind.SingleEnemy)
        {
            return BattleStage.GetRandomLivingOpponent(actor);
        }
        return null;
    }
    
    /// <summary>
    /// Get the turn order for each living battling members; Player always goes first.
    /// </summary>
    /// <returns></returns>
    private List<Character> BuildTurnOrder()
    {
        List<Character> order = BattleStage.GetAllLivingMembers();
        return order;
    }
    
    /// <summary>
    /// Check if the battle is over
    /// </summary>
    /// <returns></returns>
    public bool CheckBattleOver()
    {
        if (battleOver)
        {
            return true;
        }
        if (!BattleStage.HasLivingMembers(Team.Party))
        {
            battleOver = true;
            BattleEvents.RaiseBattleEnded(Team.Enemy);
            return true;
        }
        if (!BattleStage.HasLivingMembers(Team.Enemy))
        {
            battleOver = true;
            BattleEvents.RaiseBattleEnded(Team.Party);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Escape from the battle
    /// </summary>
    public void EscapeBattle()
    {
        sceneChanger.GoToOverworld();
    }
}
