using LS.Common;
using LS.Helper.Timer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LS.Prop
{
    /// 道具基类
    /// Time:  2020.3.16
    /// Latest Time：2020.3.18
    /// 
    /// Log： 
    /// 1.使用该基类，需要在于道具类发生Trigger的对象上挂载 T 脚本
    /// 2.需要重写Used()方法，该方法是道具效果
    /// 3.道具效果回滚，需要重写Rollback方法
    /// 
    /// <summary>
    /// 道具基类
    /// </summary>
    public abstract class BaseProp<T> : MonoBehaviour where T:Component
    {
        #region Enum PropState
        protected enum PropState
        {
            /// <summary> 初始 </summary>
            Default,
            /// <summary>是否被获取</summary>
            IsCollected,
            /// <summary> 是否过期</summary>
            IsExpiring
        }
        #endregion

        #region Fields

        public PropInfo Info;
        public  string TriggerTag = "Player";

        protected T _playerBrain;
        protected PropState _propState;
        protected Timer _timer;

        #endregion

        #region MonoBehaviour Callbacks

        protected virtual void Start()
        {
            _propState = PropState.Default;
            _timer = new Timer();
        }


        //3D support
        protected void OnTriggerEnter(Collider other)
        {
            PropCollected(other.gameObject);
        }

        //2D support
        protected void OnTriggerEnter2D(Collider2D collision)
        {
            PropCollected(collision.gameObject);
        }

        #endregion

        #region virtual Methods

        /// <summary> 道具Trigger </summary>
        protected virtual void PropCollected(GameObject obj)
        {

            if (!obj.CompareTag(TriggerTag))
                return;

            if (_propState == PropState.IsCollected || _propState == PropState.IsExpiring)
                return;

            _propState = PropState.IsCollected;

            _playerBrain = obj.GetComponent<T>();
            if(_playerBrain==null)
                return;

            ///如果玩家身上已经有同名道具，销毁
            if (SharedMethods.DeepFindTransform(_playerBrain.transform, gameObject.name)!=null)
            {
                AfterDelay();
                return;
            }
            gameObject.transform.parent = _playerBrain.transform;


            PropUsed();
            Effects();

            //作用于UI的事件系统
            foreach (GameObject cell in PropEventSystemListeners.Instance.Listeners)
            {
                ExecuteEvents.Execute<IPropEvents<T>>(cell, null, (prop, player) => prop.OnPropCollected(this, _playerBrain));
            }

            AfterTrigger();
        }


        /// <summary> 道具特效 </summary>
        protected virtual void Effects()
        {
            if (Info.SpecialEffect == null)
                return;
            //特效挂在Player身上
            Instantiate(Info.SpecialEffect, _playerBrain.transform.position, _playerBrain.transform.rotation,_playerBrain.transform);
        }

        /// <summary> 道具使用 </summary>
        protected virtual void PropUsed()
        {
            Used();
            if (Info.Once)
                Expired();
            else
            {
                _timer.Duration = Info.Duration;
                _timer.Callback = Expired;
                StartCoroutine(_timer.TimerForSeconds());
            }
        }

        /// <summary> 发生碰撞以后 </summary>
        protected virtual void AfterTrigger()
        {
            //关闭触发器
            gameObject.GetComponent<Collider>().enabled=false;
            //隐藏道具
            var renderers = GetComponentsInChildren<Renderer>();
            foreach (var cell in renderers)
                cell.enabled = false;
        }

        /// <summary> 道具过期 </summary>
        public virtual void Expired()
        {
            if (_propState == PropState.IsExpiring)
                return;
            _propState = PropState.IsExpiring;
            foreach (var cell in PropEventSystemListeners.Instance.Listeners)
                ExecuteEvents.Execute<IPropEvents<T>>(cell, null, (prop, player) => prop.OnPropExpired(this, _playerBrain));
            RollBack();
            AfterDelay();
        }

        /// <summary> 恢复玩家之前状态 </summary>
        protected virtual void RollBack() { }

        /// <summary> 清理 </summary>
        protected virtual void AfterDelay()
        {
            Destroy(this.gameObject);
        }

        /// <summary>
        /// 说明，返回你想返回的道具信息
        /// </summary>
        public virtual object Description()
        {
            return default;
        }

        #endregion

        /// <summary> 道具使用后的效果 </summary>
        protected abstract void Used();
    }
}
