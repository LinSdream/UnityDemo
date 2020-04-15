using LS.Test.AI;
using LS.Test.AI.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls.AI
{
    [CreateAssetMenu(menuName = "Souls/EnemyAI/Actions/Attack")]
    public class AttackAction : Action
    {
        public override void Act(FSMBase controller)
        {
            var fsm = controller as BlackKnightFSM;
            if (fsm.AI.isStopped&&fsm.CanAttack)//如果AI停下并且由Enemies通告可以攻击
            {
                fsm.Controller.Attack();
            }
        }

    }

}