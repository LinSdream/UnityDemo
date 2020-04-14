using Souls.AI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Souls
{
    
    public class AIController : BaseController
    {

        #region Callbacks

        private void Start()
        {
            //_fsm = GetComponent<EnemyFSM>();
            //_fsm.AI.speed = WalkSpeed;
            //_fsm.AI.angularSpeed = RotationSpeed;
        }

        protected override  void Update()
        {
            //_fsm.AI.speed = WalkSpeed;
            //_fsm.AI.angularSpeed = RotationSpeed;

            if (IsGrounded)
            {
                _anim.SetBool("IsInGround", true);
            }
            else
            {
                _anim.SetBool("IsInGround", false);
            }

        }

        private void FixedUpdate()
        {
            AnimatorUpdate();
        }

        #endregion

        #region Override Methdos
        protected override void AnimatorUpdate()
        {
            ////Forward Motion Set
            //if (_fsm.CurrentState.HasAction("Move"))
            //    _anim.SetFloat("Forward", 1);
            //else if (_fsm.CurrentState.HasAction("Run"))
            //    _anim.SetFloat("Forward", 2);
            //else
            //    _anim.SetFloat("Forward", 0);
        }

        public override void Hit()
        {
            _anim.SetTrigger("Hit");
        }

        public override void Die()
        {
            //关闭碰撞框
            var list=gameObject.GetComponentsInChildren<InteractionManager>();
            foreach(var cell in list)
            {
                foreach (var caster in cell.OverlapEcastms)
                {
                    var collider = caster.GetComponent<Collider>();
                    if (collider != null)
                        collider.enabled = false;
                }
                cell.enabled = false;
            }
            //动画
            _anim.SetTrigger("Die");
        }

        public override void Attack()
        {
            if ((CheckAnimatorState("Ground") || CheckAnimatorStateTag("AttackTag")) && IsGrounded)
            {
                    _anim.SetTrigger("Attack");
                    _anim.SetBool("AttackMirror", false);
            }
        }

        public override void Stunned()
        {
            _anim.SetTrigger("Stunned");
        }

        public override void Blocked()
        {
            _anim.SetTrigger("Blocked");
        }
        #endregion
    }

}