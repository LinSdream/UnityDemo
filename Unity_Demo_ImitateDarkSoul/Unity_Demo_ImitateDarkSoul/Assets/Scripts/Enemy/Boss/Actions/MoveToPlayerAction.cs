using LS.Test.AI;
using LS.Test.AI.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls.AI
{
    [CreateAssetMenu(menuName = "Souls/EnemyAI/Actions/Boss/MoveToPlayer")]
    public class MoveToPlayerAction : Action
    {
        public override void Act(FSMBase controller)
        {
            var fsm = controller as BossFSM;
            fsm.AI.SetDestination(fsm.TargetGameObject.transform.position);
        }
    }

}