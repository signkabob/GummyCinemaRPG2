using System.Collections.Generic;
using UnityEngine;
/*
 * Final Project: WorldManager.cs
 * Name: Ka Bo Cheung
 * Date: 08/10/2026
 * Course: GAME-1377-001
 *
 * Script for the overworld manager
 */
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

    /// <summary>
    /// Start the stage of the game based on the number of player victory
    /// </summary>
    /// <param name="numberOfVictory"></param>
    private void startStage(int numberOfVictory)
    {
        // Start with one weak enemy in the beginner stage
        if (numberOfVictory < beginnerVictoryGoal)
        {
            GameManager.Instance.StartMainGame(overworldEnemies.Count);
            SpawnEncounters(new int[]{beginnerStageIndex});
            
            // For demo purposes
            SpawnEncounters(midStageIndexes);
        }
        // Now more decent enemies show up in the mid-stage 
        else if (numberOfVictory < midVictoryGoal)
        {
            SpawnEncounters(midStageIndexes);
        }
        // Once all the enemies are defeated, the big boss shows up in the final stage.
        else
        {
            SpawnEncounters(new int[]{finalStageIndex});
        }
    }

    /// <summary>
    /// Display specific enemy encounters in the overworld
    /// </summary>
    /// <param name="encounterIndexes">Specific encounter</param>
    private void SpawnEncounters(int[] encounterIndexes)
    {
        foreach (int encounterIndex in encounterIndexes)
        {
            // If the battle from the encounter has not been won yet, the encounter will appear.
            if (GameManager.Instance.WinFlags[encounterIndex] != true)
            {
                overworldEnemies[encounterIndex].SetActive(true);
                overworldEnemies[encounterIndex].GetComponent<Encounter>().SetUp(sceneChanger, encounterIndex);
            }
        }
    }
}
