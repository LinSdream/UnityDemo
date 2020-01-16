using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LS.Common.Message;
using System;
using LS.Helper.Timer;

namespace Souls
{

    [RequireComponent(typeof(PlayerController), typeof(UserInput))]
    public class MessageProcess : MonoBehaviour
    {

        PlayerController _playerController;
        UserInput _input;

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();

            _input = GetComponent<UserInput>();

            //注册监听事件
            //Animator Root Motion
            MessageCenter.Instance.AddListener("OnUpdateRootMotionDeltaPosition", OnUpdateRootMotionDeltaPosition);

            //Check Model is in Ground
            MessageCenter.Instance.AddListener("IsInGround", IsInGround);

            //Base Layer
            MessageCenter.Instance.AddListener("OnJumpEnter", OnJumpEnter);
            MessageCenter.Instance.AddListener("OnGroundEnter", OnGroundEnter);
            MessageCenter.Instance.AddListener("OnFallEnter", OnFallEnter);
            MessageCenter.Instance.AddListener("OnRollEnter", OnRollEnter);
            MessageCenter.Instance.AddListener("OnJabEnter", OnJabEnter);
            MessageCenter.Instance.AddListener("OnJabUpdate", OnJabUpdate);
            MessageCenter.Instance.AddListener("OnRollExit", OnRollExit);
            MessageCenter.Instance.AddListener("OnAttackR_01A", OnAttackR_01A);
        }


        private void OnDestroy()
        {
            //注销监听事件
            //Animator Root Motion 
            MessageCenter.Instance.RemoveListener("OnUpdateRootMotionDeltaPosition", OnUpdateRootMotionDeltaPosition);

            //Check Model is in Ground
            MessageCenter.Instance.RemoveListener("IsInGround", IsInGround);

            //Base Layer
            MessageCenter.Instance.RemoveListener("OnJumpEnter", OnJumpEnter);
            MessageCenter.Instance.RemoveListener("OnGroundEnter", OnGroundEnter);
            MessageCenter.Instance.RemoveListener("OnFallEnter", OnFallEnter);
            MessageCenter.Instance.RemoveListener("OnRollEnter", OnRollEnter);
            MessageCenter.Instance.RemoveListener("OnJabEnter", OnJabEnter);
            MessageCenter.Instance.RemoveListener("OnJabUpdate", OnJabUpdate);
            MessageCenter.Instance.RemoveListener("OnAttackR_01A", OnAttackR_01A);
        }

        #endregion

        #region Check Model is in Ground

        private void IsInGround(GameObject sender, EventArgs e)
        {
            _playerController.IsGrounded = (e as IsInGroundEventArgs).IsInGround;
        }

        #endregion

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
            _playerController.TrackDirection = true;
            _playerController.LockPlanar = true;
            _playerController.ThrustVec = new Vector3(0, _playerController.JumpVerlocity, 0);
        }

        private void OnGroundEnter(GameObject sender, EventArgs e)
        {
            _playerController.LockPlanar = false;
            _playerController.TrackDirection = false;
        }

        private void OnFallEnter(GameObject sender, EventArgs e)
        {
            if (_playerController.IsRun)
                _playerController.LockPlanar = true;
        }

        private void OnRollEnter(GameObject sender, EventArgs e)
        {
            _playerController.TrackDirection = true;
            if (_playerController.CameraCol.LockTarget != null)
            {
                _playerController.ThrustVec = new Vector3(_playerController.Forward.x * _playerController.RollVelocity.x
                    , _playerController.RollVelocity.y, _playerController.Forward.z * _playerController.RollVelocity.x);
            }
            else
            {
                //消息处理优先于PlayerController执行，因此需要计算roll的forward向量来计算ThrustVec
                var forward = (_input.Horizontal * transform.right + _input.Vertical * transform.forward).normalized;
                _playerController.ThrustVec = new Vector3(forward.x * _playerController.RollVelocity.x
                    , _playerController.RollVelocity.y, forward.z * _playerController.RollVelocity.y); ;
            }
        }
        private void OnRollExit(GameObject sender, EventArgs e)
        {
            _playerController.LockPlanar = false;
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

        private void OnAttackR_01A(GameObject sender, EventArgs e)
        {
            _playerController.SetInputLock(true);
            _playerController.LockPlanar = true;
            _playerController.ResetMoveDirZero();
        }


        #endregion
    }

}