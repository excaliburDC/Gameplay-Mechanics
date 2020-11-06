using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPP
{
    public class PlayerInput : MonoBehaviour, IInput
    {
        public Action<Vector2> OnMovementInput { get; set; }
        public Action<Vector3> OnMovementDirectionInput { get; set; }

        private Camera cam;


        // Start is called before the first frame update
        void Start()
        {
            cam = Camera.main;
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Update is called once per frame
        void Update()
        {
            GetMovementInput();
            GetMovementDirectionInput();
        }

        private void GetMovementDirectionInput()
        {
            Vector3 cameraForwardDirection = cam.transform.forward;
            Debug.DrawRay(cam.transform.position, cameraForwardDirection * 10, Color.red);

            Vector3 directionToMoveIn = Vector3.Scale(cameraForwardDirection, (Vector3.right + Vector3.forward));
            Debug.DrawRay(cam.transform.position, directionToMoveIn * 10, Color.blue);

            OnMovementDirectionInput?.Invoke(directionToMoveIn);
        }

        private void GetMovementInput()
        {
            Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            OnMovementInput?.Invoke(input);
        }
    }
}