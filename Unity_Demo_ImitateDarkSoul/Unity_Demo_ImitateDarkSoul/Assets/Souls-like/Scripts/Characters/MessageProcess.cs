using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LS.Common.Message;
using System;
using LS.Helper.Timer;

namespace Souls
{
    public class MessageProcess : MonoBehaviour
    {

        PlayerController _playerController;

        float AnimatorLayerWeightValue = 0f;

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();

            //注册监听事件
            //Animator Root Motion 
            MessageCenter.Instance.AddListener("OnUpdateRootMotionDeltaPosition", OnUpdateRootMotionDeltaPosition);

            //Base Layer
            MessageCenter.Instance.AddListener("OnJumpEnter", OnJumpEnter);
            MessageCenter.Instance.AddListener("InGround", InGround);
            MessageCenter.Instance.AddListener("NotInGround", NotInGround);
            MessageCenter.Instance.AddListener("OnGroundEnter", OnGroundEnter);
            MessageCenter.Instance.AddListener("OnFallEnter", OnFallEnter);
            MessageCenter.Instance.AddListener("OnRollEnter", OnRollEnter);
            MessageCenter.Instance.AddListener("OnJabEnter", OnJabEnter);
            MessageCenter.Instance.AddListener("OnJabUpdate", OnJabUpdate);

            //Attack Layer
            MessageCenter.Instance.AddListener("OnAttackMaskIdleEnter", OnAttackMaskIdleEnter);
            MessageCenter.Instance.AddListener("OnAttackMaskIdleUpdate", OnAttackMaskIdleUpdate);
            MessageCenter.Instance.AddListener("OnAttack_RightHand_A_01_Enter", OnAttack_RightHand_A_01_Enter);
            MessageCenter.Instance.AddListener("OnAttack_RightHand_A_01_Update", OnAttack_RightHand_A_01_Update);
        }

        private void OnDestroy()
        {
            //注销监听事件
            //Animator Root Motion 
            MessageCenter.Instance.RemoveListener("OnUpdateRootMotionDeltaPosition", OnUpdateRootMotionDeltaPosition);

            //Base Layer
            MessageCenter.Instance.RemoveListener("OnJumpEnter", OnJumpEnter);
            MessageCenter.Instance.RemoveListener("InGround", InGround);
            MessageCenter.Instance.RemoveListener("NotInGround", NotInGround);
            MessageCenter.Instance.RemoveListener("OnGroundEnter", OnGroundEnter);
            MessageCenter.Instance.RemoveListener("OnFallEnter", OnFallEnter);
            MessageCenter.Instance.RemoveListener("OnRollEnter", OnRollEnter);
            MessageCenter.Instance.RemoveListener("OnJabEnter", OnJabEnter);
            MessageCenter.Instance.RemoveListener("OnJabUpdate", OnJabUpdate);

            //Attack Layer
            MessageCenter.Instance.RemoveListener("OnAttackMaskIdleEnter", OnAttackMaskIdleEnter);
            MessageCenter.Instance.RemoveListener("OnAttackMaskIdleUpdate", OnAttackMaskIdleUpdate);
            MessageCenter.Instance.RemoveListener("OnAttack_RightHand_A_01_Enter", OnAttack_RightHand_A_01_Enter);
            MessageCenter.Instance.RemoveListener("OnAttack_RightHand_A_01_Update", OnAttack_RightHand_A_01_Update);
        }

        #region  Animator Root Motion
        private void OnUpdateRootMotionDeltaPosition(GameObject sender, EventArgs e)
        {
            var arg = e as AnimatorMoveEventArgs;
            _playerController.DeltaPos += arg.deltaPosition;
        }
        #endregion

        #region Base Layer Events
        void OnJumpEnter(GameObject sender, EventArgs e)
        {
            _playerController.LockPlanar = true;
            _playerController.ThrustVec = new Vector3(0, _playerController.JumpVerlocity, 0);
        }

        private void NotInGround(GameObject sender, EventArgs e)
        {
            _playerController.IsGrounded = false;
        }

        private void InGround(GameObject sender, EventArgs e)
        {
            _playerController.IsGrounded = true;
            //_playerController.LockPlanar = false;
        }

        private void OnGroundEnter(GameObject sender, EventArgs e)
        {
            _playerController.LockPlanar = false;
        }

        private void OnFallEnter(GameObject sender, EventArgs e)
        {
            if (_playerController.IsRun)
                _playerController.LockPlanar = true;
        }

        private void OnRollEnter(GameObject sender, EventArgs e)
        {
            _playerController.ThrustVec = new Vector3(_playerController.Forward.x, _playerController.RollVelocity.y,
                _playerController.RollVelocity.z * _playerController.Forward.z);
        }

        /// TODO:修改后退动画
        private void OnJabEnter(GameObject sender, EventArgs e)
        {
            //_playerController.ThrustVec = new Vector3(-_playerController.Forward.x, _playerController.JabVerlocity.x,
            //    -_playerController.Forward.z * _playerController.JabVerlocity.y);
            _playerController.ThrustVec = new Vector3(0, _playerController.JabVerlocity.x, 0);
        }

        private void OnJabUpdate(GameObject sender, EventArgs e)
        {
            _playerController.ThrustVec = _playerController.Forward * _playerController.GetAnimFloat("JabVelocityCurve")
                * _playerController.DurationThrustMultiplier * 0.33f;
        }

        #endregion

        #region Attack Layer Events

        private void OnAttackMaskIdleEnter(GameObject sender, EventArgs e)
        {
            _playerController.SetInputLock(false);
            _playerController.LockPlanar = false;
            AnimatorLayerWeightValue = 0f;
        }

        private void OnAttackMaskIdleUpdate(GameObject sender,EventArgs e)
        {
            float currentWeight = _playerController.GetCurrentAnimatorLayerWeight("Attack Layer");
            currentWeight = Mathf.Lerp(currentWeight, AnimatorLayerWeightValue, 0.1f);

            _playerController.SetLayerWeight("Attack Layer", currentWeight);
        }

        private void OnAttack_RightHand_A_01_Enter(GameObject sender, EventArgs e)
        {
            _playerController.SetInputLock(true);
            _playerController.LockPlanar = true;
            _playerController.ResetMoveDir();
            AnimatorLayerWeightValue = 1f;
        }

        private void OnAttack_RightHand_A_01_Update(GameObject sender, EventArgs e)
        {
            float currentWeight = _playerController.GetCurrentAnimatorLayerWeight("Attack Layer");
            currentWeight = Mathf.Lerp(currentWeight, AnimatorLayerWeightValue, 0.1f);

            _playerController.SetLayerWeight("Attack Layer",currentWeight);
            //_playerController.ThrustVec = _playerController.Forward *
            //    _playerController.GetAnimFloat("Attack_RightHand_A_01_VelocityCurve") * _playerController.DurationThrustMultiplier;
        }
        #endregion

    }

}