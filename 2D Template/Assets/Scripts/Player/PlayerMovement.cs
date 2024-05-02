using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerMovement : MonoBehaviour {
    private Rigidbody2D rb;
    private InputActions actions;
    private Vector2 moveInput;
    private bool isSprinting;

    [HideInInspector]
    public Vector3 playerDirection;
    public float moveSpeed = 5;    
    public float sprintMultiplier = 1.5f;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        actions = new InputActions();
    }

    private void OnEnable() {
        actions.Player.Enable();

        actions.Player.Move.performed += MoveCharacter;
        actions.Player.Move.canceled += context => StopCharacter();
        actions.Player.Sprint.performed += context => isSprinting = true;
        actions.Player.Sprint.canceled += context => isSprinting = false;

        PauseManager.PauseEvent += TogglePause;
    }

    private void OnDisable() {
        actions.Player.Move.performed -= MoveCharacter;
        actions.Player.Move.canceled -= context => StopCharacter();
        actions.Player.Sprint.performed -= context => isSprinting = true;
        actions.Player.Sprint.canceled -= context => isSprinting = false;

        PauseManager.PauseEvent -= TogglePause;

        StopCharacter();
        isSprinting = false;

        actions.Player.Disable();
    }

    private void MoveCharacter(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>().normalized;
        playerDirection = -new Vector3(moveInput.x, 0, moveInput.y).normalized;

        transform.rotation = Quaternion.LookRotation(playerDirection);
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