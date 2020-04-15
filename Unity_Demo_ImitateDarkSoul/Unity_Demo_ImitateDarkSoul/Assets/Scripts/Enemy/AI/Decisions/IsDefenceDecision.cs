using LS.Test.AI;
using LS.Test.AI.Decisions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    [CreateAssetMenu(menuName = "Souls/EnemyAI/Decision/IsDefence")]
    public class IsDefenceDecision : Decision
    {
        public override bool Decide(FSMBase controller)
        {
            switch ((controller as BlackKnightFSM).FsmStatus)
            {
                case BlackKnightFSM.Stauts.Defence:
                    return true;

                case BlackKnightFSM.Stauts.None:
                case BlackKnightFSM.Stauts.Attack:
                default:
                    return false;
            }
        }
    }

}