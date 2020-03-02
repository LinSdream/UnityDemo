using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class PlayerInput : UserInput
    {

        [Header("Key Axis")]
        public string HorizontalAxis = "Horizontal";
        public string VerticalAxis = "VerticalAxis";

        public string CameraHorizontalAxis;
        public string CameraVerticalAxis;

        [Header("Buttons")]
        public string RunButton = "Run";
        public string JumpButton = "Jump";
        public string LockCursorButton = "Esc";
        public string DefenseButton = "Defense";
        public string LockOnButton = "LockOn";
        public string RightButton = "RightButton";
        public string LeftButton = "LeftButton";
        public string RightTrigger = "RightTrigger";
        public string LeftTrigger = "LeftTrigger";

        protected override void Init()
        {
            Cursor.lockState = CursorLockMode.Locked;

            RightButtonInfo = new InputInfo(RightButton);
            LeftButtonInfo = new InputInfo(LeftButton);

            RightTriggerInfo = new InputInfo(RightTrigger);
            LeftTriggerInfo = new InputInfo(LeftTrigger);
        }

        void Update()
        {
            //鼠标隐藏
            if (LockCursor)
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.None;

            if (Input.GetButtonDown(LockCursorButton))
                LockCursor = !LockCursor;

            //镜头移量
            if(LockCursor)
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


            Debug.Log(LockMovementInput);


            //按压触发的按键
            IsRun = Input.GetButton(RunButton);
            IsDefense = Input.GetButton(DefenseButton);

            IsRightButton = Input.GetButton(RightButton);
            IsLeftButton = Input.GetButton(LeftButton);

            IsDefense = (IsRightButton || IsLeftButton);

            //瞬间触发的按键
            IsJump = Input.GetButtonDown(JumpButton);
            IsLockOn = Input.GetButtonDown(LockOnButton);

            IsLeftTrigger = Input.GetButtonDown(LeftTrigger);
            IsRightTrigger = Input.GetButtonDown(RightTrigger);

            IsAttack = (IsLeftTrigger || IsRightTrigger);

        }

    }

}