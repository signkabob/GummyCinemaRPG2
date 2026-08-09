using System.Collections.Generic;
using UnityEngine;

public class BattleStage
{
    private List<Character> party;
    private List<Character> enemies;

    public BattleStage(GameObject spawningPlayer, List<GameObject> spawningEnemies, float enemyPositionOffset)
    {
        party = new List<Character>();
        party.Add(SpawnPlayer(spawningPlayer));
        
        enemies  = new List<Character>();
        SpawnEnemies(spawningEnemies, enemyPositionOffset);
    }

    public Character SpawnPlayer(GameObject player)
    {
        return GameObject.Instantiate(player).GetComponent<Character>();
    }

    public void SpawnEnemies(List<GameObject> enemies, float enemyPositionOffset)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            GameObject enemy = enemies[i];
            Character spawnedEnemy = GameObject.Instantiate(enemy, 
                            enemy.transform.position + new Vector3(enemyPositionOffset* i, 0, 0), 
                                   enemy.transform.rotation).GetComponent<Character>();
            this.enemies.Add(spawnedEnemy);
        }
    }

    public List<Character> GetLivingMembers(Team team)
    {
        List<Character> source;
        if (team == Team.Party)
        {
            source = party;
        }
        else
        {
            source = enemies;
        }
        List<Character> result = new List<Character>();
        for (int i = 0; i < source.Count; i++)
        {
            if (source[i] != null && source[i].IsAlive)
            {
                result.Add(source[i]);
            }
        }
        return result;
    }

    public List<Character> GetAllLivingMembers()
    {
        List<Character> result = GetLivingMembers(Team.Party);
        List<Character> livingEnemies = GetLivingMembers(Team.Enemy);
        for (int i = 0; i < livingEnemies.Count; i++)
        {
            result.Add(livingEnemies[i]);
        }
        return result;
    }

    public List<Character> GetAllMembers()
    {
        List<Character> result = new List<Character>();
        for (int i = 0; i < party.Count; i++)
        {
            if (party[i] != null)
            {
                result.Add(party[i]);
            }
        }
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null)
            {
                result.Add(enemies[i]);
            }
        }
        return result;
    }

    public List<Character> GetOpponents(Character c)
    {
        if (c.Team == Team.Party)
        {
            return GetLivingMembers(Team.Enemy);
        }
        return GetLivingMembers(Team.Party);
    }

    public Character GetRandomLivingOpponent(Character c)
    {
        List<Character> opponents = GetOpponents(c);
        if (opponents.Count == 0)
        {
            return null;
        }
        return opponents[Random.Range(0, opponents.Count)];
    }

    public bool HasLivingMembers(Team team)
    {
        return GetLivingMembers(team).Count > 0;
    }
}