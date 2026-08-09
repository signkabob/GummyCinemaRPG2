using UnityEngine;

public class TargetButton : MonoBehaviour
{
    private BattleUI ui;
    private Character target;

    public void Setup(BattleUI owner, Character targetCharacter)
    {
        ui = owner;
        target = targetCharacter;
    }

    public void OnClick()
    {
        ui.OnTargetChosen(target);
    }
}