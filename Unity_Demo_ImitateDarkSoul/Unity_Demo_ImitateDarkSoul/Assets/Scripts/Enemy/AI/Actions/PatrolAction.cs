using LS.Test.AI;
using LS.Test.AI.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls.AI
{
    [CreateAssetMenu(menuName = "Souls/EnemyAI/Actions/Patrol")]

    public class PatrolAction : Action
    {

        public override void Act(FSMBase controller)
        {
            var fsm = controller as BlackKnightFSM;
            fsm.AI.SetDestination(fsm.PartorlPoints[fsm.PartorlPointsIndex].position);
            if(fsm.AI.remainingDistance<=fsm.AI.stoppingDistance && !fsm.AI.pathPending)
            {
                fsm.PartorlPointsIndex = (fsm.PartorlPointsIndex + 1) % fsm.PartorlPoints.Length;
            }
        }
    }

}