using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonProjectile : MonoBehaviour
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
    //public float velocity;
    [Range(0f, 360f)]
    public float launchAngle;
    public int lineSegment = 10;

    private float radianAngle;
    private float gravity;

    // Start is called before the first frame update
    void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        cannonBall.useGravity = false;
        line.positionCount = lineSegment;
        radianAngle = launchAngle * Mathf.Deg2Rad;
        gravity = Physics.gravity.magnitude;
    }

    void Update()
    {
        LaunchProjectile();
    }

    private void LaunchProjectile()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);


        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {
            projectileHitPoint.SetActive(true);
            projectileHitPoint.transform.position = hit.point + Vector3.up * 0.1f;
            //projectileHitPoint.transform.rotation = Quaternion.Euler(hit.point + Vector3.up * 0.1f);

            Vector3 cannonVel = CalculateBallTrajectory(shootPoint.position, hit.point);

           // DrawTrajectorypath(cannonVel);


            cannonHead.rotation = Quaternion.LookRotation(cannonVel);

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
        Vector3 distance = targetPos - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0f;

      
        
        float sY = distance.y;
        float sXZ = distanceXZ.magnitude;

        float velocity = Mathf.Sqrt((sXZ * sXZ * gravity) / (sXZ * Mathf.Sin(2 * radianAngle)) - 2 * sY * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle));

        float xzVelocity = velocity * Mathf.Cos(radianAngle);
        float yVelocity = velocity * Mathf.Sin(radianAngle) - gravity * 1f;

        Vector3 result = distanceXZ.normalized;
        result *= xzVelocity ;
        result.y = yVelocity;

        return result;

      //  return velocity * distanceXZ.normalized;
    }

    private void DrawTrajectorypath(Vector3 cannonVel)
    {
        throw new NotImplementedException();
    }
}
