using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWithTrajectoryPath : MonoBehaviour
{
    [Header("Unity Reference Variables")]
    public Camera cam;
    public Rigidbody cannonBall;
    public GameObject projectileHitPoint;
    public Transform shootPoint;
    public Transform cannonHead;
    public LayerMask layer;

    [Space]
    [Header("Physics variables")]
    [Range(0f,360f)]
    public float launchAngle;


    // Start is called before the first frame update
    void Start()
    {
        cannonBall.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        LaunchProjectile();
    }

    private void LaunchProjectile()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);


        RaycastHit hit;

        if (Physics.Raycast(ray, out hit,Mathf.Infinity, layer))
        {
            projectileHitPoint.SetActive(true);
            projectileHitPoint.transform.position = hit.point + Vector3.up * 0.1f;
            //projectileHitPoint.transform.rotation = Quaternion.Euler(hit.point + Vector3.up * 0.1f);

            Vector3 cannonVel = CalculateBallTrajectory(shootPoint.position,hit.point);

            //Vector3 cannonVel = CalculateVelocity(shootPoint.position, hit.point,1f);

            //cannonHead.rotation = Quaternion.LookRotation(cannonVel);

            if (Input.GetMouseButtonDown(0))
            {
                Rigidbody rbObj = Instantiate(cannonBall, shootPoint.position, Quaternion.identity);

                rbObj.useGravity = true;

                rbObj.velocity = cannonVel;
            }
        }
    }

    private Vector3 CalculateBallTrajectory(Vector3 origin, Vector3 targetPos)
    {
        Vector3 dir = targetPos - origin;

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

    Vector3 CalculateVelocity(Vector3 origin, Vector3 target, float time)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXz = distance;
        distanceXz.y = 0f;

        float sY = distance.y;
        float sXz = distanceXz.magnitude;

        float Vxz = sXz * time;
        float Vy = (sY / time) + (0.5f * Mathf.Abs(Physics.gravity.y) * time);

        Vector3 result = distanceXz.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }
}
