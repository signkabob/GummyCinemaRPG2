using System.Collections.Generic;
using UnityEngine;
/*
 * Final Project: GameManager.cs
 * Name: Ka Bo Cheung
 * Date: 08/10/2026
 * Course: GAME-1377-001
 *
 * Script for the game manager as a singeton
 */
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Battle Parameters")]
    public GameObject Player;

    public List<GameObject> SpawningEnemies;
    public Dictionary<string, int> Inventory = new Dictionary<string, int>()
    {
        ["Potion"] = 3,
        ["Gel"] = 3,
        ["Phoenix Down"] = 1
    };

    [Header("Overworld Parameters")]
    private int CurrentEncounterID;
    public Dictionary<int, bool> WinFlags;
    public int NumberOfVictory { get; private set; } = 0;
    [SerializeField] private int cinemaPointUpgrade = 5;

    public bool IsPaused { get; private set; } = false;
    [SerializeField] private GameObject pauseMenu;
    
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

    /// <summary>
    /// Start the main game and gather the number of overworld enemies to defeat 
    /// </summary>
    /// <param name="numberOfOverWorldEnemies">Number of overworld enemies</param>
    public void StartMainGame(int numberOfOverWorldEnemies)
    {
        WinFlags = new Dictionary<int, bool>();
        for (int i = 0; i < numberOfOverWorldEnemies; i++)
        {
            WinFlags.Add(i, false);
        }
    }
    
    /// <summary>
    /// Set up specific enemies to spawn in the pre-battle
    /// </summary>
    /// <param name="spawningEnemies">Set of spawning enemies in the battle</param>
    /// <param name="encounterID">Encounter ID for the win flag</param>
    public void SetUpBattleEnemies(List<GameObject> spawningEnemies, int encounterID)
    {
        SpawningEnemies = spawningEnemies;
        CurrentEncounterID = encounterID;
    }

    /// <summary>
    /// Declare the player victory in the battle, set a checkpoint, and give reward
    /// </summary>
    public void Victory()
    {
        WinFlags[CurrentEncounterID] = true;
        Player.GetComponent<Friend>().MaxCinemaPoint +=  cinemaPointUpgrade;
        NumberOfVictory++;
    }
    
    /// <summary>
    /// Display the pause screen and pause
    /// </summary>
    public void Pause()
    {
        IsPaused = !IsPaused;
        if (IsPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        pauseMenu.SetActive(IsPaused);
    }

    /// <summary>
    /// Undisplay the pause screen and pause
    /// </summary>
    public void Unpause()
    {
        if (pauseMenu != null)
        {
            IsPaused = false;
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }
}