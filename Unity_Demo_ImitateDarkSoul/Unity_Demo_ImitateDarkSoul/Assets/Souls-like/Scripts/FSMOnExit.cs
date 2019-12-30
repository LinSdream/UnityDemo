using LS.Common.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{

    public class FSMOnExit : StateMachineBehaviour
    {
        public string[] MessageNames;

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (var name in MessageNames)
            {
                MessageCenter.Instance.SendMessage(name, animator.gameObject);
            }
        }
    }

}