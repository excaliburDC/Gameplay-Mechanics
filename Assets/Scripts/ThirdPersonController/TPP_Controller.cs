using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPP
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Animator))]
    public class TPP_Controller : MonoBehaviour
    {
        [SerializeField] private float moveSpeed =2.0f;
        [SerializeField] private float turnSpeed = 0.2f;
        [SerializeField] private float groundDist = 0.2f;
        [SerializeField] private LayerMask groundMask;


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

        private void HandleMovement(Vector2 input)
        {
            
            if(IsGrounded())
            {
                Debug.LogError("Grounded");
                if (input.y>0)
                {
                    moveVector = transform.forward * moveSpeed;
                }

                else
                {
                    moveVector = Vector3.zero;
                }
             
                
                
            }
        }

        private void HandleMovementDirection(Vector3 direction)
        {
            desiredRotationAngle = Vector3.Angle(transform.forward, direction);
            var crossProduct = Vector3.Cross(transform.forward, direction).y;

            if(crossProduct<0)
            {
                desiredRotationAngle *= -1;
            }

        }

        void RotatePlayer()
        {
            if(desiredRotationAngle > 10 || desiredRotationAngle < -10) 
            {
                Quaternion deltaRotation = Quaternion.Euler(Vector3.up * desiredRotationAngle * turnSpeed * Time.deltaTime);
                rb.MoveRotation(rb.rotation * deltaRotation);

                //transform.Rotate(Vector3.up * desiredRotationAngle * turnSpeed * Time.deltaTime);
            }
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