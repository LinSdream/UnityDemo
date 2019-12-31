using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class PlayerInput : MonoBehaviour
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

        //output signal
        [HideInInspector] public float Horizontal;
        [HideInInspector] public float Vertical;
        [HideInInspector] public float CameraHorizontal;
        [HideInInspector] public float CameraVertical;

        //pressing signal
        [HideInInspector] public bool IsRun;

        //trigger signal
        [HideInInspector] public bool IsJump = false;

        //others
        [HideInInspector] public bool LockCursor = true;
        [HideInInspector] public Vector3 InputVec => new Vector3(Horizontal, 0, Vertical);

        private void Awake()
        {
            IsRun = false;
            IsJump = false;
            LockCursor = true;
        }

        void Update()
        {

            if (Input.GetButtonDown(LockCursorButton))
                LockCursor = !LockCursor;

            if(LockCursor)
            {
                CameraHorizontal = Input.GetAxis(CameraHorizontalAxis);
                CameraVertical = Input.GetAxis(CameraVerticalAxis);
            }

            Debug.Log(new Vector3(CameraHorizontal, 0, CameraVertical));

            Horizontal = Input.GetAxis(HorizontalAxis);
            Vertical = Input.GetAxis(VerticalAxis);

            IsRun = Input.GetButton(RunButton);

            IsJump = Input.GetButtonDown(JumpButton);

        }
    }

}