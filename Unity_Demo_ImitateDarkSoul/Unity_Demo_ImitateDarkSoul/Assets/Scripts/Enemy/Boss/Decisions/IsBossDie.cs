using LS.Test.AI;
using LS.Test.AI.Decisions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls.AI
{
    [CreateAssetMenu(menuName = "Souls/EnemyAI/Decision/Boss/IsDie")]
    public class IsBossDie : Decision
    {
        public override bool Decide(FSMBase controller)
        {
            return (controller as BossFSM).Controller.CheckAnimatorState("Death");
        }
    }
}