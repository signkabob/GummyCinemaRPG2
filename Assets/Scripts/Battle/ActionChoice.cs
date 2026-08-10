using System;
/*
 * Final Project: ActionChoice.cs
 * Name: Ka Bo Cheung
 * Date: 08/10/2026
 * Course: GAME-1377-001
 *
 * Script for the action choice
 * Source: UtsabKDas's Game1377_OOP
 */
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