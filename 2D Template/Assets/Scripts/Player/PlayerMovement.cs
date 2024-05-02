using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerMovement : MonoBehaviour {
    public static PlayerMovement Instance { get; private set; }
    private Rigidbody2D rb;
    private InputActions actions;
    private Vector2 moveInput;
    private bool isSprinting;
    public DirectionEnum.Direction direction = DirectionEnum.Direction.Down;
    public float moveSpeed = 5;
    public float sprintMultiplier = 1.5f;

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
    }

    private void OnDisable() {
        actions.Player.Move.performed -= MoveCharacter;
        actions.Player.Move.canceled -= context => StopCharacter();
        actions.Player.Sprint.performed += context => Sprint(true);
        actions.Player.Sprint.canceled += context => Sprint(false);

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

        // try making it possible to stop while facing diagonally
        direction = DirectionEnum.ConvertVector2ToDirectionDiagonals(moveInput);
    }

    private void Sprint(bool b)
    {
        isSprinting = b;
    }

    private void FixedUpdate() {
        Movement();
    }

    private void Movement()
    {
        float speedToUse = isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;

        rb.velocity = moveInput * speedToUse;
    }
    private void StopCharacter()
    {
        moveInput = Vector2.zero;
        rb.velocity = moveInput;
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