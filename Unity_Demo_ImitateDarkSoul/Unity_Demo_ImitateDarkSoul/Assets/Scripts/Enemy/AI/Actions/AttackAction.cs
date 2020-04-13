using LS.Test.AI;
using LS.Test.AI.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls.AI.Actions
{
    [CreateAssetMenu(menuName = "Souls/EnemyAI/Actions/Attack")]
    public class AttackAction : Action
    {
        public override void Act(FSMBase controller)
        {
            (controller as AIFSM).Controller.Attack();
        }
    }

}