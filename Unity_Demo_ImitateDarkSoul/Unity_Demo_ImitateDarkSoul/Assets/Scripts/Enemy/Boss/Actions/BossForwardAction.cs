﻿using LS.Test.AI;
using LS.Test.AI.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls.AI
{
    [CreateAssetMenu(menuName = "Souls/EnemyAI/Actions/Boss/BossForward")]
    public class BossForwardAction : Action
    {

        [Tooltip("旋转速度系数x,0< Time.delta * x<=1")]
        public float RotationMultiplier = 1f;

        public override void Act(FSMBase controller)
        {
            var fsm = controller as BossFSM;
            if(fsm.Controller.CheckAnimatorState("Boss_Idle")||fsm.Controller.CheckAnimatorState("Boss_Walk"))
            {
                //转向，面对玩家
                Vector3 lookPos = fsm.TargetGameObject.transform.position - fsm.Controller.transform.position;
                lookPos.y = 0;
                Quaternion rotation = Quaternion.LookRotation(lookPos);
                fsm.Controller.transform.rotation = Quaternion.Slerp(fsm.Controller.transform.rotation, rotation, Time.deltaTime * RotationMultiplier);
            }
        }

    }

}