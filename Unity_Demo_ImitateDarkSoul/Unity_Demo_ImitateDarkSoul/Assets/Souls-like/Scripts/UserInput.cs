using LS.CustomInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public abstract class UserInput : InputBase
    {

        //pressing signal
        [HideInInspector] public bool IsRun = false;
        [HideInInspector] public bool IsDefense = false;
        [HideInInspector] public bool IsLockOn = false;
        [HideInInspector] public bool IsRightTrigger = false;
        [HideInInspector] public bool IsLeftTrigger = false;

        //trigger signal
        [HideInInspector] public bool IsJump = false;
        [HideInInspector] public bool IsAttack = false;
        [HideInInspector] public bool IsRightButton = false;
        [HideInInspector] public bool IsLeftButton = false;

        [HideInInspector] public Vector3 InputVec => new Vector3(Horizontal, 0, Vertical);
        [HideInInspector] public bool LockMovementInput = false;



        //[HideInInspector] public InputInfo RightButtonInfo;
        //[HideInInspector]public InputInfo LeftButtonInfo;
        //[HideInInspector]public InputInfo RightTriggerInfo;
        //[HideInInspector]public InputInfo LeftTriggerInfo;


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