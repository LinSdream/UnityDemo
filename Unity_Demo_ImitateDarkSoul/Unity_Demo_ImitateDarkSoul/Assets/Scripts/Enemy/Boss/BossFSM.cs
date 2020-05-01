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
            Short,
            Mid,
            Long,
            VLong,
        }

        #region Fields

        /// <summary> boss站，初始状态机</summary>
        public State BossBattle;
        /// <summary>玩家与Boss的距离，根据不同距离进行不同的出招</summary>
        public Distance DistanceType;
        /// <summary>Boss的Attack类型</summary>
        public int BossCombo = 1;
        [HideInInspector] public BossController BossCol;

        //计时器
        /// <summary> 行动计时器</summary>
        public float BehaviourTimer = 0f;
        /// <summary>当前频率 </summary>
        public float CurrentBehaviourFrequency;
        #endregion

        #region Mono Callbacks


        private void OnEnable()
        {
            BossMessageCenter.Instance.AddListener("BeginBossBattle", BeginBossBattle);
            BossMessageCenter.Instance.AddListener("BossSpeicialAttack", BossSpeicialAttack);
            BossMessageCenter.Instance.AddListener("ResetCombo", ResetCombo);
        }


        protected override void OnStart()
        {
            BossCol = Controller as BossController;
            DistanceType = Distance.None;
            TargetGameObject = GameObject.Find("Player");
            CurrentBehaviourFrequency = UnityEngine.Random.Range(BossCol.BossIF.BehaviourFrequency.x, BossCol.BossIF.BehaviourFrequency.y + 0.01f);//做闭区间
            BossCombo = 1;
        }

        private void OnDisable()
        {
            BossMessageCenter.Instance.RemoveListener("BeginBossBattle", BeginBossBattle);
            BossMessageCenter.Instance.RemoveListener("BossSpeicialAttack", BossSpeicialAttack);
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
            if (BossBattle == null)
                Debug.Log("!");
            TransitionToState(BossBattle);
            AudioManager.Instance.PlayMusic("BossBGM");
            BossCol.BeginBossBattle();
        }

        /// <summary> 攻击动画置0 </summary>
        private void ResetCombo(GameObject render, EventArgs e)
        {
            BossCol.Attack(0);
        }

        private void BossSpeicialAttack(GameObject render, EventArgs e)
        {
            if (BossCol.CheckAnimatorState("Boss_SpecialAttack01"))
            {
                BossCol.SpecialOne.enabled = true;
            }

        }

    }

}