using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlayerAnimManager
    {
        #region Instance
        private static PlayerAnimManager _instance;
        public static PlayerAnimManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PlayerAnimManager();
                return _instance;
            }
        }
        #endregion

        #region Public Fields

        #region AnimatorClip Name
        public string Attack01;
        public string Attack02;
        public string Attack03;
        public string Death;
        public string GetHit;
        public string Idle;
        public string Roll;
        public string Run;
        public string SpecialAttack;
        public string Unsheath;
        #endregion

        #region Animator Parameters
        public string Parameters_PlayerActionCMD = "ActionCMD";
        public string Parameters_Running;
        public string Parameters_SpecialAttack;
        public string Parameters_SwitchWeapon;
        #endregion

        #region Player Default Animator Clip Name
        public string Drink = "Player_Drinking";
        public string EmptyDrink = "Player_EmptyDrinking";
        public string Sitting = "Player_Sitting";
        #endregion
        #endregion

        #region Public Methods
        public void GetAnimByWeaponName(string name)
        {
            Attack01 = name + "-Attack01";
            Attack02 = name + "-Attack02";
            Attack03 = name + "-Attack03";
            Death = name + "-Death";
            GetHit = name + "-GetHit";
            Idle = name + "-Idle";
            Roll = name + "-Roll";
            Run = name + "-Run";
            SpecialAttack = name + "-SpecialAttack";
            Unsheath = name + "-Unsheath";
        }
        #endregion
    }

}