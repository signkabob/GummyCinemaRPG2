using UnityEngine;

public class BackToCommandButton : MonoBehaviour
{
    private BattleUI ui;

    public void Setup(BattleUI owner)
    {
        ui = owner;
    }

    public void OnClick()
    {
        ui.OnBack();
    }
}