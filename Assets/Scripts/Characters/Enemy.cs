using UnityEngine;

public abstract class Enemy : Character
{
    protected override void Awake()
    {
        Team = Team.Enemy;
        base.Awake();
    }

    public ActionChoice ChooseRandomAction()
    {
        return possibleActions[Random.Range(0, possibleActions.Count)];
    }
}
