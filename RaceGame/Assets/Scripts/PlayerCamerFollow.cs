using UnityEngine;

public class PlayerCameraFollow : MonoBehaviour
{
    public Transform Target;
    public Vector3 Offset = new Vector3(0, 3, -10);
    public float SmoothSpeed = 5f;

    private void LateUpdate()
    {
        Vector3 targetPosition = Target.position + Offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, SmoothSpeed * Time.deltaTime);
    }
}