using LS.Test.AI;
using LS.Test.AI.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls.AI.States
{
    [CreateAssetMenu(menuName = "Souls/EnemyAI/State/Idel")]
    public class InitState : State
    {
        public override void OnEnter(FSMBase controller)
        {
            (controller as AIFSM).SetForwardAnimator(0);
        }
    }

}