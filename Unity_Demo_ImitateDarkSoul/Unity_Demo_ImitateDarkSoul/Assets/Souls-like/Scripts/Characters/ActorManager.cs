using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class ActorManager : MonoBehaviour
    {
        
        [HideInInspector] public BattleManager BM;

        BaseController _controller;

        private void Awake()
        {
            _controller = GetComponent<BaseController>();

            BM = GetComponentInChildren<BattleManager>();
            if(BM==null)
            {
                Debug.LogError("ActorManager/Awake Error : can't get BattleManager in this Actor");
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