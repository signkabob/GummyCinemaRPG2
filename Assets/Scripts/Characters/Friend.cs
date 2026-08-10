using System;
using UnityEngine;
/*
 * Final Project: Friend.cs
 * Name: Ka Bo Cheung
 * Date: 08/10/2026
 * Course: GAME-1377-001
 *
 * Script for the friend class derived from character class
 * Source: UtsabKas's GAME1377_OOP
 */
public abstract class Friend : Character
{
    [Header("Friend Base")]
    public int MaxCinemaPoint;

    public event Action CinemaPointChanged;
    
    [SerializeField] private int currentCinemaPoint;
    
    public int CurrentCinemaPoint { get { return currentCinemaPoint; } }
    
    protected override void Awake()
    {
        Team = Team.Party;
        currentCinemaPoint = MaxCinemaPoint;
        base.Awake();
    }
    
    /// <summary>
    /// Restore cinema points from the source
    /// </summary>
    /// <param name="restoreAmount">cinema points</param>
    /// <param name="source">restoring source</param>
    public void RestoreCinemaPoints(int restoreAmount, Character source)
    {
        if (!IsAlive)
        {
            return;
        }
        currentCinemaPoint = Math.Min(currentCinemaPoint + restoreAmount, MaxCinemaPoint);
        BattleEvents.RaiseCinemaRestored(source, this, restoreAmount);
        CinemaPointChanged?.Invoke();
    }
}