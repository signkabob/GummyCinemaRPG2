using System.Collections.Generic;
using UnityEngine;

public class Encounter : MonoBehaviour
{
    [SerializeField] private List<GameObject> battleEnemies;
    [SerializeField] private SceneChanger sceneChanger;
    [SerializeField] private int encounterID;

    public void SetUp(SceneChanger sceneChanger, int encounterIndex)
    {
        this.sceneChanger = sceneChanger;
        encounterID = encounterIndex;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.SetUpBattleEnemies(battleEnemies, encounterID);
            sceneChanger.InitiateBattle();
        }
    }
}
