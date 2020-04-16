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
            fsm.Controller.Attack(fsm.BossCombo);
        }

    }

}