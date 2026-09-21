using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;       // Player movement speed
    public float gravity = -9.81f;     // Gravity force applied to player
    public float jumpHeight = 1.5f;    // Jump height in units

    [Header("Look Settings")]
    public Transform cameraTransform;  // Reference to the player's camera
    public float lookSensitivity = 0.5f; // Mouse sensitivity for looking
    public float verticalLookLimit = 90f; // Limit for looking up/down

    [Header("Zoom Settings")]
    public float zoomFOV = 10f;        // Field of view when zoomed in
    public float normalFOV = 60f;      // Default field of view
    public float zoomSpeed = 10f;      // Speed of zoom transition
    private Camera playerCamera;       // Reference to the Camera component

    private CharacterController controller; // Handles collisions & movement
    private Vector2 moveInput;              // Stores WASD/analog stick input
    private Vector2 lookInput;              // Stores mouse/analog look input
    private Vector3 velocity;               // Vertical velocity (gravity/jump)
    private float verticalRotation = 0f;    // Tracks up/down camera rotation
    private bool isZoomed;                  // Whether right-click zoom is currently enabled

    private void Awake()
    {
        // Get the CharacterController attached to the player
        controller = GetComponent<CharacterController>();

        // Get the Camera component from the assigned cameraTransform
        playerCamera = cameraTransform.GetComponent<Camera>();

        // Keep the cursor visible and free so the player can click keypad cubes.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        // Handle player movement (walking, gravity, jumping)
        HandleMovement();

        // Handle player looking (camera rotation)
        HandleLook();

        // Smoothly move toward the active zoom level.
        HandleZoom();
    }

    // Called when movement input (WASD/analog stick) is detected
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // Called when look input (mouse/analog stick) is detected
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    // Handles walking, gravity, and jump physics safely
    private void HandleMovement()
    {
        // Only run if controller exists and is enabled
        if (controller == null || !controller.enabled) return;

        // Calculate movement direction based on input
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        // Apply horizontal movement
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Reset downward velocity when grounded
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Apply gravity continuously
        velocity.y += gravity * Time.deltaTime;

        // Apply vertical movement (gravity/jump)
        controller.Move(velocity * Time.deltaTime);
    }

    // Handles camera rotation (looking around)
    private void HandleLook()
    {
        float mouseX = lookInput.x * lookSensitivity;
        float mouseY = lookInput.y * lookSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);

        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    // Handles jumping when input is performed
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && controller != null && controller.enabled && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    // Handles a generic "press" action (e.g., interact, use, or custom input)
    public void OnPress(InputAction.CallbackContext context)
    {
        if (!context.performed || playerCamera == null || Mouse.current == null)
        {
            return;
        }

        // Cast a ray from the mouse cursor into the scene to find a clicked keypad cube.
        Ray mouseRay = playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(mouseRay, out RaycastHit hit))
        {
            PasswordKey passwordKey = hit.collider.GetComponent<PasswordKey>();
            if (passwordKey != null)
            {
                passwordKey.Press();
            }
        }
    }
    // Toggles zoom when the zoom action (right-click) is pressed.
    public void OnZoom(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isZoomed = !isZoomed;
        }
    }

    // Smoothly zooms toward either the normal or zoomed field of view.
    private void HandleZoom()
    {
        if (playerCamera == null) return;

        float targetFOV = isZoomed ? zoomFOV : normalFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
    }
}
