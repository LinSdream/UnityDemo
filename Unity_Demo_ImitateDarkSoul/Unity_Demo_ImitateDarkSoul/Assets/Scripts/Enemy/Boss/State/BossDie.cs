using LS.Test.AI;
using LS.Test.AI.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls.AI
{
    public class BossDie : State
    {
        public override void OnEnter(FSMBase controller)
        {
            var fsm = controller as BossFSM;
            fsm.DistanceType = BossFSM.Distance.None;
            fsm.AI.isStopped = true;

        }
    }

}