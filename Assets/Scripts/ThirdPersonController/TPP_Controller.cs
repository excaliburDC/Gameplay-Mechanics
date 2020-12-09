using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPP
{
    public enum MovementDir
    {
        IDLE,
        UP,
        DOWN,
        LEFT,
        RIGHT
    }


    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Animator))]
    public class TPP_Controller : MonoBehaviour
    {
        [SerializeField] private MovementDir dir;
        [SerializeField] private float moveSpeed =2.0f;
        [SerializeField] private float turnSpeed = 0.2f;
        [SerializeField] private float groundDist = 0.2f;
        [SerializeField] private float acceleration = 2.0f;
        [SerializeField] private float deceleration = 2.0f;
        [SerializeField] private LayerMask groundMask;


        private float turnSmoothRef;

        private IInput input;
        private Rigidbody rb;
        private CapsuleCollider capsule;
        private Animator animator;

        private Vector3 moveVector = Vector3.zero;
        private Vector3 m_CapsuleCentre;

        //private bool isGrounded;
        private bool isCrouching;

        private float desiredRotationAngle = 0f;
        private float m_CapsuleHeight;
        private float velocityX = 0.0f;
        private float velocityZ = 0.0f;

        private int velocityXHash;
        private int velocityZHash;


        private void Awake()
        {
            input = GetComponent<IInput>();
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            capsule = GetComponent<CapsuleCollider>();
            m_CapsuleHeight = capsule.height;
            m_CapsuleCentre = capsule.center;

        }

        // Start is called before the first frame update
        private void OnEnable()
        {
            velocityXHash = Animator.StringToHash("VelocityX");
            velocityZHash = Animator.StringToHash("VelocityZ");

            input.OnMovementInput += HandleMovement;
            input.OnMovementDirectionInput += HandleMovementDirection;


        }

    

        // Update is called once per frame
        void Update()
        {
            //if (IsGrounded() && moveVector.magnitude > 0)
            //{
            //    RotatePlayer();
            //}
        }

        private void FixedUpdate()
        {
            if (IsGrounded() && moveVector.magnitude > 0)
            {
                RotatePlayer();
            }

            rb.MovePosition(rb.position + moveVector.normalized * Time.fixedDeltaTime);
        }

        private void HandleMovement(float h, float v)
        {
            
            if(IsGrounded())
            {
                //Debug.LogError("Grounded");

                

                if (h != 0) 
                {
                    moveVector = h * transform.right * moveSpeed;
                    dir = (h > 0) ? MovementDir.RIGHT : MovementDir.LEFT;
                }

                else if (v != 0)
                {
                    moveVector = v * transform.forward * moveSpeed;

                    dir = (v > 0) ? MovementDir.UP : MovementDir.DOWN;
                }

                else
                {
                    moveVector = Vector3.zero;

                    dir = MovementDir.IDLE;
                }
                

                
                
            }
        }

        private void HandleMovementDirection(Vector3 direction)
        {
            Vector3 turnDirection = CheckTurnDirection();

            desiredRotationAngle = Vector3.Angle(turnDirection, direction);
            Debug.Log("Rotation Angle: "+desiredRotationAngle);
            var crossProduct = Vector3.Cross(transform.forward, direction).y;
            Debug.Log("Cross Product:" + crossProduct);

            if (crossProduct < 0)
            {
                desiredRotationAngle *= -1;
            }

            //if(direction.magnitude>0f)
            //{
            //    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Deg2Rad;

            //    desiredRotationAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothRef, turnSpeed);
            //}

        }

        private Vector3 CheckTurnDirection()
        {
            Vector3 turnDir = Vector3.zero;
            switch(dir)
            {
                case MovementDir.UP:
                    turnDir = transform.forward;
                    break;

                case MovementDir.DOWN:
                    turnDir = -transform.forward;
                    break;

                case MovementDir.RIGHT:
                    turnDir = transform.right;
                    break;

                case MovementDir.LEFT:
                    turnDir = -transform.right;
                    break;

            }

            return turnDir;

            
        }

        void RotatePlayer()
        {
            if (desiredRotationAngle > 10 || desiredRotationAngle < -10)
            {
                Quaternion deltaRotation = Quaternion.Euler(Vector3.up * desiredRotationAngle * turnSpeed * Time.deltaTime);
                rb.MoveRotation(rb.rotation * deltaRotation);

                //transform.Rotate(Vector3.up * desiredRotationAngle * turnSpeed * Time.deltaTime);
            }

            //float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
           // transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);


            //Quaternion delta = Quaternion.Euler(Vector3.up * desiredRotationAngle * turnSpeed * Time.deltaTime);
            //rb.MoveRotation(rb.rotation * delta);
        }

        private void HandleAnimations()
        {

        }

        private bool IsGrounded()
        {
           
           return Physics.CheckSphere(transform.position,groundDist,groundMask, QueryTriggerInteraction.Ignore);


        }

        

        private void OnDisable()
        {

            input.OnMovementInput -= HandleMovement;
            input.OnMovementDirectionInput -= HandleMovementDirection;
        }


    }
}