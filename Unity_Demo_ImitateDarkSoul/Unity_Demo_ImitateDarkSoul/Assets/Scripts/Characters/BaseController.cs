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

        public delegate void InterationHandle();

        #region Public Fields
        public GameObject Model;
        [Tooltip("旋转速度")] public float RotationSpeed;
        [Tooltip("行走速度")] public float WalkSpeed;
        [Tooltip("跑步系数")] public float RunMultiplier = 2f;
        [Tooltip("角色的基本信息")] public CharacterInfo Info;
        public event InterationHandle InterationEvent;
        public Animator Anim => _anim;

        ///<summary> 是否在地面 </summary>
        [HideInInspector] public bool IsGrounded = true;

        #endregion

        #region Protected Fields

        protected Animator _anim;
        protected Rigidbody _rigidbody;

        #endregion 

        #region MonoBehaviour Callbacks

        protected virtual void Awake()
        {
            _anim = Model.GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        protected virtual void Update()
        {
            InterationEvent?.Invoke();
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

        public void IssueTrigger(string triggerName)
        {
            _anim.SetTrigger(triggerName);
        }

        public void IssueBool(string booleanName,bool boolean)
        {
            _anim.SetBool(booleanName, boolean);
        }

        public void IssueFloat(string floatName,float value)
        {
            _anim.SetFloat(floatName, value);
        }

        #endregion

        #region Virtual Methods

        /// <summary> 重启 </summary>
        public virtual void Restart() { }

        public virtual void Blocked(){}

        /// <summary> 弹反 </summary>
        public virtual void HeavyAttack() { }

        /// <summary> 硬值 </summary>
        public virtual void Stunned() { }

        #endregion

        #region Abstract Methods

        protected abstract void AnimatorUpdate();
        
        /// <summary> 受到伤害 </summary>
        /// <param name="value">受到的伤害值</param>
        public abstract void Hit();

        /// <summary> 攻击 </summary>
        public abstract void Attack();

        /// <summary> 死亡 </summary>
        public abstract void Die();
        #endregion
    }

}