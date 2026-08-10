using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/*
 * Final Project: BattleUI.cs
 * Name: Ka Bo Cheung
 * Date: 08/10/2026
 * Course: GAME-1377-001
 *
 * Script for the battle UI during every moment of the battle
 * Source: UtsabKas's GAME1377_OOP
 */
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

    /// <summary>
    /// Display the available options for the player
    /// </summary>
    /// <param name="round"></param>
    private void OnRoundStarted(int round)
    {
        Debug.Log("Round #"  + round);
        commandButton.gameObject.SetActive(true);
        leftButton.gameObject.SetActive(true);
        rightButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// Show the available actions for the friend
    /// </summary>
    /// <param name="friend"></param>
    private void OnFriendSelectionStarted(Character friend)
    {
        player = friend;
        ShowActions();
    }

    /// <summary>
    /// Show the available actions
    /// </summary>
    private void ShowActions()
    {
        
    }

    /// <summary>
    /// Submit the chosen action
    /// </summary>
    /// <param name="index"></param>
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
    
    /// <summary>
    /// Submit the player choice
    /// </summary>
    /// <param name="choice"></param>
    /// <param name="target"></param>
    private void Submit(ActionChoice choice, Character target)
    {
        ClearButtons();
        
        BattleManager.Instance.SubmitPlayerActionChoice(choice, target);
    }

    /// <summary>
    /// Do something when the friend is finished with selection
    /// </summary>
    private void OnFriendSelectionDone()
    {
        ClearButtons();
    }

    /// <summary>
    ///  Do something when the battle ends
    /// </summary>
    /// <param name="winner"></param>
    private void OnBattleEnded(Team winner)
    {
        ClearButtons();
    }
    
    /// <summary>
    /// Show specific options based on selected command
    /// </summary>
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

    /// <summary>
    /// Change the command option
    /// </summary>
    /// <param name="rightArrow"></param>
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

    /// <summary>
    ///  Change the choice based on the selected command
    /// </summary>
    /// <param name="downArrow"></param>
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
    
    /// <summary>
    /// Submit the choice on choosing target
    /// </summary>
    /// <param name="target"></param>
    public void OnTargetChosen(Character target)
    {
        Submit(pendingChoice, target);
    }
    
    /// <summary>
    /// Escape from the battle on confirmation
    /// </summary>
    public void OnRunConfirmation()
    {
        BattleManager.Instance.EscapeBattle();
    }

    /// <summary>
    /// Display the player health and cinema point text
    /// </summary>
    /// <param name="player"></param>
    private void ShowPlayerStatus(Friend player)
    {
        playerHealthPointsText.text = "RUFF HP: " + player.CurrentHealth + "/" + player.MaxHealth;
        playerCinemaPointsText.text = "CP: " + player.CurrentCinemaPoint + "/" + player.MaxCinemaPoint;
    }

    /// <summary>
    /// Display the enemy health text and instantiate its target button 
    /// </summary>
    /// <param name="enemies"></param>
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
                enemyHealthText.rectTransform.localPosition + new Vector3(columnOffset, 10, 0);
            targetButton.transform.SetAsFirstSibling();
            targetButton.Setup(this, enemy);
            targetButton.gameObject.SetActive(false);
            targetButtons.Add(targetButton);
        }
    }
    
    /// <summary>
    /// Show a list of available targets
    /// </summary>
    /// <param name="targets"></param>
    private void ShowTargets(List<Character> targets)
    {
        upButton.gameObject.SetActive(true);
        downButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(true);
        foreach (TargetButton targetButton in targetButtons)
        {
            targetButton.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Show a list of cinema moves
    /// </summary>
    private void ShowCinemaMoves()
    {
        backButton.gameObject.SetActive(true);
        upButton.gameObject.SetActive(true);
        downButton.gameObject.SetActive(true);
        cinemaMoves.SetActive(true);
    }
    
    /// <summary>
    /// Show a list of items
    /// </summary>
    private void ShowItems()
    {
        backButton.gameObject.SetActive(true);
        upButton.gameObject.SetActive(true);
        downButton.gameObject.SetActive(true);
        items.SetActive(true);
    }
    
    /// <summary>
    /// Show run confirmation
    /// </summary>
    private void ShowRunConfirmation()
    {
        backButton.gameObject.SetActive(true);
        runButton.gameObject.SetActive(true);
    }
    
    /// <summary>
    /// Back to command options
    /// </summary>
    public void OnBackToCommands()
    {
        ClearButtons();
        commandButton.gameObject.SetActive(true);
        leftButton.gameObject.SetActive(true);
        rightButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// Clear all player command options
    /// </summary>
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