using System;
using UnityEngine;

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