using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BallisticTrajectory : MonoBehaviour
{
    
    // Reference to the line renderer
    private LineRenderer line;
    // Initial trajectory position
    [SerializeField]
    private Vector3 shootPoint;
    // Initial trajectory velocity
    [SerializeField]
    private Vector3 startVelocity;
    // Step distance for the trajectory
    [SerializeField]
    private float trajectoryVertDist = 0.25f;
    // Max length of the trajectory
    [SerializeField]
    private float maxCurveLength = 5;
    [Header("Debug")]
    // Flag for always drawing trajectory
    [SerializeField]
    private bool _debugAlwaysDrawTrajectory = false;
    [SerializeField]
    private LayerMask layer;
    [SerializeField]
    private GameObject projectileHitPoint;

    private Camera cam;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        cam = Camera.main;
    }

    private void Update()
    {
        
    }

    private void LaunchProjectile()
    {
        // Create a list of trajectory points
        var curvePoints = new List<Vector3>();
        curvePoints.Add(shootPoint);

        // Initial values for trajectory
        var currentPosition = shootPoint;
        var currentVelocity = startVelocity;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);


        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {
            projectileHitPoint.SetActive(true);
            projectileHitPoint.transform.position = hit.point + Vector3.up * 0.1f;

            //DrawTrajectory();

            // Time to travel distance of trajectoryVertDist
            var t = trajectoryVertDist / currentVelocity.magnitude;

            // Update position and velocity
            currentVelocity = currentVelocity + t * Physics.gravity;
            currentPosition = currentPosition + t * currentVelocity;
            // Add point to the trajectory
            curvePoints.Add(currentPosition);

            // If something was hit, add last point there
            if (hit.transform)
            {
                curvePoints.Add(hit.point);
            }

            // Display line with all points
            line.positionCount = curvePoints.Count;
            line.SetPositions(curvePoints.ToArray());



        }
    }


    public void SetBallisticValues(Vector3 startPosition, Vector3 startVelocity)
    {
        this.shootPoint = startPosition;
        this.startVelocity = startVelocity;
    }
    private void DrawTrajectory()
    {
        // Create a list of trajectory points
        var curvePoints = new List<Vector3>();
        curvePoints.Add(shootPoint);

        // Initial values for trajectory
        var currentPosition = shootPoint;
        var currentVelocity = startVelocity;

        // Init physics variables
        RaycastHit hit;
        Ray ray = new Ray(currentPosition, currentVelocity.normalized);
        // Loop until hit something or distance is too great
        while (!Physics.Raycast(ray, out hit, trajectoryVertDist) && Vector3.Distance(shootPoint, currentPosition) < maxCurveLength)
        {
            // Time to travel distance of trajectoryVertDist
            var t = trajectoryVertDist / currentVelocity.magnitude;

            // Update position and velocity
            currentVelocity = currentVelocity + t * Physics.gravity;
            currentPosition = currentPosition + t * currentVelocity;
            // Add point to the trajectory
            curvePoints.Add(currentPosition);
            // Create new ray
            ray = new Ray(currentPosition, currentVelocity.normalized);
        }
        // If something was hit, add last point there
        if (hit.transform)
        {
            curvePoints.Add(hit.point);
        }
        // Display line with all points
        line.positionCount = curvePoints.Count;
        line.SetPositions(curvePoints.ToArray());
    }

}
