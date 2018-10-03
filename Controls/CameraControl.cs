using Unity2DBasics;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform playerObject;
    public BoxCollider2D cameraBounds;
    public float followSpeed = 4;    

	// Update is called once per frame
	void Update()
    {
        // Correction for the X position of the Camera
        Vector3 cameraPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        if (playerObject != null)
        {
            cameraPosition.x = playerObject.position.x;
            cameraPosition.y = playerObject.position.y;
        }

        Rect colliderHelper = BoxCollider2DHelper.toRect(cameraBounds); ;

        Camera cam = Camera.main;
        float cameraHeight = 2 * cam.orthographicSize;
        float cameraWidth = cameraHeight * cam.aspect;
        float cameraLeft = cameraPosition.x - cameraWidth / 2;
        float cameraRight = cameraPosition.x + cameraWidth / 2;
        float cameraTop = cameraPosition.y + cameraHeight / 2;
        float cameraBottom = cameraPosition.y- cameraHeight / 2;


        if (cameraLeft < colliderHelper.xMin)
            cameraPosition.x = colliderHelper.xMin + cameraWidth / 2;
        else if (cameraRight > colliderHelper.xMax)
            cameraPosition.x = colliderHelper.xMax - cameraWidth / 2;

        if (cameraTop > colliderHelper.yMin)
            cameraPosition.y = colliderHelper.yMin - cameraHeight / 2;
        else if (cameraBottom < colliderHelper.yMax)
            cameraPosition.y = colliderHelper.yMax + cameraHeight / 2;

        transform.position = cameraPosition;        
    }

    // Shows the Bounds of the Camera
    void OnDrawGizmos()
    {
        Rect colliderHelper = BoxCollider2DHelper.toRect(cameraBounds);
        // Top-Left corner
        Debug.DrawRay(new Vector3(colliderHelper.xMin, colliderHelper.yMin), Vector2.right, Color.green);
        Debug.DrawRay(new Vector3(colliderHelper.xMin, colliderHelper.yMin), Vector2.down, Color.green);
        // Top-Right corner
        Debug.DrawRay(new Vector3(colliderHelper.xMax, colliderHelper.yMin), Vector2.left, Color.green);
        Debug.DrawRay(new Vector3(colliderHelper.xMax, colliderHelper.yMin), Vector2.down, Color.green);
        // Bottom-Left corner
        Debug.DrawRay(new Vector3(colliderHelper.xMin, colliderHelper.yMax), Vector2.right, Color.green);
        Debug.DrawRay(new Vector3(colliderHelper.xMin, colliderHelper.yMax), Vector2.up, Color.green);
        // Bottom-Right corner
        Debug.DrawRay(new Vector3(colliderHelper.xMax, colliderHelper.yMax), Vector2.left, Color.green);
        Debug.DrawRay(new Vector3(colliderHelper.xMax, colliderHelper.yMax), Vector2.up, Color.green);
    }
}
