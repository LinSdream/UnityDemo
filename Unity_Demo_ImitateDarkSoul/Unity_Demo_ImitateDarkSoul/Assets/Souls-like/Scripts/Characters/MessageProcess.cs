using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LS.Common.Message;
using System;

namespace Souls
{
    public class MessageProcess : MonoBehaviour
    {

        PlayerController _playerController;

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();

            //注册监听事件
            MessageCenter.Instance.AddListener("OnJumpEnter", OnJumpEnter);
            MessageCenter.Instance.AddListener("InGround", InGround);
            MessageCenter.Instance.AddListener("NotInGround", NotInGround);
            MessageCenter.Instance.AddListener("OnGroundEnter", OnGroundEnter);
        }

        private void OnDestroy()
        {
            //注销监听事件
            MessageCenter.Instance.RemoveListener("OnJumpEnter", OnJumpEnter);
            MessageCenter.Instance.RemoveListener("InGround", InGround);
            MessageCenter.Instance.RemoveListener("NotInGround", NotInGround);
            MessageCenter.Instance.RemoveListener("OnGroundEnter", OnGroundEnter);
        }

        #region Events
        void OnJumpEnter(GameObject sender,EventArgs e)
        {
            _playerController.LockPlanar = true;
            _playerController.ThrustVec = new Vector3(0, _playerController.JumpPower, 0);
        }

        private void NotInGround(GameObject render, EventArgs e)
        {
            _playerController.IsGrounded = false;
        }

        private void InGround(GameObject render, EventArgs e)
        {
            _playerController.IsGrounded = true;
        }

        private void OnGroundEnter(GameObject render,EventArgs e)
        {
            _playerController.LockPlanar = false;
        }
        #endregion
    }

}