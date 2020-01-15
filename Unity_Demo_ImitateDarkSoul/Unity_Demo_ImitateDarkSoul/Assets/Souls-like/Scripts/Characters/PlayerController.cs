using LS.Common.Math;
using LS.Common.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        #region Public Fields
        public GameObject Model;
        public CameraController CameraCol;
        [Tooltip("旋转速度")] public float RotationSpeed;
        [Tooltip("行走速度")] public float WalkSpeed;
        [Tooltip("跑步系数")] public float RunMultiplier = 2f;
        [Tooltip("跳跃速度(垂直向上)")] public float JumpVerlocity;
        [Tooltip("翻滚速度")] public Vector2 RollVelocity;
        [Tooltip("后跳速度,x分量为up，y分量为back")] public Vector2 JabVerlocity;
        [Tooltip("高处落地硬值以落地速度计算")] public float HightFallStiff = 5f;
        [Tooltip("从高处落下后死亡高度，以落地速度计算")] public float HightFallDead = 15f;
        [Tooltip("持续状态的冲量系数")] [Range(0, 1)] public float DurationThrustMultiplier;

        /// <summary> 锁定平面位移量，跳跃的时候，不更改Move Direction </summary>
        [HideInInspector] public bool LockPlanar = false;
        /// <summary>  Root Motion DeltaPosition </summary>
        [HideInInspector] public Vector3 DeltaPos;
        ///<summary> 是否在地面 </summary>
        [HideInInspector] public bool IsGrounded = true;
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
        Animator _anim;
        UserInput _input;
        Rigidbody _rigidboy;

        /// <summary> 位移方向向量 </summary>
        Vector3 _moveDir;

        float _sqrHightFallStiff;
        float u;
        float v;
        #endregion

        #region MonoBehaviour Callbacks
        private void Awake()
        {
            _anim = Model.GetComponent<Animator>();
            _input = GetComponent<UserInput>();
            _rigidboy = GetComponent<Rigidbody>();

            _sqrHightFallStiff = HightFallStiff * HightFallStiff;
        }

        private void Update()
        {

            if (_input.IsLockOn)
                CameraCol.CameraLockOn();

            Jump();

            if (CameraCol.LockState)
            {
                //重新计算移动向量，位移向量以自身为基准
                if (!LockPlanar)
                    _moveDir = (_input.Horizontal * transform.right + _input.Vertical * transform.forward) * (_input.IsRun ? RunMultiplier * WalkSpeed : WalkSpeed);

                if (TrackDirection)
                {
                    Debug.LogWarning("Lock");
                    Model.transform.forward = _moveDir.normalized;
                }
                else
                    Model.transform.forward = transform.forward;

            }
            else
            {
                Rotation();
                //位移向量以模型为基准
                if (!LockPlanar)
                    _moveDir = _input.InputVec.normalized * (_input.IsRun ? RunMultiplier * WalkSpeed : WalkSpeed);
            }
            Defense();
            Attack();
        }

        private void FixedUpdate()
        {
            Movement();
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
                //方法1：
                Quaternion quaternion = Quaternion.LookRotation(_input.InputVec, Vector3.up);
                quaternion = Quaternion.Slerp(Model.transform.rotation, quaternion, Time.deltaTime * RotationSpeed);
                Model.transform.rotation = quaternion;
                //方法2：
                //Model.transform.forward = _input.Horizontal * Model.transform.right + _input.Vertical * Model.transform.forward;
            }
        }

        /// <summary>
        /// 位移
        /// </summary>
        void Movement()
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

            _rigidboy.position += DeltaPos;
            _rigidboy.MovePosition(_rigidboy.position + _moveDir * Time.fixedDeltaTime);

            //冲量
            _rigidboy.velocity += ThrustVec;

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
            if (_rigidboy.velocity.sqrMagnitude > _sqrHightFallStiff)
                _anim.SetTrigger("Roll");
            if (_input.IsJump)
                _anim.SetTrigger("Jump");

            if (IsGrounded)
                _anim.SetBool("IsInGround", true);
            else
                _anim.SetBool("IsInGround", false);
        }

        /// <summary>
        /// 攻击
        /// </summary>
        void Attack()
        {
            if (_input.IsAttack && CheckAnimatorState("Ground") && IsGrounded)
                _anim.SetTrigger("Attack");
        }

        /// <summary>
        /// 防御
        /// </summary>
        void Defense()
        {
            _anim.SetBool("Defense", _input.IsDefense);
        }

        #endregion

        #region Public Methods
        public float GetAnimFloat(string name)
        {
            return _anim.GetFloat(name);
        }

        public float GetCurrentAnimatorLayerWeight(string name)
        {
            return _anim.GetLayerWeight(_anim.GetLayerIndex(name));
        }

        public float GetCurrentAnimatorLayerWeight(int layerIndex)
        {
            return _anim.GetLayerWeight(layerIndex);
        }

        public void SetLayerWeight(string layerName, float weight)
        {
            _anim.SetLayerWeight(_anim.GetLayerIndex(layerName), weight);
        }

        public void SetLayerWeight(int layerIndex, float weight)
        {
            _anim.SetLayerWeight(layerIndex, weight);
        }

        public void ResetMoveDirZero()
        {
            _moveDir = Vector3.zero;
        }

        public void SetInputLock(bool on)
        {
            _input.LockMovementInput = on;
        }

        public bool CheckAnimatorState(string animtorName, string maskName = "Base Layer")
        {
            return _anim.GetCurrentAnimatorStateInfo(_anim.GetLayerIndex(maskName))
                .IsName(animtorName);
        }
       
        #endregion
    }

}