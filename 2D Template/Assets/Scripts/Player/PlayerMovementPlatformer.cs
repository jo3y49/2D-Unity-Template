using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerMovementPlatformer : MonoBehaviour {
    public static PlayerMovementPlatformer Instance { get; private set; }
    private Rigidbody2D rb;
    private InputActions actions;
    private Vector2 moveInput;
    private bool isSprinting = false;
    private bool isJumping = false;
    private bool canJump = true;
    public DirectionEnum.Direction direction = DirectionEnum.Direction.Down;
    public float moveSpeed = 5;
    public float sprintMultiplier = 1.5f;
    public float jumpForce = 10;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        actions = new InputActions();
        Instance = this;
    }

    private void OnEnable() {
        actions.Player.Enable();

        actions.Player.Move.performed += MoveCharacter;
        actions.Player.Move.canceled += context => StopCharacter();
        actions.Player.Sprint.performed += context => isSprinting = true;
        actions.Player.Sprint.canceled += context => isSprinting = false;
        actions.Player.Jump.performed += context => ApplyJump();
    }

    private void OnDisable() {
        actions.Player.Move.performed -= MoveCharacter;
        actions.Player.Move.canceled -= context => StopCharacter();
        actions.Player.Sprint.performed -= context => isSprinting = true;
        actions.Player.Sprint.canceled -= context => isSprinting = false;
        actions.Player.Jump.performed -= context => ApplyJump();

        StopCharacter();
        isSprinting = false;

        actions.Player.Disable();
    }

    private void MoveCharacter(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>().normalized;

        // try making it possible to stop while facing diagonally
        direction = DirectionEnum.ConvertVector2ToDirectionDiagonals(moveInput);
    }

    private void FixedUpdate() {
        Movement();
    }

    private void Movement()
    {
        float speedToUse = isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;

        float moveX = moveInput.x * speedToUse;
        float moveY = rb.velocity.y;
        rb.velocity = new Vector2(moveX, moveY);
    }
    private void StopCharacter()
    {
        moveInput = Vector2.zero;
        rb.velocity = moveInput;
    }

    public void ApplyJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    public void TogglePause(bool pause)
    {
        if (actions != null)
        {
            if (pause)
                actions.Player.Disable();
            else 
                actions.Player.Enable();
        }
    }
}