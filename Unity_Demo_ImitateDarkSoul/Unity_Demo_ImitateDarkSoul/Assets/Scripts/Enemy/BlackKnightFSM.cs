using LS.Common;
using LS.Test.AI;
using LS.Test.AI.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Souls
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class BlackKnightFSM : FSMBase
    {
        public enum Stauts
        {
            None,
            Attack,
            Defence
        }


        [HideInInspector] public NavMeshAgent AI;
        [HideInInspector] public AIController Controller;
        public Stauts FsmStatus = Stauts.None;
        //权重状态
        public SharedMethods.WeightRandom[] WeightedStatus;
        /// <summary> 当前攻击敌人中，在GameManager中的Enemies的Index </summary>
        public bool IsReady;
        public bool CanAttack;
        public int CurrentEnemiesIndex = -1;
        public State Show;
        //计时器给DefenceState状态使用
        public float TimerForDefence = 0;

        NavMeshTriangulation _navMeshData;
        int _timer;//计时
        int _frame;//延迟帧数

        #region Callbacks
        private void Awake()
        {
            AI = GetComponent<NavMeshAgent>();
            Controller = GetComponent<AIController>();
            AI.speed = Controller.WalkSpeed;
            AI.angularSpeed = Controller.RotationSpeed;

            _navMeshData = NavMesh.CalculateTriangulation();
        }

        protected override void OnStart()
        {
            TargetGameObject = GameObject.Find("Player");
            _frame = Random.Range(0, 2);

            WeightedStatus = new SharedMethods.WeightRandom[3];
            WeightedStatus[0]=new SharedMethods.WeightRandom() { Name = "Attack", Weighted = 5 };//攻击
            WeightedStatus[1]=new SharedMethods.WeightRandom() { Name = "Defence", Weighted = 1 };//防御
            WeightedStatus[2]=new SharedMethods.WeightRandom() { Name = "Stab", Weighted = 0 };//弹反
        }

        protected override void OnUpdate()
        {
            Show = CurrentState;

           if (IsReady)//接收到可以准备的指令后
            {
                if(_timer>=_frame)//延迟n帧后开始攻击
                {
                    switch(SharedMethods.GetWeightedRandomRes(WeightedStatus).Name)//根据权重结果分配具体行为
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
            Controller.Anim.SetFloat("Forward", value);
        }


        /// <summary>判断玩家是否在范围内 </summary>
        /// <param name="sqrDistance">距离的平方</param>
        /// <param name="halfAngle">半角</param>
        public bool IsInArea(float sqrDistance, float halfAngle)
        {
            return SharedMethods.IsInArea(transform, TargetGameObject.transform, sqrDistance, halfAngle);
        }

        /// <summary>获取navmesh上随机的一个点 </summary>
        public Vector3 GetRandomLocaion()
        {

            //每三个相邻顶点构成一个三角网格，数组减去3防止越界
            int trianglePoint = Random.Range(0, _navMeshData.indices.Length - 3);

            //取任意一个三角网格的三点的中点
            Vector3 point = (_navMeshData.vertices[_navMeshData.indices[trianglePoint]] +
                _navMeshData.vertices[_navMeshData.indices[trianglePoint + 1]]
                + _navMeshData.vertices[_navMeshData.indices[trianglePoint + 2]]) / 3;

            return point;
        }
    }

}