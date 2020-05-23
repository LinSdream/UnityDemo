using LS.Test.AI;
using LS.Test.AI.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls.AI
{
    [CreateAssetMenu(menuName = "Souls/EnemyAI/State/Defence")]
    public class DefenceState : State
    {

        [Tooltip("举盾时长")]
        public float DefenceTime = 3f;
        public State NextState;

        public override void OnEnter(FSMBase controller)
        {
            //举盾
            var fsm = controller as BlackKnightFSM;
            fsm.SetForwardAnimator(0);
            fsm.AICol.Defense(true);
            fsm.AI.isStopped = true;
        }

        public override void OnUpdate(FSMBase controller)
        {
            base.OnUpdate(controller);//保留原来的操作

            //防御处理
            var fsm = controller as BlackKnightFSM;
            fsm.TimerForDefence += Time.deltaTime;
            if (fsm.TimerForDefence >= DefenceTime)
                fsm.TransitionToState(NextState);//转向上一个状态
            else
                fsm.AICol.Defense(true);
        }

        public override void OnExit(FSMBase controller)
        {
            var fsm = controller as BlackKnightFSM;
            //放下盾牌
            fsm.AICol.Defense(false);
            fsm.AI.isStopped = false;
            fsm.TimerForDefence = 0;

        }
    }

}