using LS.Common.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{

    /// <summary>
    /// 利用Animation中的Root Motion来控制动画的位移，截断获取Move距离，然后传回父节点，确保Collider与模型不会脱节
    /// </summary>
    public class RootMotionControl : MonoBehaviour
    {

        public Vector3 AnimatorIK_LeftLowerHandRotation = Vector3.zero;

        Animator _anim;
        AnimatorMoveEventArgs _args;

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _args = new AnimatorMoveEventArgs();
        }

        private void OnAnimatorMove()
        {
            if (CheckAnimatorState("OnAttackR_01A", "Base Layer")
                || CheckAnimatorState("OnAttackR_01C", "Base Layer"))
            {
                _args.deltaPosition = _anim.deltaPosition;
                MessageCenter.Instance.SendMessage("OnUpdateRootMotionDeltaPosition", gameObject, _args);
            }
        }

        private void OnAnimatorIK(int layerIndex)
        {
            ///TODO:需要将Idle状态下的左手ik进行调整

            if (CheckAnimatorState("Ground"))
            {
                Transform leftLowerArm = _anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
                leftLowerArm.localEulerAngles += AnimatorIK_LeftLowerHandRotation;
                _anim.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm, Quaternion.Euler(leftLowerArm.localEulerAngles));
            }

        }

        #endregion

        public bool CheckAnimatorState(string animtorName, string maskName = "Base Layer")
        {
            return _anim.GetCurrentAnimatorStateInfo(_anim.GetLayerIndex(maskName))
                .IsName(animtorName);
        }
    }

}