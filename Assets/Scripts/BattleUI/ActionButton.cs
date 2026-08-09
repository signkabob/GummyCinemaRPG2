using UnityEngine;

public class ActionButton : MonoBehaviour
{
    private BattleUI ui;
    private int actionIndex;

    public void Setup(BattleUI owner, int index)
    {
        ui = owner;
        actionIndex = index;
    }

    public void OnClick()
    {
        ui.OnActionChosen(actionIndex);
    }
}