using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public  abstract class UserInput : MonoBehaviour
    {
        //output signal
        [HideInInspector] public float Horizontal;
        [HideInInspector] public float Vertical;
        [HideInInspector] public float CameraHorizontal;
        [HideInInspector] public float CameraVertical;

        //pressing signal
        [HideInInspector] public bool IsRun;
        [HideInInspector] public bool IsDefense = false;

        //trigger signal
        [HideInInspector] public bool IsJump = false;
        [HideInInspector] public bool IsAttack = false;

        //others
        public bool LockCursor = true;
        [HideInInspector] public Vector3 InputVec => new Vector3(Horizontal, 0, Vertical);
        [HideInInspector] public bool LockMovementInput = false;

        private void Awake()
        {
            IsRun = false;
            IsJump = false;
            LockCursor = true;
            IsAttack = false;
            LockMovementInput = false;
            Init();
        }

        protected virtual void Init()
        {

        }

    }

}