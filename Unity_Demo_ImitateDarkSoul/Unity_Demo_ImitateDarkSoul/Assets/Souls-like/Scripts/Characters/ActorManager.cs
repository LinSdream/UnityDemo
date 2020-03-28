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
        [HideInInspector] public StateManager SM;
        [HideInInspector] public BaseController Controller;

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            Register();

            Controller = GetComponent<BaseController>();

            //脚本获取
            BM = Bind<BattleManager>(gameObject);
            WM = Bind<WeaponManager>(gameObject);
            SM = Bind<StateManager>(gameObject);

            //创建游玩过程中的角色信息
            SM.Info = Controller.Info;
            SM.TempInfo = new CharacterTempInfo(Controller.Info);

            SM.Test();
        }

        private void OnDestroy()
        {
            UnRegister();
        }
        #endregion

        /// <summary>
        /// 尝试计算伤害，根据计算后得出的不同值进行不同的处理
        /// </summary>
        public void TryDoDamg()
        {
            if (SM.CharacterState.IsDie)
                return;
            if (SM.CharacterState.IsImmortal)
                return;
            if (SM.CharacterState.IsDefence)
                Controller.Blocked();
            else
                SM.AddHP(-50f);

        }

        public void SetAnimAfterDoDamg(float hp)
        {
            if (hp > 0)
                Controller.Hit();
            else if (hp <= 0)
                Controller.Die();
        }

        #region Private Help Methods

        /// <summary> 绑定脚本，如果脚本不存在，则自动添加 </summary>
        T Bind<T>(GameObject go) where T :AbstractActorManager
        {
            T temp = go.GetComponent<T>();
            if (temp == null)
                temp = go.AddComponent<T>();
            temp.AM = this;
            return temp;
        }

        #endregion


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