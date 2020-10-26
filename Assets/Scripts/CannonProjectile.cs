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
    public float speed;
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


            cannonHead.rotation = Quaternion.LookRotation(projectileHitPoint.transform.position);

            if (Input.GetMouseButtonDown(0))
            {
                Rigidbody rbObj = Instantiate(cannonBall, shootPoint.position, Quaternion.identity);

                rbObj.useGravity = true;

                //rbObj.velocity = cannonVel;

                rbObj.AddForce(cannonVel, ForceMode.VelocityChange);
            }
        }
    }

    private Vector3 CalculateBallTrajectory(Vector3 origin, Vector3 targetPos)
    {
        Vector3 distance = targetPos - origin;

        // Set up the terms we need to solve the quadratic equations.
        float gSquared = Physics.gravity.sqrMagnitude;
        float b = speed * speed + Vector3.Dot(distance, Physics.gravity);
        float discriminant = b * b - gSquared * distance.sqrMagnitude;

        // Check whether the target is reachable at max speed or less.
        if (discriminant < 0)
        {
            // Target is too far away to hit at this speed.
            // Abort, or fire at max speed in its general direction?
        }

        float discRoot = Mathf.Sqrt(discriminant);

        // Highest shot with the given max speed:
        float T_max = Mathf.Sqrt((b + discRoot) * 2f / gSquared);

        // Most direct shot with the given max speed:
        float T_min = Mathf.Sqrt((b - discRoot) * 2f / gSquared);

        // Lowest-speed arc available:
        float T_lowEnergy = Mathf.Sqrt(Mathf.Sqrt(distance.sqrMagnitude * 4f / gSquared));

        float T = T_min; // choose T_max, T_min, or some T in-between like T_lowEnergy

        // Convert from time-to-hit to a launch velocity:
        Vector3 velocity = distance / T - Physics.gravity * T / 2f;

        return velocity;
    }

    private void DrawTrajectorypath(Vector3 cannonVel)
    {
        throw new NotImplementedException();
    }
}
