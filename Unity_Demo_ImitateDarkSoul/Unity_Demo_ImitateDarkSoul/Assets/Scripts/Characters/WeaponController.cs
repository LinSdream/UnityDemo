using LS.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Souls
{
    public class WeaponController:MonoBehaviour
    {

        public WeaponManager WM;
        public WeaponData Data;

        /// <summary> 武器数量 </summary>
        public List<WeaponData> WeaponList;

        public float GetATK
        {
            get
            {
                return Data.ATK + WM.AM.SM.TempInfo.Damage;
            }
        }

        private void Awake()
        {
            WeaponList = GetComponentsInChildren<WeaponData>().ToList();
            //如果当前没有武器说明是玩家，需要动态加载，否则就是敌人，取0值
            if (WeaponList.Count == 0)
                return;
            Data = WeaponList[0];
        }

        public void AddWeaponData(WeaponData data)
        {
            if(WeaponList.Contains(data))
            {
                Debug.LogWarning("WeaponController/AddWeaponData Warning : the data already exist . the gameObject is " +
                    WM.AM.name);
                return;
            }
            else
            {
                WeaponList.Add(data);
            }
        }

        public WeaponData SetData(int index)
        {
            if (index < WeaponList.Count)
                Data = WeaponList[index];
            else
                Data = WeaponList[0];
            return Data;
        }

        public int WeaponIndex(string name)
        {
            for(int i=0;i<WeaponList.Count;i++)
            {
                if (WeaponList[i].WeaponName == name)
                    return i;
            }
            return -1;
        }

        public void WeaponEnable(int index,bool on)
        {
            WeaponList[index].gameObject.SetActive(on);
        }
    }
}