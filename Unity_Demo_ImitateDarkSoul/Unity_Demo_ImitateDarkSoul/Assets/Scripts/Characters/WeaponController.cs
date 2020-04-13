using LS.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class WeaponController:MonoBehaviour
    {

        public WeaponManager WM;
        public WeaponData Data;

        public float GetATK
        {
            get
            {
                return Data.ATK + WM.AM.SM.TempInfo.Damage;
            }
        }

        private void Awake()
        {
            Data = GetComponentInChildren<WeaponData>();
        }


    }
}