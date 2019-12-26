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
            MessageCenter.Instance.AddListener("OnJumpExit", OnJumpExit);
            MessageCenter.Instance.AddListener("InGround", InGround);
            MessageCenter.Instance.AddListener("NotInGround", NotInGround);
        }

        private void OnDestroy()
        {
            //注销监听事件
            MessageCenter.Instance.RemoveListener("OnJumpEnter", OnJumpEnter);
            MessageCenter.Instance.RemoveListener("OnJumpExit", OnJumpExit);
            MessageCenter.Instance.RemoveListener("InGround", InGround);
            MessageCenter.Instance.RemoveListener("NotInGround", NotInGround);
        }

        #region Events
        void OnJumpEnter(GameObject sender,EventArgs e)
        {
            _playerController.LockPlanar = true;
            _playerController.ThrustVec = new Vector3(0, _playerController.JumpPower, 0);
        }

        void OnJumpExit(GameObject sender,EventArgs e)
        {
            _playerController.LockPlanar = false;
        }

        private void NotInGround(GameObject render, EventArgs e)
        {
            _playerController.IsGrounded = false;
        }

        private void InGround(GameObject render, EventArgs e)
        {
            _playerController.IsGrounded = true;
        }
        #endregion
    }

}