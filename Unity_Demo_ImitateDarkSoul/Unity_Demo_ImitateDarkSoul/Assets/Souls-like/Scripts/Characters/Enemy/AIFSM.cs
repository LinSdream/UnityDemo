using LS.Test.AI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Souls.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AIFSM : FSMBase
    {
        [HideInInspector] public NavMeshAgent AI;
        AIController _controller;

        #region Callbacks
        private void Awake()
        {
            AI = GetComponent<NavMeshAgent>();
            _controller = GetComponent<AIController>();
        }

        public void SetForwardAnimator(float value)
        {
            _controller.Anim.SetFloat("Forward", value);
        }

        #endregion

    }

}