using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapRotation : MonoBehaviour
{
    public float rotSpeed;

    private void OnMouseDrag()
    {
        float xRot = Input.GetAxis("Mouse X") * rotSpeed;
        float yRot = Input.GetAxis("Mouse Y") * rotSpeed;

        transform.Rotate(Vector3.down, xRot);
        transform.Rotate(Vector3.right, yRot);
    }
}
