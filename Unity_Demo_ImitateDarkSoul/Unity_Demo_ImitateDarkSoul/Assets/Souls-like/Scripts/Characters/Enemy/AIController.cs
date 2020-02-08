using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Souls
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AIController : BaseController
    {

        NavMeshAgent _ai;

        #region Callbacks
        protected override void Awake()
        {
            base.Awake();

            _ai = GetComponent<NavMeshAgent>();
        }
         
        private void Update()
        {
            if (IsGrounded)
                _anim.SetBool("IsInGround", true);
            else
                _anim.SetBool("IsInGround", false);
        }

        private void FixedUpdate()
        {
            AnimatorUpdate();    
        }

        #endregion

        protected override void AnimatorUpdate()
        {

        }


    }

}