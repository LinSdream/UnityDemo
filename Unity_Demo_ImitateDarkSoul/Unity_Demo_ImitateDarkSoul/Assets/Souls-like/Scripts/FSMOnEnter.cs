using LS.Common.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{

    public delegate void AnimatorFSMHandle();

    public class FSMOnEnter : StateMachineBehaviour
    {

        public string[] MessageNames;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (string name in MessageNames)
            {
                MessageCenter.Instance.SendMessage(name, animator.gameObject);
            }
        }
    }

}