using LS.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    /// <summary>
    /// WeaponManager 负责控制双手的WeaponController
    /// </summary>
    public class WeaponManager : MonoBehaviour
    {
        WeaponController RightWC;
        WeaponController LeftWC;

        AnimatorEvents _weaponAnimatorEvent;

        private void Awake()
        {
            _weaponAnimatorEvent = gameObject.GetComponent<BaseController>().Model.GetComponent<AnimatorEvents>();

            RightWC = SharedMethods.DeepFindTransform(transform, "WeaponHandle").GetComponent<WeaponController>();
            LeftWC = SharedMethods.DeepFindTransform(transform, "ShieldHandle").GetComponent<WeaponController>();
        }
    }
}
