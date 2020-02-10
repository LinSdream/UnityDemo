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

        #region Public Methods about Aniamtor
        public bool CheckAnimatorState(string animtorName, string maskName = "Base Layer")
        {
            return _anim.GetCurrentAnimatorStateInfo(_anim.GetLayerIndex(maskName))
                .IsName(animtorName);
        }

        public bool CheckAnimatorStateTag(string tagName, string maskName = "Base Layer")
        {
            return _anim.GetCurrentAnimatorStateInfo(_anim.GetLayerIndex(maskName))
                .IsTag(tagName);
        }

        public float GetAnimFloat(string name)
        {
            return _anim.GetFloat(name);
        }

        public float GetCurrentAnimatorLayerWeight(string name)
        {
            return _anim.GetLayerWeight(_anim.GetLayerIndex(name));
        }

        public float GetCurrentAnimatorLayerWeight(int layerIndex)
        {
            return _anim.GetLayerWeight(layerIndex);
        }

        public void SetLayerWeight(string layerName, float weight)
        {
            _anim.SetLayerWeight(_anim.GetLayerIndex(layerName), weight);
        }

        public void SetLayerWeight(int layerIndex, float weight)
        {
            _anim.SetLayerWeight(layerIndex, weight);
        }

        #endregion

        #region Abstract Methods

        protected abstract void AnimatorUpdate();

        public abstract void Hit(float value);

        public abstract void Attack();
        #endregion
    }

}