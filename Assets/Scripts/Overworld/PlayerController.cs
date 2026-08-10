using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

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
        playerActions.Enable();
    }

    private void OnDisable()
    {
        playerActions.Click.performed -= HandleClickInput;
        playerActions.Disable();
    }

    private void HandleClickInput(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            targetPosition = hit.point;
            navMeshAgent.SetDestination(targetPosition);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}