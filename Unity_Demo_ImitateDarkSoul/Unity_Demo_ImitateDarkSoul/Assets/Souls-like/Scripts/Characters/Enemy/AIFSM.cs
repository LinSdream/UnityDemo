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
        [HideInInspector] public AIController Controller;

        #region Callbacks
        private void Awake()
        {
            AI = GetComponent<NavMeshAgent>();
            Controller = GetComponent<AIController>();
        }

        public void SetForwardAnimator(float value)
        {
            Controller.Anim.SetFloat("Forward", value);
        }

        #endregion

    }

}