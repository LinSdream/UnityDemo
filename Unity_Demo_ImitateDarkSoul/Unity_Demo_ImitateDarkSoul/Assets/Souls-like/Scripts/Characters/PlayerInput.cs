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

        void Update()
        {

            if (Input.GetButtonDown(LockCursorButton))
                LockCursor = !LockCursor;

            if(LockCursor)
            {
                CameraHorizontal = Input.GetAxis(CameraHorizontalAxis);
                CameraVertical = Input.GetAxis(CameraVerticalAxis);
            }
            if (!LockMovementInput)
            {
                Horizontal = Input.GetAxis(HorizontalAxis);
                Vertical = Input.GetAxis(VerticalAxis);
            }

            IsRun = Input.GetButton(RunButton);
            IsDefense = Input.GetButton(DefenseButton);

            IsJump = Input.GetButtonDown(JumpButton);
            IsAttack = Input.GetButtonDown(AttackButton);
        }

    }

}