using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LS.InputFramework;

namespace Game
{

    public class PlayerMovement : MonoBehaviour
    {

        #region Structs

        /// <summary> 角色状态 </summary>
        [System.Serializable]
        public struct States
        {
            /// <summary> 是否下蹲 </summary>
            public bool IsCrouch;
            ///<summary>是否在地面上</summary>
            public bool IsInGround;
        }

        /// <summary> 下蹲Or站立的时候的Collider的数据</summary>
        [System.Serializable]
        public struct StandupOrCrouchColliderInfo
        {
            public Vector2 ColliderStandSize;
            public Vector2 ColliderStandOffset;
            public Vector2 ColliderCrouchSize;
            public Vector2 ColliderCrouchOffset;
        }
        #endregion

        #region Public Methods

        /// <summary> 刚体 </summary>
        public Rigidbody2D Body;
        /// <summary> 碰撞器 </summary>
        public BoxCollider2D Coll;
        /// <summary> 输入</summary>
        public PlayerInput UserInput;
        /// <summary>正常的移动速度</summary>
        [Header("移动参数")]
        [Tooltip("正常的移动速度")] public float Speed = 8f;
        /// <summary>下蹲的移动速度 </summary>
        [Tooltip("下蹲的移动速度")] public float CrouchSpeedDivisor = 3f;

        [Header("跳跃设置")]
        [Tooltip("向上的力，注意改Project已经修改过重力的数值了，重力=重力*重力系数(系数在Rigidbody中修改)，现在重力为50，默认9.81")]
        public float JumpForce = 6.3f;
        /// <summary> 跳跃后的持续力 </summary>
        [Tooltip("跳跃后的持续力")] public float JumpHoldForce = 1.9f;
        ///<summary>跳跃的持续时间</summary>
        [Tooltip("跳跃的持续时间")] public float JumpHoldDuration = 0.1f;
        ///<summary>下蹲后的跳跃的额外加成</summary>
        [Tooltip("下蹲后的跳跃的额外加成")] public float CrouchJumpBoost = 2.5f;

        /// <summary>角色状态</summary>
        public States State;
        /// <summary> Collider的数据</summary>
        [HideInInspector]public StandupOrCrouchColliderInfo ColliderSizeInfo;

        #endregion

        #region Private Methods

        /// <summary>x轴速度</summary>
        private float _xVelocity;
        /// <summary> 跳跃时间</summary>
        private float _jumpTime;
        #endregion

        #region Mono Callbacks
        // Start is called before the first frame update
        void Start()
        {
            //Collider赋值
            ColliderSizeInfo.ColliderStandSize = Coll.size;
            ColliderSizeInfo.ColliderStandOffset = Coll.offset;

            ColliderSizeInfo.ColliderCrouchSize = new Vector2(Coll.size.x, Coll.size.y / 2f);
            ColliderSizeInfo.ColliderCrouchOffset = new Vector2(Coll.offset.x, Coll.offset.y / 2f);
        }

        // Update is called once per frame
        void Update()
        {
            GroundMovement();
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// 地面移动
        /// </summary>
        void GroundMovement()
        {
            if (UserInput.IsCrouch)
                Crouch();
            //如果没有按下下蹲键，并且当前处于下蹲状态，自动起立
            else if(!UserInput.IsCrouch && State.IsCrouch){
                StandUp();
            }

            _xVelocity = UserInput.Horizontal;

            if (State.IsCrouch)
                _xVelocity /= CrouchSpeedDivisor;

            Body.velocity = new Vector2(_xVelocity * Speed, Body.velocity.y);
            FilpDirction();
        }

        /// <summary>
        /// 精灵的方向
        /// </summary>
        void FilpDirction()
        {
            if(_xVelocity < 0)
                transform.localScale = new Vector2(-1, 1);
            else if(_xVelocity > 0)//排除0的结果
                transform.localScale = new Vector2(1, 1);

        }

        /// <summary>
        /// 下蹲
        /// </summary>
        void Crouch()
        {
            State.IsCrouch = true;

            Coll.size = ColliderSizeInfo.ColliderCrouchSize;
            Coll.offset = ColliderSizeInfo.ColliderCrouchOffset;
        }

        /// <summary>
        /// 起立
        /// </summary>
        void StandUp()
        {
            State.IsCrouch = false;

            Coll.size = ColliderSizeInfo.ColliderStandSize;
            Coll.offset = ColliderSizeInfo.ColliderStandOffset;
        }
        #endregion

    }

}