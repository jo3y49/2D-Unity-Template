using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public abstract class PlayerMovement : MonoBehaviour {
    public static PlayerMovement Instance { get; private set; }
    protected Rigidbody2D rb;
    protected InputActions actions;
    protected Vector2 moveInput;

    public float moveSpeed = 5;
    public float sprintMultiplier = 1.5f;
    protected bool isSprinting = false;

    public DirectionEnum.Direction direction = DirectionEnum.Direction.Right;
    

    protected virtual void Awake() {
        rb = GetComponent<Rigidbody2D>();
        actions = new InputActions();
        Instance = this;
    }

    protected virtual void OnEnable() {
        actions.Player.Enable();

        actions.Player.Move.performed += MoveCharacter;
        actions.Player.Move.canceled += context => StopCharacter();
        actions.Player.Sprint.performed += context => Sprint(true);
        actions.Player.Sprint.canceled += context => Sprint(false);
    }

    protected virtual void OnDisable() {
        actions.Player.Move.performed -= MoveCharacter;
        actions.Player.Move.canceled -= context => StopCharacter();
        actions.Player.Sprint.performed += context => Sprint(true);
        actions.Player.Sprint.canceled += context => Sprint(false);

        StopCharacter();

        actions.Player.Disable();
    }

    protected virtual void MoveCharacter(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>().normalized;

        // deadzone check to prevent joystick drift
        if (Mathf.Abs(moveInput.x) < .06f) moveInput.x = 0;
        if (Mathf.Abs(moveInput.y) < .06f) moveInput.y = 0;
    }

    protected virtual void StopCharacter()
    {   
        isSprinting = false;
        moveInput = Vector2.zero;
    }

    protected virtual void Sprint(bool b)
    {
        isSprinting = b;
    }

    protected virtual void FixedUpdate() {
        Movement();
    }

    protected virtual void Movement()
    {
        float speedToUse = isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;

        rb.velocity = moveInput * speedToUse;
    }

    public virtual void ToggleActive(bool active)
    {
        if (actions != null)
        {
            if (active)
                actions.Player.Enable();
            else 
                actions.Player.Disable();
        }
    }
}