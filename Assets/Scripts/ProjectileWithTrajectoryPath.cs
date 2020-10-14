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
    public LineRenderer line;
    

    [Space]
    [Header("Physics/normal variables")]
    [Range(0f,360f)]
    public float launchAngle;
    public int lineSegment = 10;

    private float smoothTurnAngle;


    // Start is called before the first frame update
    void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        cannonBall.useGravity = false;
        line.positionCount = lineSegment;
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

            DrawTrajectorypath(cannonVel);

            //float targetAngle = Mathf.Atan2(hit.normal.x, hit.normal.z) * Mathf.Deg2Rad + cam.transform.eulerAngles.y;

            //float angle = Mathf.SmoothDampAngle(cannonHead.eulerAngles.y, targetAngle, ref smoothTurnAngle, 0.1f);

            //cannonHead.rotation = Quaternion.Euler(0f, angle, 0f);

            //Vector3 cannonVel = CalculateVelocity(shootPoint.position, hit.point,1f);

            cannonHead.rotation = Quaternion.LookRotation(cannonVel);

            if (Input.GetMouseButtonDown(0))
            {
                Rigidbody rbObj = Instantiate(cannonBall, shootPoint.position, Quaternion.identity);
                
                rbObj.useGravity = true;

                rbObj.velocity = cannonVel;
            }
        }
    }

    void DrawTrajectorypath(Vector3 cannonVelocity)
    {
        for (int i = 0; i < lineSegment; i++)
        {
            Vector3 linePos = CalculateRenderPath(cannonVelocity, i / (float)lineSegment);
            line.SetPosition(i,linePos);
        }
    }

    private Vector3 CalculateRenderPath(Vector3 v, float time)
    {
        Vector3 Vxz = v;
        Vxz.y = 0f;

        Vector3 result = shootPoint.position + v * time;
        float sY = (-0.5f * Mathf.Abs(Physics.gravity.y) * (time * time)) + (v.y * time) + shootPoint.position.y;

        result.y = sY;

        return result;
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
