using LS.Test.AI;
using LS.Test.AI.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls.AI
{
    [CreateAssetMenu(menuName = "Souls/EnemyAI/State/Boss/MoveState")]
    public class BossMoveState : State
    {
        public override void OnEnter(FSMBase controller)
        {
            var fsm = controller as BossFSM;
            fsm.BossCol.IssueBool("Walk", true);
            //进入移动状态的时候，停止攻击动画，开启AI寻路
            fsm.BossCombo = 0;
            //攻击动画参数置0
            fsm.BossCol.Attack(0);
            fsm.AI.isStopped = false;
        }



        public override void OnExit(FSMBase controller)
        {
            var fsm = controller as BossFSM;
            fsm.BossCol.IssueBool("Walk", false);
            fsm.AI.isStopped = true;
        }
    }

}