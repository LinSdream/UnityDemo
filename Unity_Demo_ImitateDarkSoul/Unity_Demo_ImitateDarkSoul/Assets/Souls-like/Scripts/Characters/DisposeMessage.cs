using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LS.Common.Message;
using System;

namespace Souls
{
    public class DisposeMessage : MonoBehaviour
    {

        PlayerController _playerController;

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();

            //注册监听事件
            MessageCenter.Instance.AddListener("OnJumpEnter", OnJumpEnter);
            MessageCenter.Instance.AddListener("OnJumpExit", OnJumpExit);
        }

        private void OnDestroy()
        {
            //注销监听事件
            MessageCenter.Instance.RemoveListener("OnJumpEnter", OnJumpEnter);
            MessageCenter.Instance.RemoveListener("OnJumpExit", OnJumpExit);
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
        #endregion
    }

}