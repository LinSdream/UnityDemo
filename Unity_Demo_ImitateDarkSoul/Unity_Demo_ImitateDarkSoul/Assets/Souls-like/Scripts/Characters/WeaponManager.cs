using LS.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    /// <summary>
    /// WeaponManager 负责控制双手的WeaponController
    /// </summary>
    public class WeaponManager : AbstractActorManager
    {
        #region Fields

        //左右手的Handle
        [HideInInspector] public GameObject RightHandle;
        [HideInInspector] public GameObject LeftHandle;

        //左右手的WeaponController 
        [HideInInspector] public WeaponController RightWC;
        [HideInInspector] public WeaponController LeftWC;

        //启用的武器攻击collider
        Collider WeaponCollider;

        //AnimatorEvents _weaponAnimatorEvent;

        #endregion

        private void Awake()
        {

            RightHandle = SharedMethods.DeepFindTransform(transform, "WeaponHandle").gameObject;
            LeftHandle = SharedMethods.DeepFindTransform(transform, "ShieldHandle").gameObject;

            LeftWC = BindWeaponController(LeftHandle);
            RightWC = BindWeaponController(RightHandle);

            WeaponCollider = RightHandle.GetComponentsInChildren<Collider>()[0];
        }

        #region Public Methods


        /// <summary>
        /// 更改启用的武器
        /// </summary>
        /// <param name="index">第几把武器</param>
        /// <param name="isRight">是否是右手</param>
        public Collider ChangeWeaponCollider(int index, bool isRight = true)
        {
            //右手
            if (isRight)
                WeaponCollider = RightHandle.GetComponentsInChildren<Collider>()[index];
            else//左手
                WeaponCollider = LeftHandle.GetComponentsInChildren<Collider>()[index];
            if (WeaponCollider == null)//如果获取到的Collider为空，说明手下没有武器，默认更改为第一个武器
                WeaponCollider = ChangeWeaponCollider(0, isRight);
            return WeaponCollider;
        }

        /// <summary> 绑定武器的WeaponController </summary>
        public WeaponController BindWeaponController(GameObject go)
        {
            WeaponController wc;
            wc = go.GetComponent<WeaponController>();
            if (wc == null)
                wc = go.AddComponent<WeaponController>();
            wc.WM = this;
            return wc;
        }

        #endregion

        #region Animation Events
        /// <summary> 武器启用 </summary>
        public void WeaponEnable()
        {
            WeaponCollider.enabled = true;
        }

        /// <summary> 武器禁用 </summary>
        public void WeaponDisable()
        {
            WeaponCollider.enabled = false;
        }
        /// <summary> 弹反启用 </summary>
        public void CounterBackEnable()
        {
            AM.SetIsCounterBack(true);
        }
        /// <summary> 弹反禁用 </summary>
        public void CounterBackDisable()
        {
            AM.SetIsCounterBack(false);
        }
        #endregion

    }
}
