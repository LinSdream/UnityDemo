using LS.Test.AI;
using LS.Test.AI.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls.AI
{
    [CreateAssetMenu(menuName = "Souls/EnemyAI/Actions/Walk")]
    public class WalkAction : Action
    {
        [System.Serializable]
        public enum WalkAnimType
        {
            NONE,
            RUN,
            Walk
        }

        public WalkAnimType AnimType;

        public override void Act(FSMBase controller)
        {
            var fsm = controller as BlackKnightFSM;
            switch (AnimType)
            {
                case WalkAnimType.NONE:
                    break;
                case WalkAnimType.RUN:
                    fsm.SetForwardAnimator(2);
                    break;
                case WalkAnimType.Walk:
                    fsm.SetForwardAnimator(1f);
                    break;
            }
        }
    }
}
