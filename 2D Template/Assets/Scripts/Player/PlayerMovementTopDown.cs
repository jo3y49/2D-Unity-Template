using UnityEngine.InputSystem;

public class PlayerMovementTopDown : PlayerMovement {
    protected override void MoveCharacter(InputAction.CallbackContext context)
    {
        base.MoveCharacter(context);

        // try making it possible to stop while facing diagonally
        direction = DirectionEnum.ConvertVector2ToDirectionDiagonals(moveInput);
    }
}