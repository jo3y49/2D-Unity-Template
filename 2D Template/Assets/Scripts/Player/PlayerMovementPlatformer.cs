using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerMovementPlatformer : MonoBehaviour {
    public static PlayerMovementPlatformer Instance { get; private set; }
    private Rigidbody2D rb;
    private InputActions actions;
    private Vector2 moveInput;

    public float moveSpeed = 5;
    public float sprintMultiplier = 1.5f;
    private bool isSprinting = false;

    public float jumpForce = 10f;
    public float jumpHeight = 2f;
    private bool isGrounded = false;
    private bool canJump = true;

    public float dashSpeed = 20f;
    public float dashDuration = .1f;
    public float dashCooldown = 1f;


    public float groundCheckDistance = 0.1f;
    public Transform leftCheck;
    public Transform middleCheck;
    public Transform rightCheck;
    public LayerMask surfaceLayer;

    private Coroutine dashCoroutine, jumpCoroutine;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        actions = new InputActions();
        Instance = this;
    }

    private void OnEnable() {
        actions.Player.Enable();

        actions.Player.Move.performed += MoveCharacter;
        actions.Player.Move.canceled += context => StopCharacter();
        actions.Player.Sprint.performed += context => Sprint(true);
        actions.Player.Sprint.canceled += context => Sprint(false);
        actions.Player.Jump.performed += context => Jump();
    }

    private void OnDisable() {
        actions.Player.Move.performed -= MoveCharacter;
        actions.Player.Move.canceled -= context => StopCharacter();
        actions.Player.Sprint.performed -= context => Sprint(true);
        actions.Player.Sprint.canceled -= context => Sprint(false);
        actions.Player.Jump.performed -= context => Jump();

        StopCharacter();
        isSprinting = false;

        actions.Player.Disable();
    }

    private void MoveCharacter(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>().normalized;

        // deadzone check to prevent joystick drift
        if (Mathf.Abs(moveInput.x) < .06f) moveInput.x = 0;
        if (Mathf.Abs(moveInput.y) < .06f) moveInput.y = 0;
    }

    private void Sprint(bool b)
    {
        // if (b) Dash();

        isSprinting = b;
    }

    // private void Dash()
    // {
    //     dashCoroutine ??= StartCoroutine(DashCoroutine());
    // }

    public void Jump()
    {
        if (!canJump) return;

        jumpCoroutine ??= StartCoroutine(JumpCoroutine());
    }

    private void FixedUpdate() {
        Movement();
        GroundCheck();
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
        rb.velocity *= Vector2.up;
    }

    private IEnumerator JumpCoroutine()
    {
        canJump = false;
        float initialJumpPosition = transform.position.y;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        while (transform.position.y <= initialJumpPosition + jumpHeight)
        {
            yield return null;
        }

        rb.velocity = new Vector2(rb.velocity.x, 0f);
        jumpCoroutine = null;
    }

    // private IEnumerator DashCoroutine()
    // {
    //     float originalGravityScale = rb.gravityScale;

    //     // Disable gravity for the Rigidbody2D component
    //     rb.gravityScale = 0f;

    //     // Determine dash direction based on the sprite's orientation
    //     Vector2 dashDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    //     // Apply dash force, maintaining the current y velocity
    //     rb.velocity = new Vector2(dashDirection.x * dashSpeed, 0f);
        
    //     yield return new WaitForSeconds(dashDuration);

    //     rb.gravityScale = originalGravityScale;

    //     // Reset velocity to what it was before the dash
    //     rb.velocity = Vector2.zero;

    //     // Wait for the cooldown
    //     yield return new WaitForSeconds(dashCooldown);

    //     dashCoroutine = null;
    // }

    private void GroundCheck()
    {
        bool leftGrounded = Physics2D.Raycast(leftCheck.position, Vector2.down, groundCheckDistance, surfaceLayer);
        bool middleGrounded = Physics2D.Raycast(middleCheck.position, Vector2.down, groundCheckDistance, surfaceLayer);
        bool rightGrounded = Physics2D.Raycast(rightCheck.position, Vector2.down, groundCheckDistance, surfaceLayer);

        isGrounded = leftGrounded || middleGrounded || rightGrounded;

        if (isGrounded && jumpCoroutine == null) 
        {
            canJump = true;
        }

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