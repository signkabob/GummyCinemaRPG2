using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUI : MonoBehaviour
{
    public Button actionButtonPrefab;
    
    // Mine
    public Button commandButton;
    public Button leftButton;
    public Button rightButton;
    public Button upButton;
    public Button downButton;
    public Button backButton;
    public Button runButton;
    public Button targetButtonPrefab;
    private List<Button> targetButtons = new List<Button>();
    public Button selectCinemaMoveButton;
    public Button selectItemButton;

    private List<Button> spawnedButtons = new List<Button>();
    private Character player;
    private ActionChoice pendingChoice;
    private int commandIndex;
    
    private void OnEnable()
    {
        BattleEvents.FriendActionSelectionStarted += OnFriendSelectionStarted;
        BattleEvents.FriendActionSelectionDone += OnFriendSelectionDone;
        BattleEvents.RoundStarted += OnRoundStarted;
        BattleEvents.BattleEnded += OnBattleEnded;
    }

    private void OnDisable()
    {
        BattleEvents.FriendActionSelectionStarted -= OnFriendSelectionStarted;
        BattleEvents.FriendActionSelectionDone -= OnFriendSelectionDone;
        BattleEvents.RoundStarted -= OnRoundStarted;
        BattleEvents.BattleEnded -= OnBattleEnded;
    }

    private void OnRoundStarted(int round)
    {
        
    }

    private void OnFriendSelectionStarted(Character friend)
    {
        player = friend;
        ShowActions();
    }

    private void ShowActions()
    {
        
    }

    public void OnActionChosen(int index)
    {
        ActionChoice choice = player.Actions[index];
        if (choice.Targeting == TargetKind.SingleEnemy)
        {
            pendingChoice = choice;
            ShowTargets(BattleManager.Instance.BattleStage.GetLivingMembers(Team.Enemy));
        }
        else
        {
            Submit(choice, null);
        }
    }

    public void OnCommandChosen(int index)
    {
        switch ((Command) index)
        {
            case Command.Attack:
                // Attack
                break;
            case Command.Cinema:
                // Cinema
                break;
            case Command.Items:
                // Cinema
                break;
            case Command.Run:
                // Run
                break;
        }
        Debug.Log("Pressed Command.");
    }

    private void ShowTargets(List<Character> targets)
    {
        
    }

    public void OnTargetChosen(Character target)
    {
        Submit(pendingChoice, target);
    }

    public void OnBack()
    {
        ShowActions();
    }

    private void Submit(ActionChoice choice, Character target)
    {
        ClearButtons();
        
        BattleManager.Instance.SubmitPlayerActionChoice(choice, target);
    }

    private void OnFriendSelectionDone()
    {
        ClearButtons();
    }

    private void OnBattleEnded(Team winner)
    {
        ClearButtons();
    }


    private void ClearButtons()
    {
        for (int i = 0; i < spawnedButtons.Count; i++)
        {
            if (spawnedButtons[i] != null)
            {
                Destroy(spawnedButtons[i].gameObject);
            }
        }
        spawnedButtons.Clear();
    }
}