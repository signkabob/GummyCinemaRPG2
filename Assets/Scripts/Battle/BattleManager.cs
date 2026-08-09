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

    private bool battleOver;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        BattleStage = new BattleStage(GameManager.Instance.player, GameManager.Instance.spawningEnemies, enemyPositionOffset);
    }
}
