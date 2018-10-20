using Unity2DBasics;
using UnityEngine;

public class Physical : MonoBehaviour
{
    public bool debug;
    public float mass = 1;

    private const float rayCastOffset = 0.025f;
    private Rect _colliderRect;
    private Rigidbody2D _rigidbody;
    protected Rigidbody2D Body { get { return _rigidbody; } }
    protected Vector2 Velocity { get { return Body.velocity; } set { Body.velocity = value; } }

    protected virtual void Update()
    {
        _colliderRect = BoxCollider2DHelper.toRect(GetComponent<BoxCollider2D>());
        processPhysics();
    }

    protected virtual void Start()
    {
        initializeComponents();
        Body.freezeRotation = true;
    }

    protected virtual void initializeComponents()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        if (_rigidbody == null)
        {
            _rigidbody = gameObject.AddComponent<Rigidbody2D>();
            _rigidbody.gravityScale = 3;
            _rigidbody.mass = this.mass;
        }
    }

    protected virtual void processPhysics()
    {
        Vector2 nextBottomLeft = getNextPosition(getBottomLeft());
        Vector2 nextBottomRight = getNextPosition(getBottomRight());
        RaycastHit2D hit1 = Physics2D.Raycast(getBottomLeft(), getNextPosition(getBottomLeft()));
        //DebugHelper.drawPoint(predictedPosition, Color.cyan);
        if (debug)
        {
            Vector2 bottomLeft = getBottomLeft();
            Vector2 bottomRight = getBottomRight();
            DebugHelper.drawPoint(bottomLeft, Color.magenta);
            DebugHelper.drawPoint(bottomRight, Color.magenta);
            DebugHelper.drawPoint(nextBottomLeft, Color.yellow);
            DebugHelper.drawPoint(nextBottomRight, Color.yellow);
            DebugHelper.drawRectPoints(_colliderRect, Color.red);
        }
    }

    protected bool isGrounded()
    {
        return isGrounded(getBottomLeft()) || isGrounded(getBottomRight());
    }

    private bool isGrounded(Vector2 rayCastOrigin)
    {
        RaycastHit2D hit = Physics2D.Raycast(rayCastOrigin, Vector2.down, rayCastOffset);
        if (debug && hit)
            Debug.Log(hit.transform);
        return hit && hit.collider != transform.GetComponent<BoxCollider2D>();
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

    private Vector2 getNextPosition(Vector2 startPosition)
    {
        return new Vector2(startPosition.x + Velocity.x * Time.deltaTime, startPosition.y + Velocity.y * Time.deltaTime);
    }

    private Vector2 getNextPosition(float x, float y)
    {
        return getNextPosition(new Vector2(x, y));
    }
}
