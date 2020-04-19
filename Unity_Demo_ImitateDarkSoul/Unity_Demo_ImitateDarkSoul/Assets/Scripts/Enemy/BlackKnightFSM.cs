using LS.Common;
using LS.Test.AI;
using LS.Test.AI.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Souls.AI
{
    [System.Serializable]
    public struct WeightedRandomValue : SharedMethods.IWeightedPair
    {
        public string Name;
        public int Value;

        public string WeightedName => Name;
        public int WeightedValue => Value;
    }

    public class BlackKnightFSM : EnemyBaseFSM
    {
        public enum Stauts
        {
            None,
            Attack,
            Defence
        }

          [HideInInspector] public AIController AICol;
        [HideInInspector] public Stauts FsmStatus = Stauts.None;
        //权重状态
        [HideInInspector] public WeightedRandomValue[] WeightedStatus;
        /// <summary> 当前攻击敌人中，在GameManager中的Enemies的Index </summary>
        [HideInInspector] public bool IsReady;
        [HideInInspector] public bool CanAttack;
        [HideInInspector] public int CurrentEnemiesIndex = -1;
        //计时器给DefenceState状态使用
        [HideInInspector] public float TimerForDefence = 0;

        int _timer;//计时
        int _frame;//延迟帧数

        #region Callbacks
        protected override void OnStart()
        {
            AICol = Controller as AIController;
            AI.speed = AICol.WalkSpeed;
            AI.angularSpeed = AICol.RotationSpeed;

            TargetGameObject = GameObject.Find("Player");
            _frame = Random.Range(0, 2);

            CurrentEnemiesIndex = EnemyManager.Instance.AddEnemies(this);
            IsReady = false;

            WeightedStatus = new WeightedRandomValue[3];
            WeightedStatus[0]=new WeightedRandomValue() { Name = "Attack", Value = 10 };//攻击
            WeightedStatus[1]=new WeightedRandomValue() { Name = "Defence", Value = 1 };//防御
            WeightedStatus[2]=new WeightedRandomValue() { Name = "Stab", Value = 0 };//弹反
        }

        protected override void OnUpdate()
        {
            Show = CurrentState;

           if (IsReady)//接收到可以准备的指令后
            {
                if(_timer>=_frame)//延迟n帧后开始攻击
                {
                    switch(SharedMethods.GetWeightedRandomRes(WeightedStatus).WeightedName)//根据权重结果分配具体行为
                    {
                        case "Attack":
                            FsmStatus = Stauts.Attack;
                            break;
                        case "Defence":
                            FsmStatus = Stauts.Defence;
                            break;
                        default:
                            FsmStatus = Stauts.None;
                            break;
                    }
                    //重置
                    _timer = 0;
                    _frame = Random.Range(0, 2);
                }
                else
                {
                    _timer++;
                    FsmStatus = Stauts.None;//状态保存咸鱼
                }
            }
        }

        #endregion
        public void SetForwardAnimator(float value)
        {
            AICol.Anim.SetFloat("Forward", value);
        }

    }

}