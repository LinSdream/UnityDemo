using LS.Test.AI;
using LS.Test.AI.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls.AI
{
    [CreateAssetMenu(menuName = "Souls/EnemyAI/State/Trick")]
    public class TrickState : State
    {
        ////进入该状态机的时候添加进敌人列表
        //public override void OnEnter(FSMBase controller)
        //{
        //    var fsm = controller as BlackKnightFSM;
        //    fsm.CurrentEnemiesIndex= EnemyManager.Instance.AddEnemies(fsm);
        //    fsm.IsReady = false;
        //}

        ////退出的时候要删除
        //public override void OnExit(FSMBase controller)
        //{
        //    var fsm = controller as BlackKnightFSM;
        //    EnemyManager.Instance.RemoveEnemies(fsm.CurrentEnemiesIndex);
        //    fsm.IsReady = false;
        //    fsm.CurrentEnemiesIndex = -1;
        //}
    }

}