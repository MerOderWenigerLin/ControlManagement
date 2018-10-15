using Unity2DBasics;
using UnityEngine;

public class MoveableObject : PhysicalObject
{
    public float moveSpeed = 5F;
    public float jumpStrength = 7F;

    private bool _isJumping;

    protected override void Update()
    {
        base.Update();
        processMovement();
    }

    protected virtual void processMovement()
    {

    }

    protected void moveLeft()
    {
        Velocity = new Vector3(moveSpeed * -1, Velocity.y);
    }

    protected void moveRight()
    {
        Velocity = new Vector3(moveSpeed, Velocity.y);
    }

    protected void stopMovement()
    {
        Velocity = new Vector3(0, Velocity.y);
    }

    protected void startJumping()
    {
        if (!_isJumping && isGrounded())
        {
            _isJumping = true;
            Velocity = new Vector3(Velocity.x, jumpStrength, 0);
        }
        if (_isJumping)
            Body.AddForce(Vector2.up * jumpStrength * Time.deltaTime * 50);
    }

    protected void stopJumping()
    {
        _isJumping = false;
    }

}
