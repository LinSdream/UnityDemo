using LS.Test.AI;
using LS.Test.AI.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    [CreateAssetMenu(menuName = "Souls/EnemyAI/State/Defence")]
    public class DefenceState : State
    {

        [Tooltip("举盾时长")]
        public float DefenceTime = 5f;

        public override void OnEnter(FSMBase controller)
        {
            //举盾
            (controller as BlackKnightFSM).Controller.Defense(true);
        }

        public override void OnUpdate(FSMBase controller)
        {
            base.OnUpdate(controller);//保留原来的操作

            //防御处理
            var fsm = controller as BlackKnightFSM;
            fsm.TimerForDefence += Time.deltaTime;
            if (fsm.TimerForDefence >= DefenceTime)
                fsm.TransitionToState(fsm.PreviousState);//转向上一个状态
            else
                fsm.Controller.Defense(true);
        }

        public override void OnExit(FSMBase controller)
        {
            //放下盾牌
            (controller as BlackKnightFSM).Controller.Defense(false);
            (controller as BlackKnightFSM).TimerForDefence = 0;

        }
    }

}