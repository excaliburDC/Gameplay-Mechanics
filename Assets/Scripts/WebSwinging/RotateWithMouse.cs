using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithMouse : MonoBehaviour
{
    [SerializeField] private float _turnSpeed = 3f;

    private void Update()
    {
        float horizontal = Input.GetAxis("Mouse X");
        transform.Rotate(horizontal * _turnSpeed * Vector3.up, Space.World);
    }
}
