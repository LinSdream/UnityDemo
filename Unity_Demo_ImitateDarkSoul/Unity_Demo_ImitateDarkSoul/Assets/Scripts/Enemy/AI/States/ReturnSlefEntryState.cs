using LS.Test.AI;
using LS.Test.AI.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls.AI
{
    [CreateAssetMenu(menuName = "Souls/EnemyAI/State/ReturnSelfEntryState")]
    public class ReturnSlefEntryState : State
    {
        public override void OnEnter(FSMBase controller)
        {
            controller.TransitionToState((controller as BlackKnightFSM).EnterState);
        }
    }

}