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
            BM = Bind<BattleManager>(transform.Find("BattleSensor").gameObject);
            WM = Bind<WeaponManager>(Controller.Model);
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
        public void TryDoDamg(WeaponController wc,bool attackValid,bool counterValid)
        {
   
            if (SM.CharacterState.IsDie)
                return;
            if(SM.CharacterState.IsCounterBackSuccess)
            {
                Debug.Log("!!!!!!!!!!");
                if (counterValid)
                {
                    Debug.Log("valid");
                    wc.WM.AM.Controller.Stunned();
                    return;
                }
            }
            if (SM.CharacterState.isCounterBackFailure)
            {
                if(attackValid)
                    SM.AddHP(0f);
                return;
            }
            if (SM.CharacterState.IsImmortal)
                return;
            if (SM.CharacterState.IsDefence)
                Controller.Blocked();
            else
                if(attackValid)
                    SM.AddHP(0f);

        }

        public void SetAnimAfterDoDamg(float hp)
        {
            if (hp > 0)
                Controller.Hit();
            else if (hp <= 0)
                Controller.Die();
        }

        public void SetIsCounterBack(bool on)
        {
            SM.CharacterState.IsCounterBackEnable = on;
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
            MessageCenter.Instance.AddListener("OnCounterBackExit", OnCounterBackExit);
        }

        /// <summary>
        /// MessageCenter 事件反注册
        /// </summary>
        private void UnRegister()
        {
            MessageCenter.Instance.RemoveListener("OnAttackExit", OnAttackExit);
            MessageCenter.Instance.RemoveListener("OnCounterBackExit", OnCounterBackExit);
        }

        private void OnAttackExit(GameObject sender, EventArgs e)
        {
            if (sender != gameObject)
                return;
            WM.WeaponDisable();
        }

        private void OnCounterBackExit(GameObject sender,EventArgs eventArgs)
        {
            WM.CounterBackDisable();
        }
        #endregion

    }

}