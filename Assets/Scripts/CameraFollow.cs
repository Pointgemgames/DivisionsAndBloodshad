using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = Vector3.Lerp(transform.position, target.position, smoothSpeed);
            targetPosition.z = -10;

            transform.position = targetPosition;
        }

        else
        {
            target = GameObject.FindGameObjectWithTag("Player")?.transform;
        }
    }
}
