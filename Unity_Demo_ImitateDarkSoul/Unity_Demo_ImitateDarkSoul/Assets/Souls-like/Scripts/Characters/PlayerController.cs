using LS.Common.Math;
using LS.Common.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    [RequireComponent(typeof(UserInput))]
    public class PlayerController : BaseController
    {
        #region Public Fields
        [Tooltip("跳跃速度(垂直向上)")] public float JumpVerlocity;
        [Tooltip("翻滚速度")] public Vector2 RollVelocity;
        [Tooltip("后跳速度,x分量为up，y分量为back")] public Vector2 JabVerlocity;
        [Tooltip("高处落地硬值以落地速度计算")] public float HightFallStiff = 5f;
        [Tooltip("从高处落下后死亡高度，以落地速度计算")] public float HightFallDead = 15f;
        [Tooltip("持续状态的冲量系数")] [Range(0, 1)] public float DurationThrustMultiplier;

        public CameraController CameraCol;
        /// <summary> 锁定平面位移量，跳跃的时候，不更改Move Direction </summary>
        [HideInInspector] public bool LockPlanar = false;
        /// <summary>  Root Motion DeltaPosition </summary>
        [HideInInspector] public Vector3 DeltaPos;
        
        /// <summary> 是否锁定跟踪目标，Model的forward指向Target</summary>
        [HideInInspector] public bool TrackDirection = false;
        /// <summary>临时冲量</summary>
        [HideInInspector] public Vector3 ThrustVec;
        /// <summary> 模型的正前方</summary>
        [HideInInspector] public Vector3 Forward => Model.transform.forward;

        /// <summary> 是否处于跑步状态 </summary>
        public bool IsRun
        {
            get
            {
                float val = _anim.GetFloat("Forward");
                if (val > 1)
                    return true;
                else
                    return false;
            }
        }
        #endregion

        #region Private Fields
        
        UserInput _input;

        ///TODO:武器系统暂时未制作，左手先进行模拟武器系统
        /// <summary> 左手是否盾牌</summary>
        [SerializeField] bool _leftIsShield = true;
        /// <summary>
        /// 左手是否是盾牌，-1 双手无盾牌； 0:左手盾 ； 1 ：右手盾
        /// </summary>
        public int LeftIsShield
        {
            get
            {
                if (_leftIsShield)
                    return 0;
                return -1;
            }
        }
        /// <summary> 位移方向向量 </summary>
        Vector3 _moveDir;

        float _sqrHightFallStiff;
        float u;
        float v;
        #endregion

        #region MonoBehaviour Callbacks
        protected override void Awake()
        {
            base.Awake();

            _input = GetComponent<UserInput>();
            _sqrHightFallStiff = HightFallStiff * HightFallStiff;
        }

        private void Update()
        {
            if (_input.IsLockOn)
                CameraCol.CameraLockOn();

            Jump();

            CalculateMoveDirection();

            if (_leftIsShield)
                Defense();
            Attack();
        }

        private void FixedUpdate()
        {
            Move();
        }

        #endregion

        #region Override Methods
        protected override void AnimatorUpdate()
        {
            SharedMethods.SquareToDiscMapping(_input.Horizontal, _input.Vertical, out u, out v);
            if (CameraCol.LockState)
            {
                _anim.SetFloat("Forward", (u * u + v * v) * Mathf.Lerp(_anim.GetFloat("Forward")
                    , (_input.Vertical > 0 ? 1 : -1f) * (_input.IsRun ? 2f : 1f), 0.5f));

                _anim.SetFloat("Right", (u * u + v * v) * Mathf.Lerp(_anim.GetFloat("Right")
                    , (_input.Horizontal > 0 ? 1 : -1f) * (_input.IsRun ? 2f : 1f), 0.5f));
            }
            else
            {
                //取x2+y2=1范围内的0-1的值
                _anim.SetFloat("Forward",
                   (u * u + v * v)
                    * Mathf.Lerp(_anim.GetFloat("Forward"), (_input.IsRun && !_input.IsDefense ? 2f : 1f), 0.5f));
                _anim.SetFloat("Right", 0);
            }

        }

        /// <summary>
        /// 攻击
        /// </summary>
        public override void Attack()
        {

            if (_input.IsAttack && (CheckAnimatorState("Ground") || CheckAnimatorStateTag("AttackTag")) && IsGrounded)
            {
                if (_input.IsLeftTrigger && !_leftIsShield)
                {
                    _anim.SetBool("AttackMirror", true);
                    _anim.SetTrigger("Attack");
                }
                else if (_input.IsRightTrigger)
                {
                    _anim.SetTrigger("Attack");
                    _anim.SetBool("AttackMirror", false);
                }

            }
        }

        public override void Hit(float value)
        {
            _anim.SetTrigger("Hit");
        }
        #endregion

        #region Private Methdos


        /// <summary> 旋转 </summary>
        void Rotation()
        {
            ///TODO:可以进一步优化
            //角色旋转
            if (_input.Horizontal != 0 || _input.Vertical != 0)
            {
                var forward = Vector3.Scale(CameraCol.CameraObj.forward, new Vector3(1, 0, 1)) * _input.Vertical + CameraCol.CameraObj.right * _input.Horizontal;
                if (forward.magnitude > 1f)
                    forward.Normalize();
                forward = transform.InverseTransformDirection(forward);
                //方法1：
                Quaternion quaternion = Quaternion.LookRotation(forward, Vector3.up);
                quaternion = Quaternion.Slerp(Model.transform.rotation, quaternion, Time.deltaTime * RotationSpeed);
                Model.transform.rotation = quaternion;
                //方法2：
                //Model.transform.forward = _input.Horizontal * Model.transform.right + _input.Vertical * Model.transform.forward;
            }
        }

        /// <summary>
        /// 位移
        /// </summary>
        void Move()
        {
            AnimatorUpdate();
            _rigidbody.position += DeltaPos;
            _rigidbody.MovePosition(_rigidbody.position + _moveDir * (_input.IsRun ? RunMultiplier * WalkSpeed : WalkSpeed) * Time.fixedDeltaTime);

            //冲量
            _rigidbody.velocity += ThrustVec;

            //冲量这里需要优化，如果使用如下优化，需要把MovePosition删除，用每帧对_rigidbody添加一个初速度移动
            //_rigidboy.velocity += new Vector3(_moveDir.x, _rigidboy.velocity.y, _moveDir.z) + ThrustVec;
            ThrustVec = Vector3.zero;
            DeltaPos = Vector3.zero;
        }

        /// <summary>
        /// 翻滚，跳跃
        /// </summary>
        void Jump()
        {
            if (_rigidbody.velocity.sqrMagnitude > _sqrHightFallStiff)
                _anim.SetTrigger("Roll");
            if (_input.IsJump)
                _anim.SetTrigger("Jump");

            if (IsGrounded)
                _anim.SetBool("IsInGround", true);
            else
                _anim.SetBool("IsInGround", false);
        }

        /// <summary>
        /// 防御
        /// </summary>
        void Defense()
        {
            _anim.SetBool("Defense", _input.IsDefense);
            if (CheckAnimatorState("Ground"))
            {
                if (_input.IsDefense)
                    SetLayerWeight("Defanse Layer", 1);
                else
                    SetLayerWeight("Defanse Layer", 0);
            }
            else
            {
                SetLayerWeight("Defanse Layer", 0);
            }
        }

        /// <summary>
        /// 计算方向向量
        /// </summary>
        void CalculateMoveDirection()
        {

            if (!LockPlanar)
            {
                _moveDir = Vector3.Scale(CameraCol.CameraObj.forward, new Vector3(1, 0, 1)) * _input.Vertical + CameraCol.CameraObj.right * _input.Horizontal;
                if (_moveDir.magnitude > 1f)
                    _moveDir.Normalize();
                _moveDir = transform.InverseTransformDirection(_moveDir);
            }
            if (CameraCol.LockState)
            {
                if (TrackDirection)
                    Model.transform.forward = _moveDir;
                else
                    Model.transform.forward = transform.forward;
            }
            else
            {
                Rotation();
            }

            ////根据镜头锁定状态计算位移向量
            //if (CameraCol.LockState)
            //{

            //    //重新计算移动向量，位移向量以自身为基准
            //    if (!LockPlanar)
            //        _moveDir = (_input.Horizontal * transform.right + _input.Vertical * transform.forward) * (_input.IsRun ? RunMultiplier * WalkSpeed : WalkSpeed);

            //    if (TrackDirection)
            //        Model.transform.forward = _moveDir;
            //    else
            //        Model.transform.forward = transform.forward;
            //}
            //else
            //{
            //    //位移向量以模型为基准
            //    if (!LockPlanar)
            //    {
            //        _moveDir = Vector3.Scale(CameraCol.CameraObj.forward, new Vector3(1, 0, 1)) * _input.Vertical + CameraCol.CameraObj.right * _input.Horizontal;
            //        if (_moveDir.magnitude > 1f)
            //            _moveDir.Normalize();
            //        _moveDir = transform.InverseTransformDirection(_moveDir);
            //    }
            //    //Vector3.Scale(CameraCol.CameraObj.forward, new Vector3(1, 0, 1)) * _input.Vertical + CameraCol.CameraObj.right * _input.Horizontal
            //    Rotation();
            //    //_moveDir = _input.InputVec.normalized * ;
            //}
        }
        #endregion

        #region Public Methods
       
        public void ResetMoveDirZero()
        {
            _moveDir = Vector3.zero;
        }

        public void SetInputLock(bool on)
        {
            _input.LockMovementInput = on;
        }

        #endregion
    }

}