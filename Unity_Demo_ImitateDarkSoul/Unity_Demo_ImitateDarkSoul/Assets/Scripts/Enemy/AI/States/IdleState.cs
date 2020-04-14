using LS.Test.AI;
using LS.Test.AI.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls.AI
{
    [CreateAssetMenu(menuName = "Souls/EnemyAI/State/Idle")]
    public class IdleState : State
    {
        public override void OnEnter(FSMBase controller)
        {
            (controller as BlackKnightFSM).SetForwardAnimator(0);
            Debug.Log("!");
        }
    }

}