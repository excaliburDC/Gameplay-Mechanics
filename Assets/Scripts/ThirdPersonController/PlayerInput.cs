using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPP
{
    public class PlayerInput : MonoBehaviour
    {
        public Action<Vector2> OnMovementInput;
        public Action<Vector3> OnMovementDirectionInput;

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
            
        }

        private void GetMovementInput()
        {
            throw new NotImplementedException();
        }
    }
}