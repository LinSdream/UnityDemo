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
            public bool IsOnGround;
            ///<summary>是否在跳跃</summary>
            public bool IsJump;
            ///<summary>头顶有障碍物</summary>
            public bool IsHeadBlock;
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
        [Tooltip("向上的力，注意改Project已经修改过重力的数值了，实际重力=重力*重力系数(系数在Rigidbody中修改)，现在重力为50，默认9.81")]
        public float JumpForce = 6.3f;
        /// <summary> 跳跃后的持续力 </summary>
        [Tooltip("跳跃后的持续力")] public float JumpHoldForce = 1.9f;
        ///<summary>跳跃的持续时间</summary>
        [Tooltip("跳跃的持续时间")] public float JumpHoldDuration = 0.1f;
        ///<summary>下蹲后的跳跃的额外加成</summary>
        [Tooltip("下蹲后的跳跃的额外加成")] public float CrouchJumpBoost = 2.5f;


        [Header("其他设置")]
        [Tooltip("环境检测层")] public LayerMask GroundLayer;
        /// <summary> 左右两个脚的距离，Collider的一半(x轴) </summary>
        [HideInInspector] public float FootOffesst;
        /// <summary>头顶的检测距离 </summary>
        [Tooltip("头顶的检测距离")] public float HeadClearance = 0.5f;
        ///<summary>地面的检测距离</summary>
        [Tooltip("地面的检测距离")] public float GroundDistance = 0.2f;

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

            FootOffesst = Coll.size.x / 2f;
        }

        private void FixedUpdate()
        {
            PhysicsCheck();
            GroundMovement();
            MidAirMovement();
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// 地面移动
        /// </summary>
        void GroundMovement()
        {
            if (UserInput.IsCrouch && !State.IsCrouch && State.IsOnGround)
                Crouch();
            //如果没有按下下蹲键，并且当前处于下蹲状态，自动起立
            else if (!UserInput.IsCrouch && State.IsCrouch&&!State.IsHeadBlock)
                StandUp();
            else if (!State.IsOnGround && State.IsCrouch)//如果不在地面，并且在下蹲状态
                StandUp();

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

        void MidAirMovement()
        {
            if(State.IsOnGround&&UserInput.JumpPressed&&!State.IsJump)
            {
                if(State.IsCrouch)
                {
                    StandUp();
                    Body.AddForce(new Vector2(0f, CrouchJumpBoost), ForceMode2D.Impulse);
                }

                State.IsOnGround = false;
                State.IsJump = true;

                _jumpTime = JumpHoldDuration;

                Body.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
            }else if (State.IsJump)
            {
                _jumpTime -= Time.fixedDeltaTime;
                if(UserInput.JumpHeld)
                    Body.AddForce(new Vector2(0f, JumpHoldForce), ForceMode2D.Impulse);
                if (_jumpTime <=0)
                    State.IsJump = false;
            }
        }

        /// <summary>
        /// 物理检测
        /// </summary>
        void PhysicsCheck()
        {

            RaycastHit2D leftFoot = Raycast(new Vector2(-FootOffesst, 0f), Vector2.down, GroundDistance, GroundLayer);
            RaycastHit2D rightFoot = Raycast(new Vector2(FootOffesst, 0f), Vector2.down, GroundDistance, GroundLayer);

            var headCheck = Raycast(new Vector2(0f, Coll.size.y), Vector2.up, HeadClearance, GroundLayer);

            if (leftFoot||rightFoot)
                State.IsOnGround = true;
            else
                State.IsOnGround = false;

            if (headCheck)
                State.IsHeadBlock = true;
            else
                State.IsHeadBlock = false;
        }

        RaycastHit2D Raycast(Vector2 offset,Vector2 rayDiractionNormalized,float length,LayerMask layer)
        {
            Vector2 pos = transform.position;
            RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDiractionNormalized, length, layer);

#if UNITY_EDITOR
            Color color = hit ? Color.red : Color.green;
            Debug.DrawRay(pos + offset, rayDiractionNormalized * length ,color); ;
#endif

            return hit;
        }

        #endregion

    }

}