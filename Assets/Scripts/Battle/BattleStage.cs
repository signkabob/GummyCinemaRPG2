using System.Collections.Generic;
using UnityEngine;
/*
 * Final Project: BattleStage.cs
 * Name: Ka Bo Cheung
 * Date: 08/10/2026
 * Course: GAME-1377-001
 *
 * Script for the battle stage and the status checker for its battling members
 * Source: UtsabKDas's Game1377_OOP
 */
public class BattleStage
{
    private List<Character> party;
    private List<Character> enemies;

    public BattleStage(List<Character> party, List<Character> enemies)
    {
        this.party = party;
        this.enemies = enemies;
    }

    /// <summary>
    /// Get the living batting memebers from specific team
    /// </summary>
    /// <param name="team"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Get all living battling members on the stage
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Get all battling members on the stage
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Get the opponents of the battling member
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public List<Character> GetOpponents(Character c)
    {
        if (c.Team == Team.Party)
        {
            return GetLivingMembers(Team.Enemy);
        }
        return GetLivingMembers(Team.Party);
    }

    /// <summary>
    /// Get random living opponent of the battling member
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public Character GetRandomLivingOpponent(Character c)
    {
        List<Character> opponents = GetOpponents(c);
        if (opponents.Count == 0)
        {
            return null;
        }
        return opponents[Random.Range(0, opponents.Count)];
    }

    /// <summary>
    /// Check if the team has any living battling member
    /// </summary>
    /// <param name="team"></param>
    /// <returns></returns>
    public bool HasLivingMembers(Team team)
    {
        return GetLivingMembers(team).Count > 0;
    }
}