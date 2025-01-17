﻿using LS.Test.AI;
using LS.Test.AI.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls.AI
{
    [CreateAssetMenu(menuName = "Souls/EnemyAI/State/Boss/Attack")]
    public class BossAttackState : State
    {

        public override void OnEnter(FSMBase controller)
        {
            var fsm = controller as BossFSM;

            //这里就先用Attack1
            fsm.BossCombo = 1;
        }

        public override void OnUpdate(FSMBase controller)
        {
            base.OnUpdate(controller);
            var fsm = controller as BossFSM;

            if (fsm.BehaviourTimer >= fsm.CurrentBehaviourFrequency)
            {
                fsm.BehaviourTimer = 0;
                fsm.CurrentBehaviourFrequency = Random.Range(fsm.BossCol.Freq.x, 
                    fsm.BossCol.Freq.y + 0.02f);//做闭区间
            }
            else
            {
                fsm.BehaviourTimer += Time.deltaTime;
            }

        }

        public override void OnExit(FSMBase controller)
        {
            var fsm = controller as BossFSM;
            fsm.BehaviourTimer = 0f;
            fsm.CurrentBehaviourFrequency = 0f;
        }
    }

}