using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{

    public class WeaponManager
    {
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

    }
}
