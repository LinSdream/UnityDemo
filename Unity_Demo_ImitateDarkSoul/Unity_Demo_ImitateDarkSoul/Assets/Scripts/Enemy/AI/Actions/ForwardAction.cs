using LS.Test.AI;
using LS.Test.AI.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    [CreateAssetMenu(menuName = "Souls/EnemyAI/Actions/Forward")]
    public class ForwardAction : Action
    {
        public override void Act(FSMBase controller)
        {
            var fsm = (controller as BlackKnightFSM);
            if(fsm.AI.isStopped)
            {
                fsm.Controller.Rotation(fsm.TargetGameObject.transform);
            }
        }
    }

}