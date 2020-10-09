using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMotion : MonoBehaviour
{
    public Rigidbody ball;
    public Transform target;
    [Space]
    [Header("Physics variables")]
    public float launchAngle;
    
    [Range(0,100)]
    public float initialVel;
    public float time;

    // Start is called before the first frame update
    void Start()
    {
        ball.useGravity = false;


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ball.useGravity = true;
            LaunchBall();
        }
    }

    private void LaunchBall()
    {
        ball.velocity = CalculateBallTrajectory() ;
    }

    private Vector3 CalculateBallTrajectory()
    {
        Vector3 dir = target.position - ball.position;

        float height = dir.y; // get height difference

        dir.y = 0f;

        float dist = dir.magnitude;

        float gravity = Physics.gravity.magnitude;

        float radianAngle = Mathf.Deg2Rad * launchAngle;

        dir.y = dist * Mathf.Tan(radianAngle); // set dir to the elevation angle.

        dist += height / Mathf.Tan(radianAngle); // Correction for small height differences



        //rearranged the range formula { R = u * u * sin(2 * angle) / g}, to find the velocity during launch
        float velocity = Mathf.Sqrt(dist * gravity / Mathf.Sin(2 * radianAngle));


        return velocity * dir.normalized; // Return a normalized vector.


    }
}


