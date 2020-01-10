using LS.Common.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{

    public class FSMOnEnter : StateMachineBehaviour
    {
        public string[] MessageNames;

        FSMEventArgs _args = new FSMEventArgs();

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _args.StateInfo = stateInfo;
            _args.LayerIndex = layerIndex;
            foreach (string name in MessageNames)
            {
                MessageCenter.Instance.SendMessage(name,animator.gameObject,_args);
            }
        }
    }

}