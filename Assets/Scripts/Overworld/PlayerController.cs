using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
/*
 * Final Project: PlayerController.cs
 * Name: Ka Bo Cheung
 * Date: 08/10/2026
 * Course: GAME-1377-001
 *
 * Script for the player controller
 * Source: UtsabKDas's Game1377_AI_Practice
 */
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float stoppingDistance = 0.1f;

    public InputSystem_Actions inputSystemActions;
    private InputSystem_Actions.PlayerActions playerActions;

    private NavMeshAgent navMeshAgent;
    private Vector3 targetPosition;

    private void Awake()
    {
        inputSystemActions = new InputSystem_Actions();
        playerActions = inputSystemActions.Player;

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = stoppingDistance;
        targetPosition = transform.position;
    }

    private void OnEnable()
    {
        playerActions.Click.performed += HandleClickInput;
        playerActions.Pause.performed += HandlePauseInput;
        playerActions.Enable();
    }

    private void OnDisable()
    {
        playerActions.Click.performed -= HandleClickInput;
        playerActions.Pause.performed -= HandlePauseInput;
        playerActions.Disable();
    }

    /// <summary>
    /// Click on the screen to move the player toward that position 
    /// </summary>
    /// <param name="context"></param>
    private void HandleClickInput(InputAction.CallbackContext context)
    {
        if (!GameManager.Instance.IsPaused)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                targetPosition = hit.point;
                navMeshAgent.SetDestination(targetPosition);
            }
        }
    }

    /// <summary>
    /// Press pause key to pause the game
    /// </summary>
    /// <param name="context"></param>
    private void HandlePauseInput(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.IsPaused)
        {
            GameManager.Instance.Unpause();
        }
        else
        {
            GameManager.Instance.Pause();
        }
    }
}