using LS.Test.AI;
using LS.Test.AI.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls.AI.Actions
{
    [CreateAssetMenu(menuName = "Souls/EnemyAI/Actions/Move")]
    public class MoveAction : Action
    {
        public override void Act(FSMBase controller)
        {
            var ai = (controller as AIFSM).AI;
            if (controller.TargetGameObject != null)
                ai.SetDestination(controller.TargetGameObject.transform.position);
            
        }
    }

}