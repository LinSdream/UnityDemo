using LS.Test.AI;
using LS.Test.AI.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls.AI
{
    [CreateAssetMenu(menuName = "Souls/EnemyAI/State/Boss/Attack")]
    public class BossAttack : State
    {

        public override void OnEnter(FSMBase controller)
        {
            var fsm = controller as BossFSM;

            //这里就先用Attack1
            fsm.BossCombo = 1;
        }

        public override void OnExit(FSMBase controller)
        {
            var fsm = controller as BossFSM;
            fsm.BehaviourTimer = 0f;
            fsm.CurrentBehaviourFrequency = 0f;
        }
    }

}