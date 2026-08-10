using UnityEngine;
/*
 * Final Project: Enemy.cs
 * Name: Ka Bo Cheung
 * Date: 08/10/2026
 * Course: GAME-1377-001
 *
 * Script for the enemy class derived from character class
 * Source: UtsabKas's GAME1377_OOP
 */
public abstract class Enemy : Character
{
    protected override void Awake()
    {
        Team = Team.Enemy;
        base.Awake();
    }
    
    /// <summary>
    /// Choose random action
    /// </summary>
    /// <returns></returns>
    public ActionChoice ChooseRandomAction()
    {
        return possibleActions[Random.Range(0, possibleActions.Count)];
    }
}
