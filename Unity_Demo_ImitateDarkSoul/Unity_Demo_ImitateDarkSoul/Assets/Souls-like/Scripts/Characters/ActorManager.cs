using LS.Common.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class ActorManager : MonoBehaviour
    {

        [HideInInspector] public BattleManager BM;
        [HideInInspector] public WeaponManager WM;

        BaseController _controller;

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            Register();

            _controller = GetComponent<BaseController>();

            BM = GetComponent<BattleManager>();
            WM = GetComponent<WeaponManager>();
            if (BM == null || WM == null)
            {
                Debug.LogError("ActorManager/Awake Error : can't get BattleManager or WeaponManager in this Actor");
                return;
            }
            BM.AM = this;
        }

        private void OnDestroy()
        {
            UnRegister();
        }
        #endregion

        public void Damage()
        {
            _controller.Hit(0);
        }

        #region Message Private Methods
        /// <summary>
        /// MessageCenter 事件注册
        /// </summary>
        private void Register()
        {
            MessageCenter.Instance.AddListener("OnAttackExit", OnAttackExit);
        }

        /// <summary>
        /// MessageCenter 事件反注册
        /// </summary>
        private void UnRegister()
        {
            MessageCenter.Instance.RemoveListener("OnAttackExit", OnAttackExit);
        }

        private void OnAttackExit(GameObject sender, EventArgs e)
        {
            if (sender != gameObject)
                return;
            WM.WeaponDisable();
        }
        #endregion

    }

}