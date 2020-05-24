using LS.Test.AI;
using LS.Test.AI.Decisions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls.AI
{
    [CreateAssetMenu(menuName = "Souls/EnemyAI/Decision/Boss/BossTrickPlayerByDistance")]
    public class BossTrickPlayerDecison : Decision
    {

        [Tooltip("是否需要移动到最短距离")]
        public bool CanShort;

        public override bool Decide(FSMBase controller)
        {
            var fsm = controller as BossFSM;

            if (fsm.Controller.CheckAnimatorStateTag("Attack"))
                return false;

            switch (fsm.DistanceType)
            {
                case BossFSM.Distance.Mid:
                    if (!CanShort)//按照最短来计算达到要求
                        return true;
                    else
                        return false;
                case BossFSM.Distance.Short:
                    return true;
                case BossFSM.Distance.Long:
                case BossFSM.Distance.None:
                default:
                    return false;
                
            }
        }

    }

}