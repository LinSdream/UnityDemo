using LS.Test.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls.AI
{
    public class AIFSM : FSMBase
    {
        AIController _controller;

        #region Callbacks
        private void Awake()
        {
            _controller = GetComponent<AIController>();
        }
        #endregion

    }

}