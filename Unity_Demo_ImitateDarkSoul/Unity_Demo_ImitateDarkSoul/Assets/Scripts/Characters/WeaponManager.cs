using LS.Common;
using LS.Common.Message;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] Collider _weaponCollider;
        WeaponUIEventArgs _args = new WeaponUIEventArgs();
        List<Collider> _rightColliders = new List<Collider>();
        List<Collider> _leftColliders = new List<Collider>();
        //AnimatorEvents _weaponAnimatorEvent;

        #endregion

        #region Public Methods

        private void Awake()
        {
            RightHandle = SharedMethods.DeepFindTransform(transform, "WeaponHandle").gameObject;
            LeftHandle = SharedMethods.DeepFindTransform(transform, "ShieldHandle").gameObject;

            LeftWC = BindWeaponController(LeftHandle);
            RightWC = BindWeaponController(RightHandle);

            var colliders = RightHandle.GetComponentsInChildren<Collider>();
            if (colliders.Length == 0)
                return;
            _weaponCollider = colliders[0];

            _rightColliders = colliders.ToList();
            _leftColliders = LeftHandle.GetComponentsInChildren<Collider>().ToList();
        }

        /// <summary>
        /// 更改启用的武器
        /// </summary>
        /// <param name="index">第几把武器</param>
        /// <param name="isRight">是否是右手</param>
        public Collider ChangeWeaponCollider(int index, bool isRight = true)
        {
            SetWeaponData(index, isRight);
            //右手
            if (isRight)
                _weaponCollider = _rightColliders[index];
            else//左手
                _weaponCollider = _leftColliders[index];
            if (_weaponCollider == null)//如果获取到的Collider为空，说明手下没有武器，默认更改为第一个武器
                _weaponCollider = ChangeWeaponCollider(0, isRight);

            if (isRight)
                _args.WeaponName = RightWC.Data.WeaponName;
            else
                _args.WeaponName = LeftWC.Data.WeaponName;

            MessageCenter.Instance.SendMessage("WeaponUIChange",AM.gameObject,_args);

            return _weaponCollider;
        }

        public WeaponData SetWeaponData(int index, bool isRight = true)
        {
            if (isRight)
                return RightWC.SetData(index);
            else
                return LeftWC.SetData(index);
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

        public int GetWeaponIndex(string name,bool isRight=true)
        {
            if (isRight)
                return RightWC.WeaponIndex(name);
            else
                return LeftWC.WeaponIndex(name);
        }

        public void AddWeaponCollider(Collider collider,bool isRight=true)
        {
            if (isRight)
                _rightColliders.Add(collider);
            else
                _leftColliders.Add(collider);
        }

        #endregion

        #region Animation Events
        /// <summary> 武器启用 </summary>
        public void WeaponEnable()
        {
            if (_weaponCollider == null)
                return;
            _weaponCollider.enabled = true;
        }

        /// <summary> 武器禁用 </summary>
        public void WeaponDisable()
        {
            _weaponCollider.enabled = false;
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
