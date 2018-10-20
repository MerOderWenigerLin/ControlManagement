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
        correctPosition(getBottomLeft());
        correctPosition(getBottomRight());
    }

    protected void correctPosition(Vector2 position)
    {
        Vector2 heading = getNextPosition(position) - position;
        float distance = heading.magnitude;
        if (distance > 0)
        {
            Vector2 direction = heading / distance;
            RaycastHit2D hit1 = Physics2D.Raycast(position + direction * rayCastOffset, direction, distance);
            if (hit1 && hit1.collider != transform.GetComponent<BoxCollider2D>())
            {
                Velocity = new Vector2();
                if (hit1.distance > 0)
                    transform.position = new Vector2(transform.position.x, transform.position.y) + (direction * hit1.distance);
            }
        }
    }

    protected bool isGrounded()
    {
        return willCollide(getBottomLeft(), Vector2.down) || willCollide(getBottomRight(), Vector2.down);
    }

    protected bool willCollide(Vector2 startPosition, Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(startPosition + direction * rayCastOffset, direction, rayCastOffset);
        if (debug && hit)
            Debug.Log(hit.transform);
        return hit && hit.collider != transform.GetComponent<BoxCollider2D>();
    }

    private Vector2 getBottomLeft()
    {
        float y = _colliderRect.yMin;
        float x = _colliderRect.center.x + (_colliderRect.width / 2 * -1);
        return new Vector2(x, y);
    }

    private Vector2 getBottomRight()
    {
        float y = _colliderRect.yMin;
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
