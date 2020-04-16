using LS.Test.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Souls.AI
{
    public class BossFSM : EnemyBaseFSM
    {
        public BossController Controller;

        public int BossCombo = 1;

        protected override void Awake()
        {
            base.Awake();
            Controller = GetComponent<BossController>();
        }

        protected override void OnStart()
        {
            TargetGameObject = GameObject.Find("Player");
            BossCombo = 1;
        }

    }

}