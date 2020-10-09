using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TPPRigidbody : MonoBehaviour
{
    public float Speed = 5f;
    public float JumpHeight = 2f;
    public float GroundDistance = 0.2f;
    public float DashDistance = 5f;
    public float turnRate = 0.1f;
    public LayerMask Ground;
    public Transform cam;

    private Rigidbody rb;
    private float turnVelocity;
    private Vector3 moveDir = Vector3.zero;
    private Vector3 move;
    private bool isGrounded = true;
    private Transform groundChecker;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundChecker = transform.GetChild(0);
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);


        moveDir = Vector3.zero;
        moveDir.x = Input.GetAxis("Horizontal");
        moveDir.z = Input.GetAxis("Vertical");

        //if (moveDir != Vector3.zero)
        //{
            
            
        //}



        if (moveDir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Deg2Rad * cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, turnRate);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            move = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            //transform.forward = move.normalized;
            transform.forward = moveDir.normalized + move;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Debug.Log("Jumped");
            rb.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -4f * Physics.gravity.y), ForceMode.VelocityChange);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Dash");
            Vector3 dashVelocity = Vector3.Scale(transform.forward, DashDistance * 
                                    new Vector3((Mathf.Log(1f / (Time.deltaTime * rb.drag + 1)) / -Time.deltaTime), 
                                    0, (Mathf.Log(1f / (Time.deltaTime * rb.drag + 1)) / -Time.deltaTime)));
            rb.AddForce(dashVelocity, ForceMode.VelocityChange);
        }
    }


    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDir.normalized * Speed * Time.fixedDeltaTime);
    }
}
