using LS.Test.AI;
using LS.Test.AI.Decisions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls.AI
{
    [CreateAssetMenu(menuName = "Souls/EnemyAI/Decision/IfHit")]
    public class IfHitDecision : Decision
    {
        public override bool Decide(FSMBase controller)
        {
            var fsm = controller as BlackKnightFSM;
            return fsm.AICol.CheckAnimatorState("Hit");
        }
    }

}