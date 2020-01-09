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
        public string AttackButton = "Attack";
        public string DefenseButton = "Defense";
        public string LockOnButton = "LockOn";

        protected override void Init()
        {
            Cursor.lockState = CursorLockMode.Locked;
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

            //按压触发的按键
            IsRun = Input.GetButton(RunButton);
            IsDefense = Input.GetButton(DefenseButton);

            //瞬间触发的按键
            IsJump = Input.GetButtonDown(JumpButton);
            IsAttack = Input.GetButtonDown(AttackButton);
            IsLockOn = Input.GetButtonDown(LockOnButton);
        }

    }

}