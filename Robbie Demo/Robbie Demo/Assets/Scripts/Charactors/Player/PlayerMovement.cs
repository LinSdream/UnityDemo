using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LS.InputFramework;
using LS.Test.Others;

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
            ///<summary>是否悬挂</summary>
            public bool IsHanging;

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

        #region Methods

        /// <summary> 刚体 </summary>
        [Header("Base")]
        public Rigidbody2D Body;
        /// <summary> 碰撞器 </summary>
        public BoxCollider2D Coll;
        /// <summary> 输入</summary>
        public PlayerInput UserInput;
        /// <summary>正常的移动速度</summary>
        [Header("移动参数")]
        [Tooltip("正常的移动速度")] public float Speed = 8f;
        /// <summary>x轴速度</summary>
        [HideInInspector] public float XVelocity;
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
        /// <summary> 悬挂后跳上平台所施加的力 </summary>
        [Tooltip("悬挂后跳上平台所施加的力")] public float HangingJumpForce = 15f;
        /// <summary> 悬挂后的下一个操作的缓冲时间 </summary>
        [Tooltip("悬挂后的下一个操作的缓冲时间")] public float NextActionAfterHangingCacheTime = 0.05f;
        /// <summary> 悬挂后的下一个操作的缓冲时间 </summary>
        private float _nextActionAfterHangingCacheTime;

        [Header("其他设置")]
        [Tooltip("环境检测层")] public LayerMask GroundLayer;
        /// <summary> 左右两个脚的距离，Collider的一半(x轴) </summary>
        [Tooltip("左右两个脚的距离，一般为Collider的一半(x轴)")] public float FootOffesst = 0.4f;
        /// <summary>头顶的检测距离 </summary>
        [Tooltip("头顶的检测距离")] public float HeadClearance = 0.5f;
        ///<summary>地面的检测距离</summary>
        [Tooltip("地面的检测距离")] public float GroundDistance = 0.2f;
        ///<summary>角色高度</summary>
        private float _playerHeight;
        ///<summary>眼睛的高度</summary>
        [Tooltip("眼睛的高度")] public float EyesHeight = 1.5f;
        ///<summary>判断墙的距离，能够挂再墙上</summary>
        [Tooltip("判断墙的距离，能够挂再墙上")] public float GrabDistance = 0.4f;
        ///<summary>到达墙面的距离</summary>
        [Tooltip("到达墙面的距离")] public float ReachOffset = 0.7f;
        ///<summary>挂墙的射线的偏移量</summary>
        [Tooltip("挂墙的射线的偏移量")] public float CheckHangingRayOffset = -0.05f;
        ///<summary>悬挂的时候的偏移量</summary>
        [Tooltip("悬挂的时候的偏移量")] public float HaningOffset = -0.05f;

        /// <summary>角色状态</summary>
        public States State;
        /// <summary> Collider的数据</summary>
        private StandupOrCrouchColliderInfo _colliderSizeInfo;
        /// <summary> 跳跃时间</summary>
        private float _jumpTime;

        #endregion

        #region Mono Callbacks
        // Start is called before the first frame update
        void Start()
        {
            //Collider赋值
            _colliderSizeInfo.ColliderStandSize = Coll.size;
            _colliderSizeInfo.ColliderStandOffset = Coll.offset;

            _colliderSizeInfo.ColliderCrouchSize = new Vector2(Coll.size.x, Coll.size.y / 2f);
            _colliderSizeInfo.ColliderCrouchOffset = new Vector2(Coll.offset.x, Coll.offset.y / 2f);

            _playerHeight = Coll.size.y;
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
            if (State.IsHanging)
                return;

            if (UserInput.IsCrouch && !State.IsCrouch && State.IsOnGround)
                Crouch();
            //如果没有按下下蹲键，并且当前处于下蹲状态，自动起立
            else if (!UserInput.IsCrouch && State.IsCrouch && !State.IsHeadBlock)
                StandUp();
            else if (!State.IsOnGround && State.IsCrouch)//如果不在地面，并且在下蹲状态
                StandUp();

            XVelocity = UserInput.Horizontal;

            if (State.IsCrouch)
                XVelocity /= CrouchSpeedDivisor;

            Body.velocity = new Vector2(XVelocity * Speed, Body.velocity.y);
            FilpDirction();
        }

        /// <summary>
        /// 精灵的方向
        /// </summary>
        void FilpDirction()
        {
            if (XVelocity < 0)
                transform.localScale = new Vector3(-1, 1, 1);
            else if (XVelocity > 0)//排除0的结果
                transform.localScale = new Vector3(1, 1, 1);

        }

        /// <summary>
        /// 下蹲
        /// </summary>
        void Crouch()
        {
            State.IsCrouch = true;

            //更改Collider
            Coll.size = _colliderSizeInfo.ColliderCrouchSize;
            Coll.offset = _colliderSizeInfo.ColliderCrouchOffset;
        }

        /// <summary>
        /// 起立
        /// </summary>
        void StandUp()
        {
            State.IsCrouch = false;

            //重置Collider
            Coll.size = _colliderSizeInfo.ColliderStandSize;
            Coll.offset = _colliderSizeInfo.ColliderStandOffset;
        }

        void MidAirMovement()
        {

            if (State.IsHanging)
            {
                _nextActionAfterHangingCacheTime -= Time.deltaTime;
                //如果两个数字相减小于1说明两个数字的符号相同，表示方向相同
                //只有同方向且经过了操作缓存才可以上一个平台
                if (UserInput.Horizontal != 0 && Mathf.Abs(transform.localScale.x - UserInput.Horizontal) < 1 && _nextActionAfterHangingCacheTime<0)
                {
                    Body.bodyType = RigidbodyType2D.Dynamic;
                    Body.AddForce(new Vector2(0f, HangingJumpForce), ForceMode2D.Impulse);
                    State.IsHanging = false;
                    _nextActionAfterHangingCacheTime = 0;
                }
                if (UserInput.CrouchPressed || Mathf.Abs(transform.localScale.x - UserInput.Horizontal)>1)
                {
                    Body.bodyType = RigidbodyType2D.Dynamic;
                    State.IsHanging = false;
                    _nextActionAfterHangingCacheTime = 0;
                }
            }

            //如果在地面且按下跳跃键，而且不处于正在跳跃状态
            if (State.IsOnGround && UserInput.JumpPressed && !State.IsJump&&!State.IsHeadBlock)
            {
                if (State.IsCrouch)//如果下蹲
                {
                    StandUp();
                    Body.AddForce(new Vector2(0f, CrouchJumpBoost), ForceMode2D.Impulse);
                }

                State.IsOnGround = false;
                State.IsJump = true;

                _jumpTime = JumpHoldDuration;

                Body.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);

                //播放音效
                AudioManager.Instance.PlaySFX("Jump");
                AudioManager.Instance.PlaySFX("Jump Voice");

            }
            else if (State.IsJump)//如果是在跳跃状态，实际上是完成蓄力跳的功能
            {
                _jumpTime -= Time.fixedDeltaTime;//跳跃时间内
                if (UserInput.JumpHeld)//持续加力
                    Body.AddForce(new Vector2(0f, JumpHoldForce), ForceMode2D.Impulse);
                if (_jumpTime <= 0)
                    State.IsJump = false;
            }
        }

        /// <summary>
        /// 物理检测
        /// </summary>
        void PhysicsCheck()
        {
            //左右脚
            RaycastHit2D leftFoot = Raycast(new Vector2(-FootOffesst, 0f), Vector2.down, GroundDistance, GroundLayer);
            RaycastHit2D rightFoot = Raycast(new Vector2(FootOffesst, 0f), Vector2.down, GroundDistance, GroundLayer);

            //头部
            var headCheck = Raycast(new Vector2(0f, Coll.size.y), Vector2.up, HeadClearance, GroundLayer);

            if (leftFoot || rightFoot)
                State.IsOnGround = true;
            else
                State.IsOnGround = false;

            if (headCheck)
                State.IsHeadBlock = true;
            else
                State.IsHeadBlock = false;

            //角色射线检测的方向，左右方向
            float direction = transform.localScale.x;
            Vector2 grabDir = new Vector2(direction, 0f);

            //头顶射线
            var blockedCheck = Raycast(new Vector2((FootOffesst + CheckHangingRayOffset) * direction, _playerHeight), grabDir, GrabDistance, GroundLayer);
            //眼部射线
            var wallCheck = Raycast(new Vector2((FootOffesst + CheckHangingRayOffset) * direction, EyesHeight), grabDir, GrabDistance, GroundLayer);
            //墙壁挂载的射线检测
            var ledgeCheck = Raycast(new Vector2(ReachOffset * direction, _playerHeight), Vector2.down, GrabDistance, GroundLayer);

            if (!State.IsOnGround && Body.velocity.y < 0 && ledgeCheck && wallCheck && !blockedCheck)
            {
                //悬挂的时候固定位置
                Vector2 pos = transform.position;
                pos.x += (wallCheck.distance + HaningOffset) * direction;//x轴方向的固定
                pos.y -= ledgeCheck.distance;//y轴方向的固定
                transform.position = pos;

                //Body.bodyType = RigidbodyType2D.Kinematic;
                Body.bodyType = RigidbodyType2D.Static;
                _nextActionAfterHangingCacheTime = NextActionAfterHangingCacheTime;
                State.IsHanging = true;
            }

        }

        RaycastHit2D Raycast(Vector2 offset, Vector2 rayDiractionNormalized, float length, LayerMask layer)
        {
            Vector2 pos = transform.position;
            RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDiractionNormalized, length, layer);

#if UNITY_EDITOR
            Color color = hit ? Color.red : Color.green;
            Debug.DrawRay(pos + offset, rayDiractionNormalized * length, color); ;
#endif

            return hit;
        }

        #endregion

    }

}