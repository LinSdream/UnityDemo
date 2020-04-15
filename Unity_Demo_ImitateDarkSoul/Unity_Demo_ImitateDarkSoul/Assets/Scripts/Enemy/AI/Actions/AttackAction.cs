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
            if (fsm.AI.isStopped)//如果AI停下并且由Enemies通告可以攻击
            {
                switch (fsm.FsmStatus)//根据状态进行处理，当然确保唯一性，这里只处理攻击
                {
                    case BlackKnightFSM.Stauts.Attack:
                        fsm.Controller.Attack();
                        break;

                    case BlackKnightFSM.Stauts.None:
                    case BlackKnightFSM.Stauts.Defence:
                    default:
                        break;
                }
            }
        }

    }

}