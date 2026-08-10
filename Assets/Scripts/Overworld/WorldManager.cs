using System.Collections.Generic;
using UnityEngine;


public class WorldManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> overworldEnemies;
    [SerializeField] private int beginnerStageIndex = 0;
    [SerializeField] private int beginnerVictoryGoal = 1;
    [SerializeField] private int[] midStageIndexes = {1,2,3};
    [SerializeField] private int midVictoryGoal = 4;
    [SerializeField] private int finalStageIndex;
    [SerializeField] private SceneChanger sceneChanger;
    
    void Start()
    {
        startStage(GameManager.Instance.NumberOfVictory);
        finalStageIndex = overworldEnemies.Count - 1;
    }

    private void startStage(int numberOfVictory)
    {
        if (numberOfVictory < beginnerVictoryGoal)
        {
            GameManager.Instance.StartGame(overworldEnemies.Count);
            SpawnEncounters(new int[]{beginnerStageIndex});
        }
        else if (numberOfVictory < midVictoryGoal)
        {
            SpawnEncounters(midStageIndexes);
        }
        else
        {
            SpawnEncounters(new int[]{finalStageIndex});
        }
    }

    private void SpawnEncounters(int[] encounterIndexes)
    {
        foreach (int encounterIndex in encounterIndexes)
        {
            if (GameManager.Instance.WinFlags[encounterIndex] != true)
            {
                overworldEnemies[encounterIndex].SetActive(true);
                overworldEnemies[encounterIndex].GetComponent<Encounter>().SetUp(sceneChanger, encounterIndex);
            }
        }
    }
}
