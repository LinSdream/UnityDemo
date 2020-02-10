using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{

    // This script is designed to be placed on the root object of a Character,
    // comprising 3 gameobjects, each parented to the next:

    // 	Character
    // 		CharacterModel
    // 		Sendsor  ---> need OnGroundSendsor Script
    [RequireComponent(typeof(Rigidbody))]
    public abstract class BaseController : MonoBehaviour
    {
        public GameObject Model;
        [Tooltip("旋转速度")] public float RotationSpeed;
        [Tooltip("行走速度")] public float WalkSpeed;
        [Tooltip("跑步系数")] public float RunMultiplier = 2f;

        ///<summary> 是否在地面 </summary>
        [HideInInspector] public bool IsGrounded = true;

        protected Animator _anim;
        protected Rigidbody _rigidbody;

        #region MonoBehaviour Callbacks

        protected virtual void Awake()
        {
            _anim = Model.GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        #endregion

        protected abstract void AnimatorUpdate();
    }

}