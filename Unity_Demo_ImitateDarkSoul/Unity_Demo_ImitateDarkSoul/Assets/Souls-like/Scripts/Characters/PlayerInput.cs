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

        [Header("Buttons")]
        public string RunButton = "Run";
        public string Jump = "Jump";

        //input signal
        [HideInInspector] public float Horizontal;
        [HideInInspector] public float Vertical;

        //pressing signal
        [HideInInspector] public bool IsRun;

        //trigger signal
        public bool IsJump = false;

        [HideInInspector] public Vector3 InputVec => new Vector3(Horizontal, 0, Vertical);

        void Update()
        {
            Horizontal = Input.GetAxis(HorizontalAxis);
            Vertical = Input.GetAxis(VerticalAxis);

            IsRun = Input.GetButton(RunButton);

            IsJump = Input.GetButtonDown(Jump);

        }
    }

}