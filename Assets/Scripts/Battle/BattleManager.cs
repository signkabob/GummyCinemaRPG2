using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }
    
    [Header("Battle Members")]
    public List<Character> party = new List<Character>();
    public List<Character> enemies = new List<Character>();
    
    [Header("Positioning")]
    [SerializeField] private float enemyPositionOffset = -2.5f; 
    
    [Header("Pacing")]
    public float actionDelay = 0.7f;
    public float roundDelay = 0.4f;
    
    public BattleStage BattleStage { get; private set; }

    private int roundNumber;
    private bool battleOver;

    private Character player;
    private int commandIndex;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        
        SpawnPlayer();
        SpawnEnemies();
        BattleStage = new BattleStage(party, enemies);
        InjectBattleStage();
        BattleEvents.RaiseBattleStarted();
        StartRound();
    }
    
    public void SpawnPlayer()
    {
        player = Instantiate(GameManager.Instance.player).GetComponent<Character>();
        party.Add(player);
    }

    public void SpawnEnemies()
    {
        List<GameObject> spawningEnemies = GameManager.Instance.spawningEnemies;
        for (int i = 0; i < spawningEnemies.Count; i++)
        {
            GameObject enemy = spawningEnemies[i];
            Character spawnedEnemy = Instantiate(enemy, 
                enemy.transform.position + new Vector3(enemyPositionOffset* i, 0, 0), 
                enemy.transform.rotation).GetComponent<Character>();
            enemies.Add(spawnedEnemy);
        }
    }

    private void InjectBattleStage()
    {
        List<Character> allCharactersOnStage = BattleStage.GetAllLivingMembers();
        for (int i = 0; i < allCharactersOnStage.Count; i++)
        {
            allCharactersOnStage[i].Initialize(BattleStage);
        }
    }
    
    private void StartRound()
    {
        if (battleOver)
        {
            return;
        }

        roundNumber = roundNumber + 1;
        BattleEvents.RaiseRoundStarted(roundNumber);
        BattleEvents.RaiseFriendActionSelectionStarted(player);
    }

    public void SubmitPlayerActionChoice(ActionChoice choice, Character target)
    {
        player.PlanAction(choice, target);
        BattleEvents.RaiseFriendActionSelectionDone();
        StartCoroutine(ResolveTurn());
    }
    
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

    private Character PickTargetFor(Character actor, ActionChoice choice)
    {
        if (choice.Targeting == TargetKind.SingleEnemy)
        {
            return BattleStage.GetRandomLivingOpponent(actor);
        }
        return null;
    }
    
    private List<Character> BuildTurnOrder()
    {
        List<Character> order = BattleStage.GetAllLivingMembers();
        return order;
    }
    
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
}
