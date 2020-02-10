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

            //Base Layer
            MessageCenter.Instance.AddListener("OnJumpEnter", OnJumpEnter);
            MessageCenter.Instance.AddListener("OnGroundEnter", OnGroundEnter);
            MessageCenter.Instance.AddListener("OnFallEnter", OnFallEnter);
            MessageCenter.Instance.AddListener("OnRollEnter", OnRollEnter);
            MessageCenter.Instance.AddListener("OnJabEnter", OnJabEnter);
            MessageCenter.Instance.AddListener("OnJabUpdate", OnJabUpdate);
            MessageCenter.Instance.AddListener("OnRollExit", OnRollExit);
            MessageCenter.Instance.AddListener("OnAttackR_01A", OnAttackR_01A);
            MessageCenter.Instance.AddListener("OnHitEnter", OnHitEnter);
        }

        private void OnDestroy()
        {
            //注销监听事件
            //Animator Root Motion 
            MessageCenter.Instance.RemoveListener("OnUpdateRootMotionDeltaPosition", OnUpdateRootMotionDeltaPosition);

            //Base Layer
            MessageCenter.Instance.RemoveListener("OnJumpEnter", OnJumpEnter);
            MessageCenter.Instance.RemoveListener("OnGroundEnter", OnGroundEnter);
            MessageCenter.Instance.RemoveListener("OnFallEnter", OnFallEnter);
            MessageCenter.Instance.RemoveListener("OnRollEnter", OnRollEnter);
            MessageCenter.Instance.RemoveListener("OnJabEnter", OnJabEnter);
            MessageCenter.Instance.RemoveListener("OnJabUpdate", OnJabUpdate);
            MessageCenter.Instance.RemoveListener("OnAttackR_01A", OnAttackR_01A);
            MessageCenter.Instance.RemoveListener("OnHitEnter", OnHitEnter);
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
            _playerController.SetInputLock(false);
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
                var forward = (Vector3.Scale(_playerController.CameraCol.CameraObj.forward, new Vector3(1, 0, 1))
                   * _input.Vertical + _playerController.CameraCol.CameraObj.right * _input.Horizontal).normalized;
                //var forward = (_input.Horizontal * transform.right + _input.Vertical * transform.forward).normalized;
                _playerController.ThrustVec = new Vector3(forward.x * _playerController.RollVelocity.x
                    , _playerController.RollVelocity.y, forward.z * _playerController.RollVelocity.y);

                Debug.Log((_playerController.CameraCol.LockTarget.transform.position - transform.position).magnitude);
                //var forward = (_input.Horizontal * _playerController.Model.transform.right + _input.Vertical * _playerController.Model.transform.forward)
                //    * _playerController.Rigidbody.magnitude;
                //_playerController.Rigidbody = forward;
                //Debug.Log("Ing" + _playerController.Rigidbody);
                //_playerController.ThrustVec = new Vector3(forward.x * _playerController.RollVelocity.x
                //    , _playerController.RollVelocity.y, forward.z * _playerController.RollVelocity.x);
            }
            else
            {
                //消息处理优先于PlayerController执行，因此需要计算roll的forward向量来计算ThrustVec
                var forward = (Vector3.Scale(_playerController.CameraCol.CameraObj.forward, new Vector3(1, 0, 1))
                    * _input.Vertical + _playerController.CameraCol.CameraObj.right * _input.Horizontal).normalized;
                //var forward = (_input.Horizontal * transform.right + _input.Vertical * transform.forward).normalized;
                _playerController.ThrustVec = new Vector3(forward.x * _playerController.RollVelocity.x
                    , _playerController.RollVelocity.y, forward.z * _playerController.RollVelocity.y); 
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

        private void OnHitEnter(GameObject render, EventArgs e)
        {
            _playerController.SetInputLock(true);
        }

        #endregion
    }

}