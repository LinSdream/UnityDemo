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

        [HideInInspector] public GameObject RightHandle;
        [HideInInspector] public GameObject LeftHandle;

        [HideInInspector] public WeaponController RightWC;
        [HideInInspector] public WeaponController LeftWC;


        Collider WeaponCollider;

        //AnimatorEvents _weaponAnimatorEvent;

        private void Awake()
        {

            RightHandle = SharedMethods.DeepFindTransform(transform, "WeaponHandle").gameObject;
            LeftHandle = SharedMethods.DeepFindTransform(transform, "ShieldHandle").gameObject;

            LeftWC = BindWeaponController(LeftHandle);
            RightWC = BindWeaponController(RightHandle);

            WeaponCollider = RightHandle.GetComponentsInChildren<Collider>()[0];
        }

        #region Public Methods

        public Collider ChangeWeaponCollider(int index, bool isRight = true)
        {

            if (!isRight)
                WeaponCollider = RightHandle.GetComponentsInChildren<Collider>()[index];
            else
                WeaponCollider = LeftHandle.GetComponentsInChildren<Collider>()[index];
            if (WeaponCollider == null)
                WeaponCollider = ChangeWeaponCollider(0, isRight);
            return WeaponCollider;
        }

        public WeaponController BindWeaponController(GameObject go)
        {
            WeaponController wc;
            wc = go.GetComponent<WeaponController>();
            if (wc == null)
                wc = go.AddComponent<WeaponController>();
            wc.WM = this;
            return wc;
        }

        public void WeaponEnable()
        {
            WeaponCollider.enabled = true;
        }

        public void WeaponDisable()
        {
            WeaponCollider.enabled = false;
        }

        public void CounterBackEnable()
        {
            AM.SetIsCounterBack(true);
        }

        public void CounterBackDisable()
        {
            AM.SetIsCounterBack(false);
        }

        #endregion
    }
}
