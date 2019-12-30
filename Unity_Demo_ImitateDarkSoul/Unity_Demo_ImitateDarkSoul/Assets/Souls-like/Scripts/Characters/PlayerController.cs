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
        [Tooltip("旋转速度")] public float RotationSpeed;
        [Tooltip("行走速度")]public float WalkSpeed;
        [Tooltip("跑步系数")]public float RunMultiplier=2f;
        [Tooltip("跳跃力(换算向上速度)")]public float JumpPower;
        [Tooltip("高处落地硬值")]public float HightFallStiff = 5f;

        /// <summary> 锁定平面位移量，跳跃的时候，不更改Move Direction </summary>
        [HideInInspector] public bool LockPlanar = false;
        [HideInInspector] public Vector3 ThrustVec;
        [HideInInspector] public bool IsGrounded = true;
        
        [HideInInspector] public bool IsRun
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
        PlayerInput _input;
        Rigidbody _rigidboy;

        Vector3 _moveDir;

        float _sqrHightFallStiff;
        float u;
        float v;
        #endregion

        #region MonoBehaviour Callbacks
        private void Awake()
        {
            _anim = Model.GetComponent<Animator>();
            _input = GetComponent<PlayerInput>();
            _rigidboy = GetComponent<Rigidbody>();

            _sqrHightFallStiff = HightFallStiff * HightFallStiff;
        }

        private void Update()
        {
            Rotation();
            Jump();
            if (!LockPlanar)
                _moveDir = _input.InputVec.normalized * (_input.IsRun ? RunMultiplier * WalkSpeed : WalkSpeed);

            var names = MessageCenter.Instance.GetRegisteredMessagesName();
            foreach (var name in names)
                Debug.Log(name);
        }

        private void FixedUpdate()
        {
            Movement();
        }

        #endregion

        #region Private Methdos

        void Rotation()
        {
            ///TODO:可以进一步优化
            //取x2+y2=1范围内的0-1的值
            SharedMethods.SquareToDiscMapping(_input.Horizontal, _input.Vertical, ref u, ref v);
            _anim.SetFloat("Forward",
               (u * u + v * v)
                * Mathf.Lerp(_anim.GetFloat("Forward"), (_input.IsRun ? 2f : 1f), 0.5f));

            //角色旋转
            if (_input.Horizontal != 0 || _input.Vertical != 0)
            {
                //方法1：
                Quaternion quaternion = Quaternion.LookRotation(_input.InputVec, Vector3.up);
                quaternion = Quaternion.Lerp(Model.transform.rotation, quaternion, Time.deltaTime * RotationSpeed);
                Model.transform.rotation = quaternion;
                //方法2：
                //Model.transform.forward = _input.Horizontal * transform.right + _input.Vertical * transform.forward;
            }

        }

        void Movement()
        {
            _rigidboy.MovePosition(_rigidboy.transform.position + _moveDir * Time.fixedDeltaTime);

            //向上的冲量
            _rigidboy.velocity += ThrustVec;
            ThrustVec = Vector3.zero;
        }

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

        #endregion

    }

}