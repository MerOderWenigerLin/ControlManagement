using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : PhysicalObject
{
    private float _oldJumpingVelocity;

    public bool controlIsPressed(Control control)
    {
        return ControlManager.getInstance().controlIsPressed(control);
    }

    private void checkMovementInput()
    {
        if (controlIsPressed(Control.MoveLeft))
            moveLeft();
        else if (controlIsPressed(Control.MoveRight))
            moveRight();
        else
            stopMovement();            
    }

    private void checkJumpingInput()
    {
        if (controlIsPressed(Control.Jump))
            startJumping();
        else if (Body.velocity.y <= 0)
            stopJumping();
    }

    private void checkInputs()
    {
        checkMovementInput();
        checkJumpingInput();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    public void Update()
    {
        processPhysics();
        checkInputs();
        if (transform.position.y < 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        Body.freezeRotation = true;
    }
}
