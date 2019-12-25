﻿using LS.Common.Math;
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
        public float RotationSpeed;
        public float WalkSpeed;
        public float RunMultiplier=2f;
        #endregion

        #region Private Fields
        Animator _anim;
        PlayerInput _input;
        Rigidbody _rigidboy;

        Vector3 _moveDir;

        float u;
        float v;
        #endregion

        #region MonoBehaviour Callbacks
        private void Awake()
        {
            _anim = Model.GetComponent<Animator>();
            _input = GetComponent<PlayerInput>();
            _rigidboy = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            Rotation();
            Jump();
            _moveDir = _input.InputVec.normalized * (_input.IsRun ? RunMultiplier*WalkSpeed : WalkSpeed);
        }

        private void FixedUpdate()
        {
            Movement();
        }
        #endregion

        #region Private Methdos
        void Rotation()
        {
            //取x2+y2=1范围内的0-1的值
            SharedMethods.SquareToDiscMapping(_input.Horizontal, _input.Vertical, ref u, ref v);
            _anim.SetFloat("Forward",
                Mathf.Sqrt(u * u + v * v)
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
        }

        void Jump()
        {
            if (_input.IsJump)
                _anim.SetTrigger("Jump");
        }
        #endregion
    }

}