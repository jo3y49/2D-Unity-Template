using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementPlatformer : PlayerMovement {
    public float jumpForce = 10f;
    public float jumpHeight = 2f;
    private bool isGrounded = false;
    private bool canJump = true;

    public float dashSpeed = 16f;
    public float dashDuration = .1f;
    public float dashCooldown = 1f;
    private bool isDashing = false;

    private float originalGravityScale;
    public float groundCheckDistance = 0.1f;
    public Transform leftCheck;
    public Transform middleCheck;
    public Transform rightCheck;
    public LayerMask surfaceLayer;

    private Coroutine dashCoroutine, jumpCoroutine;

    protected override void Awake() {
        base.Awake();
        originalGravityScale = rb.gravityScale;
    }

    protected override void OnEnable() {
        base.OnEnable();
        actions.Player.Jump.performed += context => Jump();
    }

    protected override void OnDisable() {
        actions.Player.Move.performed -= MoveCharacter;
        actions.Player.Move.canceled -= context => StopCharacter();
        actions.Player.Sprint.performed -= context => Sprint(true);
        actions.Player.Sprint.canceled -= context => Sprint(false);
        actions.Player.Jump.performed -= context => Jump();

        StopCharacter();

        actions.Player.Disable();
    }

    protected override void MoveCharacter(InputAction.CallbackContext context)
    {
        base.MoveCharacter(context);

        direction = DirectionEnum.ConvertVector2ToDirectionNoDiagonals(moveInput * Vector2.right);
    }

    protected override void Sprint(bool b)
    {
        if (b && !isGrounded) AirDash();

        else base.Sprint(b);
    }

    private void AirDash()
    {
        isSprinting = false;
        dashCoroutine ??= StartCoroutine(DashCoroutine());
    }

    public void Jump()
    {
        if (!canJump) return;

        jumpCoroutine ??= StartCoroutine(JumpCoroutine());
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
        GroundCheck();
    }

    protected override void Movement()
    {
        if (isDashing == true) return;

        float speedToUse = isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;

        float moveX = moveInput.x * speedToUse;
        float moveY = rb.velocity.y;
        rb.velocity = new Vector2(moveX, moveY);
    }

    private IEnumerator JumpCoroutine()
    {
        KillDash();
        canJump = false;
        float initialJumpPosition = transform.position.y;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        while (transform.position.y <= initialJumpPosition + jumpHeight)
        {
            yield return null;
        }

        KillJump();
    }

    private IEnumerator DashCoroutine()
    {
        KillJump();
        isDashing = true;

        // Disable gravity for the Rigidbody2D component
        rb.gravityScale = 0f;

        // Determine dash direction based on the sprite's orientation
        Vector2 dashDirection = DirectionEnum.ConvertDirectionToVector2(direction);

        // Apply dash force, maintaining the current y velocity
        rb.velocity = new Vector2(dashDirection.x * dashSpeed, 0f);
        
        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravityScale;

        isDashing = false;

        // Wait for the cooldown
        yield return new WaitForSeconds(dashCooldown);

        dashCoroutine = null;
    }

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

    private void KillJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        if (jumpCoroutine != null) StopCoroutine(jumpCoroutine);
        jumpCoroutine = null;
        canJump = false;
    }

    private void KillDash()
    {
        rb.gravityScale = originalGravityScale;
        isDashing = false;
    }

    // public void TogglePause(bool pause)
    // {
    //     if (actions != null)
    //     {
    //         if (pause)
    //             actions.Player.Disable();
    //         else 
    //             actions.Player.Enable();
    //     }
    // }
}