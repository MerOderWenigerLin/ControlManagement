using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Moveable
{
    private float _oldJumpingVelocity;

    public bool controlIsPressed(Control control)
    {
        return ControlManager.getInstance().controlIsPressed(control);
    }

    private void processMovementInput()
    {
        if (controlIsPressed(Control.MoveLeft))
            moveLeft();
        else if (controlIsPressed(Control.MoveRight))
            moveRight();
        else
            stopMovement();            
    }

    private void processJumpingInput()
    {
        if (controlIsPressed(Control.Jump))
            startJumping();
        else if (Velocity.y <= 0)
            stopJumping();
    }

    protected override void processMovement()
    {
        processMovementInput();
        processJumpingInput();
    }
    
}
