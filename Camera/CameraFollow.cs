using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public float fixedYPosition = 0f; // y���� ������ų ��

    public bool followTarget = true;

    void LateUpdate()
    {
        if (followTarget)
        {
            Vector3 targetPosition = target.position;
            targetPosition.y = fixedYPosition; // y�� ����
            Vector3 desiredPosition = targetPosition + new Vector3(0, 0, -10);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
