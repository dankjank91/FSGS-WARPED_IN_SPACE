using UnityEngine;

public class FaceCenter : MonoBehaviour
{

    private void Update()
    {
        ObjFaceCenter();
    }
    void ObjFaceCenter()
    {
        Vector2 center = Vector2.zero;
        Vector2 objPosition = transform.position;
        Vector2 directionToCenter = center - objPosition;
        float angleToCenter = Mathf.Atan2(directionToCenter.y, directionToCenter.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(45f, 0f, angleToCenter);
    }
}
