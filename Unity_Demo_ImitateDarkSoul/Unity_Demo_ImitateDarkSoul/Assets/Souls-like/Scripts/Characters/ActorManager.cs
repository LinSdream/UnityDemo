using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class ActorManager : MonoBehaviour
    {
        
        [HideInInspector] public BattleManager BM;
        [HideInInspector] public WeaponManager WM;

        BaseController _controller;

        private void Awake()
        {
            _controller = GetComponent<BaseController>();

            BM = GetComponent<BattleManager>();
            WM = GetComponent<WeaponManager>();
            if(BM==null||WM==null)
            {
                Debug.LogError("ActorManager/Awake Error : can't get BattleManager or WeaponManager in this Actor");
                return;
            }
            BM.AM = this;
        }
       
        public void Damage()
        {
            _controller.Hit(0);
        }
    }

}