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
        //Time.timeScale = 0.05F;
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
        if (isGrounded())
            return;

        Vector2 bottomLeft = getBottomLeft();
        Vector2 nextBottomLeft = getNextPosition(bottomLeft);

        Vector2 heading = nextBottomLeft - bottomLeft;
        float distance = heading.magnitude;
        Vector2 direction = heading / distance;

        RaycastHit2D hit1 = Physics2D.Raycast(bottomLeft, direction, distance);
        if (hit1 && hit1.collider != transform.GetComponent<BoxCollider2D>())
        {
            Velocity = new Vector2();
            transform.position = new Vector2(transform.position.x, transform.position.y) + (direction * hit1.distance);
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
        float y = _colliderRect.yMin - rayCastOffset;
        float x = _colliderRect.center.x + (_colliderRect.width / 2 * -1);
        return new Vector2(x, y);
    }

    private Vector2 getBottomRight()
    {
        float y = _colliderRect.yMin - rayCastOffset;
        float x = _colliderRect.center.x + _colliderRect.width / 2;
        return new Vector2(x, y);
    }

    private Vector2 getNextPosition(Vector2 startPosition)
    {
        return new Vector2(startPosition.x, startPosition.y) + Velocity * Time.deltaTime;
    }

    private Vector2 getNextPosition(float x, float y)
    {
        return getNextPosition(new Vector2(x, y));
    }
}
