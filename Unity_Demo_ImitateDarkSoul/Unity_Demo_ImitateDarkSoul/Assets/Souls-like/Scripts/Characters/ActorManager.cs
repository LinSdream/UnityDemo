using LS.Common.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{

    /// <summary> 角色管理 </summary>
    public class ActorManager : MonoBehaviour
    {

        #region Manager Fields
        /// <summary> 战斗模块管理 </summary>
        [HideInInspector] public BattleManager BM;
        /// <summary> 武器管理，之后的武器系统有WM完成 </summary>
        [HideInInspector] public WeaponManager WM;
        /// <summary> 状态管理，多种状态的判断 </summary>
        [HideInInspector] public StateManager SM;
        /// <summary> 角色控制器 </summary>
        [HideInInspector] public BaseController Controller;
        /// <summary> 导演控制器 </summary>
        [HideInInspector] public DirectorManager DM;
        /// <summary> 交互管理器 </summary>
        [HideInInspector] public InteractionManager IM;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {

            Controller = GetComponent<BaseController>();

            var sensor = transform.Find("BattleSensor").gameObject;

            //脚本获取
            BM = Bind<BattleManager>(sensor);
            WM = Bind<WeaponManager>(Controller.Model);
            SM = Bind<StateManager>(gameObject);
            DM = Bind<DirectorManager>(gameObject);
            IM = Bind<InteractionManager>(sensor);

            //创建游玩过程中的角色信息
            SM.Info = Controller.Info;
            SM.TempInfo = new CharacterTempInfo(Controller.Info);

            Register();
        }

        private void OnDestroy()
        {
            UnRegister();
        }
        #endregion

        #region Other Methods

        public void DoInterationEvent()
        {
            foreach(var cell in IM.OverlapEcastms)
            {
                switch(cell.EventName)
                {
                    case "StabFront":
                        DM.Play("FrontStab", this,cell.AM);
                            break;
                }
            }
        }

        /// <summary>
        /// 尝试计算伤害，根据计算后得出的不同值进行不同的处理
        /// </summary>
        public void TryDoDamg(WeaponController wc,bool attackValid,bool counterValid)
        {
            //角色已经死亡，不计算任何伤害
            if (SM.CharacterState.IsDie)
                return;
            //角色弹反成功
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
            //弹反失败
            if (SM.CharacterState.isCounterBackFailure)
            {
                //如果攻击有效，即在弹反过程中受到伤害
                if(attackValid)
                    SM.AddHP(-wc.WM.AM.SM.TempInfo.Damage);
                return;
            }
            //是否无敌，无敌状态下，不收到任何伤害
            if (SM.CharacterState.IsImmortal)
                return;
            //是否防御，防御情况下，动画展示
            if (SM.CharacterState.IsDefence)
                Controller.Blocked();
            else//所有情况外，就是受伤
                if(attackValid)//敌人攻击范围有效
                    SM.AddHP(-wc.WM.AM.SM.TempInfo.Damage);
        }

        /// <summary> 根据血条进行不同的动画展示 </summary>
        public void SetAnimAfterDoDamg(float hp)
        {
            if (hp > 0)
                Controller.Hit();
            else if (hp <= 0)
            {
                Controller.Die();
                WM.WeaponDisable();
            }
        }

        public void SetIsCounterBack(bool on)
        {
            SM.CharacterState.IsCounterBackEnable = on;
        }

        public void LockUnLockActorController(bool on)
        {
            Controller.IssueBool("Lock", on);
        }

        #endregion

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

            Controller.InterationEvent += DoInterationEvent;
        }

        /// <summary>
        /// MessageCenter 事件反注册
        /// </summary>
        private void UnRegister()
        {
            MessageCenter.Instance.RemoveListener("OnAttackExit", OnAttackExit);
            MessageCenter.Instance.RemoveListener("OnCounterBackExit", OnCounterBackExit);

            Controller.InterationEvent -= DoInterationEvent;
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