using Unity2DBasics;
using UnityEngine;

public class PhysicalObject : MonoBehaviour
{
    public bool debug;

    public float moveSpeed = 5F;
    public float jumpStrength = 7F;

    private bool _isJumping;

    private const float rayCastOffset = 0.025f;
    private Rect _colliderRect;
    private Rigidbody2D _rigidbody;
    protected Rigidbody2D Body
    {
        get
        {
            if (_rigidbody == null)
                _rigidbody = gameObject.GetComponent<Rigidbody2D>();
            return _rigidbody;
        }
    }
    protected Vector2 Velocity { get { return Body.velocity; } set { Body.velocity = value; } }

    public bool isGrounded(float posXOffset)
    {
        float rayCastOriginPosY = _colliderRect.yMin - rayCastOffset;
        Vector2 rayCastOrigin = new Vector2(_colliderRect.center.x + posXOffset, rayCastOriginPosY);
        RaycastHit2D hit = Physics2D.Raycast(rayCastOrigin, Vector2.down, rayCastOffset);
        if (debug)
            DebugHelper.drawPoint(rayCastOrigin, Color.magenta);
        if (debug && hit)
            Debug.Log(hit.transform);
        return hit && hit.collider != transform.GetComponent<BoxCollider2D>();
    }

    public bool isGrounded()
    {
        _colliderRect = BoxCollider2DHelper.toRect(GetComponent<BoxCollider2D>());
        if (debug)
            DebugHelper.drawRectPoints(_colliderRect, Color.red);
        return isGroundedLeft() || isGroundedRight();
    }

    public void moveLeft()
    {
        Velocity = new Vector3(moveSpeed * -1, Velocity.y);
    }

    public void moveRight()
    {
        Velocity = new Vector3(moveSpeed, Velocity.y);
    }

    public void stopMovement()
    {
        Velocity = new Vector3(0, Velocity.y);
    }

    public void startJumping()
    {
        if (!_isJumping && isGrounded())
        {
            _isJumping = true;
            Velocity = new Vector3(Velocity.x, jumpStrength, 0);
        }
        if (_isJumping)
            Body.AddForce(Vector2.up * jumpStrength * Time.deltaTime * 50);
    }

    public void stopJumping()
    {
        _isJumping = false;
    }

    private bool isGroundedLeft()
    {
        return isGrounded((_colliderRect.width / 2 * -1) + rayCastOffset);
    }

    private bool isGroundedRight()
    {
        return isGrounded(_colliderRect.width / 2 - rayCastOffset);
    }

    protected virtual void processMovement()
    {

    }

    protected virtual void processPhysics()
    {

    }

    void Update()
    {
        processMovement();
        processPhysics();
    }

    void Start()
    {
        Body.freezeRotation = true;
    }

}
