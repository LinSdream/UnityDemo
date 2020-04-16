using LS.Common;
using LS.Test.AI;
using LS.Test.AI.States;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Souls.AI
{
    public class BossFSM : EnemyBaseFSM
    {

        public enum Distance
        {
            None,
            Mid,
            Long,
            Short
        }

        #region Fields

        /// <summary> boss站，初始状态机</summary>
        public State BossBattle;
        /// <summary>玩家与Boss的距离，根据不同距离进行不同的出招</summary>
        public Distance DistanceType;
        /// <summary>Boss的Attack类型</summary>
        public int BossCombo = 1;
        /// <summary> Boss的行动频率</summary>
        [Tooltip("行动频率，[min,max]")]public Vector2 BehaviourFrequency = new Vector2(5f, 10f);
        [HideInInspector] public BossController Controller;
        /// <summary>招式权重 </summary>
        [HideInInspector] public List<SharedMethods.WeightRandom> Weighted = new List<SharedMethods.WeightRandom>();
        /// <summary> 行动计时器</summary>
        public float BehaviourTimer = 0f;
        public float CurrentBehaviourFrequency;
        #endregion

        #region Mono Callbacks

        protected override void Awake()
        {
            base.Awake();
            Controller = GetComponent<BossController>();

            //之后作为ScriptableObject或者Json，这里先用来测试
            Weighted.Add(new SharedMethods.WeightRandom() { WeightedName = "Attack1", Weighted = 5 });
            Weighted.Add(new SharedMethods.WeightRandom() { WeightedName = "Attack2", Weighted = 3 });
            Weighted.Add(new SharedMethods.WeightRandom() { WeightedName = "Attack3", Weighted = 3 });
         
        }

        private void OnEnable()
        {
            BossMessageCenter.Instance.AddListener("BeginBossBattle", BeginBossBattle);
            BossMessageCenter.Instance.AddListener("ResetCombo", ResetCombo);
        }


        protected override void OnStart()
        {
            DistanceType = Distance.None;
            TargetGameObject = GameObject.Find("Player");
            CurrentBehaviourFrequency = UnityEngine.Random.Range(BehaviourFrequency.x, BehaviourFrequency.y + 0.01f);//做闭区间
            BossCombo = 1;
        }

        private void OnDisable()
        {
            BossMessageCenter.Instance.RemoveListener("BeginBossBattle", BeginBossBattle);
            BossMessageCenter.Instance.RemoveListener("ResetCombo", ResetCombo);
        }

        protected override void OnUpdate()
        {
            Show = CurrentState;
        }

        #endregion

        /// <summary> 通过消息来进入boss战状态 </summary>
        private void BeginBossBattle(GameObject render, EventArgs e)
        {
            TransitionToState(BossBattle);
        }

        /// <summary> 攻击动画置0 </summary>
        private void ResetCombo(GameObject render, EventArgs e)
        {
            Controller.Attack(0);
        }

    }

}