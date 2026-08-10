using UnityEngine;
using UnityEngine.InputSystem;
/*
 * Final Project: CameraController.cs
 * Name: Ka Bo Cheung
 * Date: 08/10/2026
 * Course: GAME-1377-001
 *
 * Script for the overworld camera 
 * Source: UtsabKDas's Game1377_AI_Practice
 */
public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float verticalSpeed = 10f;      
    [SerializeField] private float rotationSensitivity = 1f;
    [SerializeField] private float minPitch = 20f;
    [SerializeField] private float maxPitch = 80f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 50f;
    [SerializeField] private float minHeight = 0f;
    [SerializeField] private float maxHeight = 50f;

    public InputSystem_Actions inputSystemActions;
    private InputSystem_Actions.PlayerActions playerActions;

    private float yaw;
    private float pitch;
    private float distance;
    private bool isRotating;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float elevateInput;

    private Vector3 pivotPosition;

    private void Awake()
    {
        inputSystemActions = new InputSystem_Actions();
        playerActions = inputSystemActions.Player;
    }

    private void OnEnable()
    {
        playerActions.Move.performed += HandleMoveInput;
        playerActions.Move.canceled += HandleMoveCanceled;
        playerActions.Look.performed += HandleLookInput;
        playerActions.Look.canceled += HandleLookCanceled;
        playerActions.Elevate.performed += HandleElevateInput;
        playerActions.Elevate.canceled += HandleElevateCanceled;
        playerActions.RotateHold.performed += HandleRotateHoldStarted;
        playerActions.RotateHold.canceled += HandleRotateHoldCanceled;
        playerActions.Enable();
    }

    private void OnDisable()
    {
        playerActions.Move.performed -= HandleMoveInput;
        playerActions.Move.canceled -= HandleMoveCanceled;
        playerActions.Look.performed -= HandleLookInput;
        playerActions.Look.canceled -= HandleLookCanceled;
        playerActions.Elevate.performed -= HandleElevateInput;
        playerActions.Elevate.canceled -= HandleElevateCanceled;
        playerActions.RotateHold.performed -= HandleRotateHoldStarted;
        playerActions.RotateHold.canceled -= HandleRotateHoldCanceled;
        playerActions.Disable();
    }

    private void Start()
    {
        yaw = 45f;
        pitch = 45f;
        distance = 10f;

        pivotPosition = transform.position;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsPaused)
        {
            UpdateCameraRotation();
        }
    }
    
    private void LateUpdate()
    {
        UpdateCameraPosition();
    }

    private void HandleMoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void HandleMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    private void HandleLookInput(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void HandleLookCanceled(InputAction.CallbackContext context)
    {
        lookInput = Vector2.zero;
    }

    private void HandleElevateInput(InputAction.CallbackContext context)
    {
        elevateInput = context.ReadValue<float>();
    }

    private void HandleElevateCanceled(InputAction.CallbackContext context)
    {
        elevateInput = 0f;
    }
    
    /// <summary>
    /// Move the camera vertically or horizontally
    /// </summary>
    private void HandleCameraMovement()
    {
        Vector3 moveDirection = transform.forward * moveInput.y + transform.right * moveInput.x;
        moveDirection.y = 0f;
        pivotPosition += moveDirection.normalized * moveSpeed * Time.deltaTime;

        pivotPosition.y += elevateInput * verticalSpeed * Time.deltaTime;
        pivotPosition.y = Mathf.Clamp(pivotPosition.y, minHeight, maxHeight);
    }

    /// <summary>
    /// Zoom the camera in or out
    /// </summary>
    private void HandleCameraZoom()
    {
        float scrollInput = Mouse.current.scroll.ReadValue().y;

        if (scrollInput != 0f)
        {
            distance -= scrollInput * zoomSpeed * Time.deltaTime;
            distance = Mathf.Clamp(distance, minZoom, maxZoom);
        }
    }

    private void HandleRotateHoldStarted(InputAction.CallbackContext context)
    {
        isRotating = true;
    }
    
    private void HandleRotateHoldCanceled(InputAction.CallbackContext context)
    {
        isRotating = false;
    }

    /// <summary>
    /// Update the camera rotation
    /// </summary>
    private void UpdateCameraRotation()
    {
        if (isRotating)
        {
            yaw += lookInput.x * rotationSensitivity;
            pitch -= lookInput.y * rotationSensitivity;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }
    }

    /// <summary>
    /// Update the camera position to focus and rotate around the player
    /// </summary>
    private void UpdateCameraPosition()
    {
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 offset = rotation * new Vector3(0f, 0f, -distance);
        transform.position = target.position + offset;
        transform.LookAt(target);
    }
}