using LS.Test.AI;
using LS.Test.AI.Actions;
using Souls.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls.AI
{

    [CreateAssetMenu(menuName = "Souls/EnemyAI/Actions/Trick")]
    public class TrickPlayerAction : Action
    {

        [Tooltip("向量表示范围，平方表示 0-10,10-50，,50-60")]
        public Vector3 Offset = new Vector3(100f, 2500f, 3600f);
        public float SqrDistanceToPlayer = 25f;

        public override void Act(FSMBase controller)
        {

            if (controller.TargetGameObject == null)
                return;
            var fsm = controller as BlackKnightFSM;

            float distance = (fsm.TargetGameObject.transform.position - fsm.transform.position).sqrMagnitude;

            //如果敌人在10米以内
            if (distance <= Offset.x)
            {
                //如果小于5，认为到达目的地，停止
                if (distance < SqrDistanceToPlayer)
                    fsm.AI.isStopped = true;
                else//否则更改为走路
                {
                    fsm.AI.isStopped = false;
                    fsm.AI.speed = fsm.AICol.WalkSpeed;
                    fsm.AI.SetDestination(fsm.TargetGameObject.transform.position);
                    fsm.SetForwardAnimator(1);
                }
            }
            else if (distance > Offset.x && distance <= Offset.y)//如果在10-50，则跑步追上Player
            {
                fsm.AI.isStopped = false;
                fsm.AI.speed = fsm.AICol.WalkSpeed * fsm.AICol.RunMultiplier;
                fsm.AI.SetDestination(fsm.TargetGameObject.transform.position);
                fsm.SetForwardAnimator(2);
            }
            else
                return;
                //fsm.TransitionToState();
        }
    }

}