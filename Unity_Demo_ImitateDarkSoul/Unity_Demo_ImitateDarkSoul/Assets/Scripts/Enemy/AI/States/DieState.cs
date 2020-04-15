using LS.Test.AI;
using LS.Test.AI.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls.AI
{
    [CreateAssetMenu(menuName = "Souls/EnemyAI/State/Die")]
    public class DieState : State
    {
        //退出的时候要删除
        public override void OnExit(FSMBase controller)
        {
            var fsm = controller as BlackKnightFSM;
            EnemyManager.Instance.RemoveEnemies(fsm.CurrentEnemiesIndex);
            fsm.IsReady = false;
            fsm.CurrentEnemiesIndex = -1;
        }
    }

}