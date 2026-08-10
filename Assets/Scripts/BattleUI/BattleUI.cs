using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUI : MonoBehaviour
{
    public Button actionButtonPrefab;
    
    [Header("Buttons")]
    public Button commandButton;
    public Button leftButton;
    public Button rightButton;
    public Button upButton;
    public Button downButton;
    public Button backButton;
    public Button runButton;
    public Button targetButtonPrefab;
    private List<TargetButton> targetButtons = new List<TargetButton>();
    public GameObject cinemaMoves;
    public Button selectCinemaMoveButton;
    public GameObject items;
    public Button selectItemButton;
    
    [Header("Text")]
    public TextMeshProUGUI playerHealthPointsText;
    public TextMeshProUGUI playerCinemaPointsText;
    public TextMeshProUGUI potionText;
    public TextMeshProUGUI gelText;
    public TextMeshProUGUI reviveText;
    public TextMeshProUGUI enemyHealthTextPrefab;
    private List<TextMeshProUGUI> enemiesHealthText = new List<TextMeshProUGUI>();
    public TextMeshProUGUI announcerText;
    
    [Header("Command Button Parameters")]
    [SerializeField] private int commandIndex;
    [SerializeField] private int commandLength;
    [SerializeField] private TextMeshProUGUI commandText;
    
    [Header("Target Button Parameters")]
    [SerializeField] private Transform canvas;
    [SerializeField] private float rowOffset = -30.0f;
    [SerializeField] private float columnOffset = -120.0f;
    
    private List<Button> spawnedButtons = new List<Button>();
    private Character player;
    private ActionChoice pendingChoice;
    
    private void OnEnable()
    {
        BattleEvents.FriendActionSelectionStarted += OnFriendSelectionStarted;
        BattleEvents.FriendActionSelectionDone += OnFriendSelectionDone;
        BattleEvents.RoundStarted += OnRoundStarted;
        BattleEvents.BattleEnded += OnBattleEnded;
    }

    private void Start()
    {
        commandIndex = 0;
        commandLength =  Enum.GetNames(typeof(Command)).Length;
        commandText.text = ((Command) commandIndex).ToString();
        ShowPlayerStatus(BattleManager.Instance.Player.GetComponent<Friend>());
        ShowEnemiesHealth(BattleManager.Instance.Enemies);
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
        Debug.Log("Round #"  + round);
        commandButton.gameObject.SetActive(true);
        leftButton.gameObject.SetActive(true);
        rightButton.gameObject.SetActive(true);
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
    
    public void OnCommandChosen()
    {
        ClearButtons();
        switch ((Command) commandIndex)
        {
            case Command.Attack:
                ShowTargets(BattleManager.Instance.BattleStage.GetLivingMembers(Team.Enemy));
                break;
            case Command.Cinema:
                ShowCinemaMoves();
                break;
            case Command.Items:
                ShowItems();
                break;
            case Command.Run:
                ShowRunConfirmation();
                break;
        }
    }

    public void onChangeCommand(bool rightArrow)
    {
        if (rightArrow)
        {
            commandIndex = (commandIndex + 1) % commandLength;
        }
        else
        {
            commandIndex = (commandIndex - 1 + commandLength) % commandLength;
        }
        commandText.text = ((Command) commandIndex).ToString();
    }

    public void onChangeChoice(bool downArrow)
    {
        switch ((Command) commandIndex)
        {
            case Command.Attack:
                // change target
                break;
            case Command.Cinema:
                // ChangeCinemaMove(downArrow);
                break;
            case Command.Items:
                // ChangeItemsMove(downArrow);
                break;
        }
    }
    
    public void OnTargetChosen(Character target)
    {
        Submit(pendingChoice, target);
    }

    public void OnRunConfirmation()
    {
        BattleManager.Instance.EscapeBattle();
    }

    private void ShowPlayerStatus(Friend player)
    {
        playerHealthPointsText.text = "RUFF HP: " + player.CurrentHealth + "/" + player.MaxHealth;
        playerCinemaPointsText.text = "CP: " + player.CurrentCinemaPoint + "/" + player.MaxCinemaPoint;
    }

    private void ShowEnemiesHealth(List<Character> enemies)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            Character enemy = enemies[i];
            // Initialize and store the enemy status text
            TextMeshProUGUI enemyHealthText = Instantiate(enemyHealthTextPrefab, GameObject.Find("Canvas").transform);
            enemyHealthText.rectTransform.localPosition += new Vector3(0.0f, i * rowOffset, 0.0f);
            enemyHealthText.text = enemy.CharacterName + ": " + enemy.CurrentHealth + "/" + enemy.MaxHealth;
            enemyHealthText.transform.SetAsFirstSibling();
            enemiesHealthText.Add(enemyHealthText);

            // Initialize and store enemy target button   
            TargetButton targetButton = Instantiate(targetButtonPrefab, canvas).GetComponent<TargetButton>();
            targetButton.GetComponent<RectTransform>().localPosition =
                enemyHealthText.rectTransform.localPosition + new Vector3(-120, 10, 0);
            targetButton.transform.SetAsFirstSibling();
            targetButton.Setup(this, enemy);
            targetButton.gameObject.SetActive(false);
            targetButtons.Add(targetButton);
        }
    }
    
    private void ShowTargets(List<Character> targets)
    {
        upButton.gameObject.SetActive(true);
        downButton.gameObject.SetActive(true);
        //targetButtons[currentTarget].gameObject.SetActive(true);
    }

    private void ShowCinemaMoves()
    {
        backButton.gameObject.SetActive(true);
        upButton.gameObject.SetActive(true);
        downButton.gameObject.SetActive(true);
        cinemaMoves.SetActive(true);
    }
    
    private void ShowItems()
    {
        backButton.gameObject.SetActive(true);
        upButton.gameObject.SetActive(true);
        downButton.gameObject.SetActive(true);
        items.SetActive(true);
    }
    
    private void ShowRunConfirmation()
    {
        backButton.gameObject.SetActive(true);
        runButton.gameObject.SetActive(true);
    }
    
    public void OnBackToCommands()
    {
        ClearButtons();
        commandButton.gameObject.SetActive(true);
        leftButton.gameObject.SetActive(true);
        rightButton.gameObject.SetActive(true);
    }

    private void ClearButtons()
    {
        commandButton.gameObject.SetActive(false);
        leftButton.gameObject.SetActive(false);
        rightButton.gameObject.SetActive(false);
        upButton.gameObject.SetActive(false);
        downButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
        runButton.gameObject.SetActive(false);
        cinemaMoves.SetActive(false);
        items.SetActive(false);
        foreach (TargetButton targetButton in targetButtons)
        {
            targetButton.gameObject.SetActive(false);
        }
    }
}