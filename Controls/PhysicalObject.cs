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

    public bool isGrounded(Vector2 rayCastOrigin)
    {
        RaycastHit2D hit = Physics2D.Raycast(rayCastOrigin, Vector2.down, rayCastOffset);
        if (debug && hit)
            Debug.Log(hit.transform);
        return hit && hit.collider != transform.GetComponent<BoxCollider2D>();
    }

    public bool isGrounded()
    {
        return isGrounded(getBottomLeft()) || isGrounded(getBottomRight());
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

    private Vector2 getBottomLeft()
    {
        float rayCastOriginPosY = _colliderRect.yMin - rayCastOffset;
        Vector2 rayCastOrigin = new Vector2(_colliderRect.center.x + (_colliderRect.width / 2 * -1) + rayCastOffset, rayCastOriginPosY);
        return rayCastOrigin;
    }

    private Vector2 getBottomRight()
    {
        float rayCastOriginPosY = _colliderRect.yMin - rayCastOffset;
        Vector2 rayCastOrigin = new Vector2(_colliderRect.center.x + _colliderRect.width / 2 - rayCastOffset, rayCastOriginPosY);
        return rayCastOrigin;
    }

    protected virtual void processMovement()
    {

    }

    protected virtual void processPhysics()
    {
        Vector2 predictedPosition = new Vector2(_colliderRect.x, _colliderRect.yMin + Velocity.y * Time.deltaTime);
        //DebugHelper.drawPoint(predictedPosition, Color.cyan);
        if (debug)
        {
            Vector2 bottomLeft = getBottomLeft();
            Vector2 bottomRight = getBottomRight();
            DebugHelper.drawPoint(bottomLeft, Color.magenta);
            DebugHelper.drawPoint(bottomRight, Color.magenta);
            DebugHelper.drawRectPoints(_colliderRect, Color.blue);
        }
    }

    void Update()
    {
        _colliderRect = BoxCollider2DHelper.toRect(GetComponent<BoxCollider2D>());
        processMovement();
        processPhysics();
    }

    void Start()
    {
        //Time.timeScale = 0.1F;
        Body.freezeRotation = true;
    }

}
