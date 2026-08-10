using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject Player;
    public List<GameObject> SpawningEnemies = new List<GameObject>();
    public Dictionary<string, int> Inventory = new Dictionary<string, int>()
    {
        ["Potion"] = 3,
        ["Gel"] = 3,
        ["Phoenix Down"] = 1
    };
    
    [SerializeField] private int cinemaPointUpgrade;

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

    private void LevelUp()
    {
        Player.GetComponent<Ruff>().MaxCinemaPoint +=  cinemaPointUpgrade;
    }
}