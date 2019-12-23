using LS.Common;
using LS.Helper.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioManager = LS.Test.AudioManager;

namespace Game
{

    public enum PlayerStatus
    {
        Default,
        Idle,
        Run,
        Roll,
        Attack,
        SpecialAttack,
        Switch,
        Drinking,
        CanSit,
        Sit,
        PowerOver,
        Dead
    }

    public class Player : MonoBehaviour
    {
        #region Private Fields
        ThirdPersonMovement _movement;
        Animator _anim;
        AnimatorStateInfo _animInfo;
        int CurrentCombo;
        #endregion

        #region Public Fields
        public PlayerStatus Status = PlayerStatus.Default;
        #endregion
        #region MonoBehaviour Callbacks
        private void Start()
        {
            _movement = GetComponent<ThirdPersonMovement>();
            _anim = GetComponent<Animator>();
            _movement.MovementInGroundAnimEvent += Walk_AnimtorEvent;

            _animInfo = _anim.GetCurrentAnimatorStateInfo(0);
            _movement.BeforeInputAction += CheckStatus;
            _movement.BeforeInputAction += () =>
            {
                if (Input.GetButtonDown("Attack"))
                    Attack();
            };
        }

        private void Update()
        {
            
            switch (Status)
            {
                case PlayerStatus.Default:
                    break;
                case PlayerStatus.Idle:
                    Idle();
                    break;
                case PlayerStatus.Run:
                    Run();
                    break;
                case PlayerStatus.Roll:
                    break;
                case PlayerStatus.Attack:
                    
                    break;
                case PlayerStatus.SpecialAttack:
                    break;
                case PlayerStatus.Switch:
                    break;
                case PlayerStatus.Drinking:
                    break;
                case PlayerStatus.CanSit:
                    break;
                case PlayerStatus.Sit:
                    break;
                case PlayerStatus.PowerOver:
                    break;
                case PlayerStatus.Dead:
                    break;
            }
        }
        #endregion
        #region Animator Events
        private void Walk_AnimtorEvent(float h, float v)
        {
            if (h == 0 && v == 0)
            {
                _anim.SetBool("Running", false);
            }
            else
            {
                _anim.SetBool("Running", true);
                Status = PlayerStatus.Run;
            }

        }
        #endregion

        #region Public Methods

        public void CheckStatus()
        {
            _animInfo = _anim.GetCurrentAnimatorStateInfo(0);

            if (_animInfo.normalizedTime > 1f)
            {
                if (!_animInfo.IsName("Unarmed-Attack01") ||
                    !_animInfo.IsName("Unarmed-Attack02") ||
                    !_animInfo.IsName("Unarmed-Attack03"))
                {
                    _anim.SetInteger("ActionCMD", 0);
                    CurrentCombo = 0;
                }
            }
        }

        public void Run()
        {
           
        }

        public void Idle()
        {
            
        }

        public void Attack()
        {
            
            if ((_animInfo.IsName("Unarmed-Idle")||_animInfo.IsName("Unarmed-Run"))
                && _animInfo.normalizedTime > 0 && CurrentCombo == 0)
            {
                Status = PlayerStatus.Attack;
                CurrentCombo = 1;
                _anim.SetInteger("ActionCMD", 1);
            }
            if (_animInfo.IsName("Unarmed-Attack01") && _animInfo.normalizedTime > 0 && CurrentCombo == 1)
            {
                Status = PlayerStatus.Attack;
                CurrentCombo = 2;
                _anim.SetInteger("ActionCMD", 2);
            }
            if (_animInfo.IsName("Unarmed-Attack02") && _animInfo.normalizedTime > 0 && CurrentCombo == 2)
            {
                Status = PlayerStatus.Attack;
                CurrentCombo = 3;
                _anim.SetInteger("ActionCMD", 3);
            }
        }
        #endregion
    }
}