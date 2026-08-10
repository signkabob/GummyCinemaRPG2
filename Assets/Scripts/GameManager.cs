using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Battle Parameters")]
    public GameObject Player;
    public List<GameObject> SpawningEnemies = new List<GameObject>();
    public Dictionary<string, int> Inventory = new Dictionary<string, int>()
    {
        ["Potion"] = 3,
        ["Gel"] = 3,
        ["Phoenix Down"] = 1
    };

    [Header("Overworld Parameters")]
    private int CurrentEncounterID;
    public Dictionary<int, bool> WinFlags;
    public int NumberOfVictory = 0;
    
    [SerializeField] private int cinemaPointUpgrade = 5;
    
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame(int numberOfOverWorldEnemies)
    {
        WinFlags = new Dictionary<int, bool>();
        for (int i = 0; i < numberOfOverWorldEnemies; i++)
        {
            WinFlags.Add(i, false);
        }
    }
    
    public void SetUpBattleEnemies(List<GameObject> spawningEnemies, int encounterID)
    {
        SpawningEnemies = spawningEnemies;
        CurrentEncounterID = encounterID;
    }

    public void Victory()
    {
        WinFlags[CurrentEncounterID] = true;
        Player.GetComponent<Friend>().MaxCinemaPoint +=  cinemaPointUpgrade;
        NumberOfVictory++;
    }
}