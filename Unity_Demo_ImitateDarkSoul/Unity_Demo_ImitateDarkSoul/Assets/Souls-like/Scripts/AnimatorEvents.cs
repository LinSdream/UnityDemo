using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class AnimatorEvents : MonoBehaviour
    {

        Animator _anim;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
        }

        public void ResetTrigger(string name)
        {
            _anim.ResetTrigger(name);
        }
    }

}