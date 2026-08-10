using System.Collections.Generic;
using UnityEngine;
/*
 * Final Project: Encounter.cs
 * Name: Ka Bo Cheung
 * Date: 08/10/2026
 * Course: GAME-1377-001
 *
 * Script for the encounter and the battle trigger with the setup of specific enemies
 */
public class Encounter : MonoBehaviour
{
    [SerializeField] private List<GameObject> battleEnemies;
    [SerializeField] private SceneChanger sceneChanger;
    [SerializeField] private int encounterID;

    /// <summary>
    /// Assign the scene manager and ID to this encounter
    /// </summary>
    /// <param name="sceneChanger"></param>
    /// <param name="encounterIndex"></param>
    public void SetUp(SceneChanger sceneChanger, int encounterIndex)
    {
        this.sceneChanger = sceneChanger;
        encounterID = encounterIndex;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        // Initiate the battle once collided with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.SetUpBattleEnemies(battleEnemies, encounterID);
            sceneChanger.InitiateBattle();
        }
    }
}
