using System;
public enum TargetKind
{
    SingleEnemy,
    AllEnemies,
    Self
}

public struct ActionChoice
{
    public string Label;
    public TargetKind Targeting;
    public Action Perform;

    public ActionChoice(string label, TargetKind targeting, Action perform)
    {
        Label = label;
        Targeting = targeting;
        Perform = perform;
    }
}