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
            if (fsm.BehaviourTimer >= fsm.CurrentBehaviourFrequency)
            {
                fsm.Controller.Attack(fsm.BossCombo);
                fsm.BehaviourTimer = 0;
                fsm.CurrentBehaviourFrequency = Random.Range(fsm.BehaviourFrequency.x, fsm.BehaviourFrequency.y + 0.01f);//做闭区间
            }
            else
            {
                fsm.BehaviourTimer += Time.deltaTime;
            }

        }

    }

}