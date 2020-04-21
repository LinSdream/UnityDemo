using LS.Test.AI;
using LS.Test.AI.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls.AI
{
    [CreateAssetMenu(menuName = "Souls/EnemyAI/Actions/Boss/CalculatePlayerDistance")]
    public class CalculatePlayerDistanceAction : Action
    {

        [Tooltip("玩家距离，三段判定，平方计算")]
        public Vector3 Distance=new Vector3(4,25,100);

        public override void Act(FSMBase controller)
        {
            var fsm = controller as BossFSM;
            if (controller.TargetGameObject == null)
            {
                fsm.DistanceType = BossFSM.Distance.None;
                return;
            }

            float distance = (fsm.TargetGameObject.transform.position - controller.transform.position).sqrMagnitude;

            //设置玩家与Boss的距离
            if (distance <= Distance.x)
                fsm.DistanceType = BossFSM.Distance.Short;
            else if (distance > Distance.x && distance <= Distance.y)
                fsm.DistanceType = BossFSM.Distance.Mid;
            else if (distance > Distance.y && distance <= Distance.z)
                fsm.DistanceType = BossFSM.Distance.Long;
            else if (distance > Distance.z)
                fsm.DistanceType = BossFSM.Distance.VLong;
            else
            {
                fsm.DistanceType = BossFSM.Distance.None;
                Debug.LogWarning("CalcuatePlayerDistanceAction/Act Warning : the distance is an invalid value");
            }
        }

    }

}