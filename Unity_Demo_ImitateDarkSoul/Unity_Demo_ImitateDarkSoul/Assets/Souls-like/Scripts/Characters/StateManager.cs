using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{


    #region Struct CharacterTempInfo

    /// <summary>
    /// 暂存角色的数据信息
    /// </summary>
    [System.Serializable]
    public struct CharacterTempInfo
    {
        public float HP;
        public float Damage;

        public CharacterTempInfo(CharacterInfo info)
        {
            HP = info.HP;
            Damage = info.Damage;
        }

    }

    #endregion

    #region Sturct CharacterStateFlag


    /// <summary>
    /// 角色状态
    /// </summary>
    [System.Serializable]
    public class CharacterStateFlag
    {
        public bool IsGround;
        public bool IsJump;
        public bool IsFall;
        public bool IsRoll;
        public bool IsJob;
        public bool IsAttack;
        public bool IsDie;
        public bool IsHit;
        public bool IsBlock;
        public bool IsDefence;

        //特殊状态
        public bool IsAllowDefence;
        public bool IsImmortal;//是否无敌
        public bool IsCounterBack;// related to state
        public bool IsCounterBackEnable; // related to animation events
        public bool IsCounterBackSuccess;
        public bool isCounterBackFailure;
    }

    #endregion

    /// <summary>
    /// 数值状态管理
    /// </summary>
    public class StateManager : AbstractActorManager
    {
        [HideInInspector] public CharacterInfo Info;
        public CharacterTempInfo TempInfo;

        public CharacterStateFlag CharacterState;

        #region MonoBehaviour Callbacks

        private void Start()
        {
            CharacterState = new CharacterStateFlag();
        }

        private void Update()
        {
            GetState();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 生命值操作
        /// </summary>
        public void AddHP(float value)
        {
            TempInfo.HP += value;
            TempInfo.HP = Mathf.Clamp(TempInfo.HP, 0, Info.HP);

            AM.SetAnimAfterDoDamg(TempInfo.HP);
        }

        public void Test()
        {
            Debug.Log(TempInfo.HP);
        }

        #endregion


        #region Private Methods

        public void GetState()
        {
            CharacterState.IsAttack = AM.Controller.CheckAnimatorStateTag("AttackRTag")|| AM.Controller.CheckAnimatorStateTag("AttackLTag");
            CharacterState.IsBlock = AM.Controller.CheckAnimatorState("Blocked");
            CharacterState.IsDie = AM.Controller.CheckAnimatorState("Death");
            CharacterState.IsFall = AM.Controller.CheckAnimatorState("Fall");
            CharacterState.IsGround = AM.Controller.CheckAnimatorState("Ground");
            CharacterState.IsHit = AM.Controller.CheckAnimatorState("Hit");
            CharacterState.IsJob = AM.Controller.CheckAnimatorState("Job");
            CharacterState.IsJump = AM.Controller.CheckAnimatorState("Jump");
            CharacterState.IsRoll = AM.Controller.CheckAnimatorState("Roll");
            
            CharacterState.IsAllowDefence = CharacterState.IsGround || CharacterState.IsBlock;
            CharacterState.IsDefence = CharacterState.IsAllowDefence && AM.Controller.CheckAnimatorState("Defanse_OneHand", "Defanse Layer");

            CharacterState.IsImmortal = CharacterState.IsRoll || CharacterState.IsJob;

            CharacterState.IsCounterBack = AM.Controller.CheckAnimatorState("CounterBack");
            CharacterState.IsCounterBackSuccess = CharacterState.IsCounterBackEnable;
            CharacterState.isCounterBackFailure = CharacterState.IsCounterBack && !CharacterState.IsCounterBackEnable;

        }

        #endregion
    }

}