using LS.Test.AI;
using LS.Test.AI.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls.AI
{

    public class DefenseAction : Action
    {
        public override void Act(FSMBase controller)
        {
            var fsm = controller as BlackKnightFSM;
            fsm.Controller.Defense(true);
        }
    }

}