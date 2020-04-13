using Souls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.TestCamera
{
    public class PlayerInput : UserInput
    {

        [Header("Key Axis")]
        public string HorizontalAxis = "Horizontal";
        public string VerticalAxis = "Vertical";

        public string CameraHorizontalAxis;
        public string CameraVerticalAxis;

        [Header("Key Button")]
        public string InterationButton = "Interation";
        public string LockCursorButton = "Esc";
        void Update()
        {
            //鼠标隐藏
            if (LockCursor)
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.None;

            if (Input.GetButtonDown(LockCursorButton))
                LockCursor = !LockCursor;

            IsInteration = Input.GetKeyDown(InterationButton);

            //镜头移量
            if (LockCursor)
            {
                CameraHorizontal = Input.GetAxis(CameraHorizontalAxis);
                CameraVertical = Input.GetAxis(CameraVerticalAxis);
            }

            //位移量
            if (!LockMovementInput)
            {
                Horizontal = Input.GetAxis(HorizontalAxis);
                Vertical = Input.GetAxis(VerticalAxis);
            }
            
        }
    }

}