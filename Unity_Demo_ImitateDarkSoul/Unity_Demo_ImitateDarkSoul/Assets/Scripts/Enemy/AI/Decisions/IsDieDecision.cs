using LS.Test.AI;
using LS.Test.AI.Decisions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls.AI
{
    [CreateAssetMenu(menuName = "Souls/EnemyAI/Decision/IsDie")]
    public class IsDieDecision : Decision
    {
        public override bool Decide(FSMBase controller)
        {
            return (controller as BlackKnightFSM).Controller.CheckAnimatorState("Death");
        }
    }

}