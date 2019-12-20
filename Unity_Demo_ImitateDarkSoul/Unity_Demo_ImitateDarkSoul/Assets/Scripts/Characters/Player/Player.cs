using LS.Helper.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Player : MonoBehaviour
    {
        ThirdPersonMovement _movement;
        Animator _anim;
        private void Start()
        {
            _movement = GetComponent<ThirdPersonMovement>();
            _anim = GetComponent<Animator>();
            _movement.MovementInGroundAnimEvent += (h, v) =>
            {
                if (h == 0 && v == 0)
                    _anim.SetBool("Running", false);
                else
                    _anim.SetBool("Running", true);
            };
        }
    }
}