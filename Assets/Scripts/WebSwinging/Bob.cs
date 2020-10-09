using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Bob
{
    public Vector3 velocity;
    public float gravity = 20f;
    public float drag;
    public float maxSpeed;

    //for control over gravity, we can use it for sideways or reverse gravity
    public Vector3 gravityDirection = new Vector3(0f, 1f, 0f);

    private Vector3 dampingDir;

    public void ApplyGravity()
    {
        velocity -= gravityDirection * gravity * Time.deltaTime;
    }

    public void ApplyDamping()
    {
        dampingDir = -velocity;
        dampingDir *= drag;
        velocity += dampingDir;
    }

    public void ClampMaxSpeed()
    {
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
    }
}
