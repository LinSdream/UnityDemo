using LS.CustomInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public abstract class UserInput : InputBase
    {

        //pressing signal
        [HideInInspector] public bool IsRun { get; protected set; }
        [HideInInspector] public bool IsDefense { get; protected set; }
        [HideInInspector] public bool IsLockOn { get; protected set; }
        [HideInInspector] public bool IsRightTrigger { get; protected set; }
        [HideInInspector] public bool IsLeftTrigger { get; protected set; }

        //trigger signal
        [HideInInspector] public bool IsJump { get; protected set; }
        [HideInInspector] public bool IsAttack { get; protected set; }
        [HideInInspector] public bool IsRightButton { get; protected set; }
        [HideInInspector] public bool IsLeftButton { get; protected set; }
        [HideInInspector] public bool IsInteration { get; protected set; }

        [HideInInspector] public Vector3 InputVec => new Vector3(Horizontal, 0, Vertical);
        [HideInInspector] public bool LockMovementInput = false;

        //[HideInInspector] public InputInfo RightButtonInfo;
        //[HideInInspector]public InputInfo LeftButtonInfo;
        //[HideInInspector]public InputInfo RightTriggerInfo;
        //[HideInInspector]public InputInfo LeftTriggerInfo;


        private void Awake()
        {
            IsRun = false;
            IsDefense = false;
            IsLockOn = false;
            IsJump = false;
            IsLeftButton = false;
            IsRightButton = false;
            IsRightTrigger = false;
            LockCursor = true;
            IsAttack = false;
            IsInteration = false;
            LockMovementInput = false;
            Init();
        }

        protected virtual void Init()
        {

        }

        public void ResetInputVec()
        {
            Horizontal = 0;
            Vertical = 0;
        }
       
    }

}