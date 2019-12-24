using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{

    public class WeaponManager
    {
        #region Fields
        private static WeaponManager _instance;
        public static WeaponManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new WeaponManager();
                return _instance;
            }
        }
        public string WeaponPath => "M_ROOT/M_CENTER/BODY1/BODY2/R_CBONE/R_ARM1/R_ARM2/R_Hand/Weapon/";
        public List<GameObject> WeaponList = new List<GameObject>();
        public int CurrentWeapon = 0;

        public int GetChangeWeapon
        {
            get
            {
                if (CurrentWeapon >= WeaponList.Count - 1)
                    CurrentWeapon = 0;
                else
                    CurrentWeapon++;
                return CurrentWeapon;
            }
        }
        #endregion

        #region Public Methods
        public void FindAndAddList()
        {
            GameObject unamred = GameObject.Find(WeaponPath + "Unarmed");
            GameObject sword = GameObject.Find(WeaponPath + "Sword");
            GameObject sickle = GameObject.Find(WeaponPath + "Sickle");

            WeaponList.Add(unamred);
            WeaponList.Add(sword);
            WeaponList.Add(sickle);
        }

        public GameObject GetCurrentWeapon()
        {
            Debug.Log("CurrentWeapon index:" + CurrentWeapon);
            return WeaponList[CurrentWeapon];
        }

        public GameObject GetNextWeapon()
        {
            Debug.Log("NextWeapon" + CurrentWeapon);
            return WeaponList[GetChangeWeapon];
        }
        #endregion

    }
}
