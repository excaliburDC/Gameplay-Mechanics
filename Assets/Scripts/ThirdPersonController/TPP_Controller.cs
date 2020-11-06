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

        private Vector3 m_CapsuleCentre;

        //private bool isGrounded;
        private bool isCrouching;

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
        void Start()
        {

            input.OnMovementInput += HandleMovement;
            input.OnMovementDirectionInput += (input) => { Debug.Log("Direction " + input); };
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleMovement(Vector2 input)
        {
            if(IsGrounded())
            {
                Debug.LogError("Grounded");
            }
        }

        private bool IsGrounded()
        {
           return Physics.CheckSphere(transform.position,groundDist,groundMask, QueryTriggerInteraction.Ignore);


        }


    }
}