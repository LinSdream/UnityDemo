using LS.Test.AI;
using LS.Test.AI.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls.AI
{
    [CreateAssetMenu(menuName = "Souls/EnemyAI/Actions/Boss/Attack")]
    public class BossAttackAction : Action
    {
        public override void Act(FSMBase controller)
        {
            var fsm = controller as BossFSM;
            if (fsm.BehaviourTimer >= fsm.CurrentBehaviourFrequency)//计时器累加由AttackState来计算，这里就不用计算了
            {
                fsm.BossCol.Attack(fsm.BossCombo);
            }

        }

    }

}