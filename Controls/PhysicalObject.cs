using Unity2DBasics;
using UnityEngine;

public class PhysicalObject : MonoBehaviour
{
    public bool debug;

    private const float rayCastOffset = 0.025f;
    private Rect _colliderRect;

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
        Debug.Log("is grounded checked");
        _colliderRect = BoxCollider2DHelper.toRect(GetComponent<BoxCollider2D>());
        if (debug)
            DebugHelper.drawRectPoints(_colliderRect, Color.red);
        return isGroundedLeft() || isGroundedRight();
    }

    private bool isGroundedLeft()
    {
        return isGrounded((_colliderRect.width / 2 * -1) + rayCastOffset);
    }

    private bool isGroundedRight()
    {
        return isGrounded(_colliderRect.width / 2 - rayCastOffset);
    }

}
