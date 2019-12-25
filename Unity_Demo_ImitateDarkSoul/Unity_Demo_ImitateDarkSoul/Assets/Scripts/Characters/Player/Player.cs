using LS.Common;
using LS.Helper.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioManager = LS.Test.AudioManager;

namespace Game
{

    #region Enum PlayerStatus
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
    #endregion

    public class Player : MonoBehaviour
    {
        #region Private Fields
        ThirdPersonMovement _movement;
        Animator _anim;
        AnimatorStateInfo _animInfo;
        int CurrentCombo;
        CharacterController _characterController;
        #endregion

        #region Public Fields
        public PlayerStatus Status = PlayerStatus.Default;
        public int DrinkNum = 3;
        public float DrinkSpeed = 2f;
        #endregion
        #region MonoBehaviour Callbacks

        private void Awake()
        {
            WeaponManager.Instance.WeaponList.Clear();
            WeaponManager.Instance.FindAndAddList();
            PlayerAnimManager.Instance.GetAnimByWeaponName("Unarmed");
            _characterController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            _movement = GetComponent<ThirdPersonMovement>();
            _anim = GetComponent<Animator>();
            _movement.MovementInGroundAnimEvent += Walk_AnimtorEvent;

            _animInfo = _anim.GetCurrentAnimatorStateInfo(0);
            _movement.BeforeInputAction += CheckStatusEvent;
            _movement.BeforeInputAction += GetButtomDownEvent;
        }

        private void Update()
        {

            switch (Status)
            {
                case PlayerStatus.Default:
                    break;
                case PlayerStatus.Idle:
                    break;
                case PlayerStatus.Run:
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
        #region Events
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

        public void CheckStatusEvent()
        {
            _animInfo = _anim.GetCurrentAnimatorStateInfo(0);

            if (_animInfo.normalizedTime < 1f)
            {
                if (_animInfo.IsName(PlayerAnimManager.Instance.Attack01))
                    Status = PlayerStatus.Attack;
                if (_animInfo.IsName(PlayerAnimManager.Instance.SpecialAttack))
                    Status = PlayerStatus.SpecialAttack;
                if(_animInfo.IsName(PlayerAnimManager.Instance.Roll))
                {
                    if(Status!=PlayerStatus.Roll)
                    {
                        _characterController.SimpleMove(transform.forward * _movement.MoveSpeed);
                    }
                    Status = PlayerStatus.Roll;
                }
                if (_animInfo.IsName(PlayerAnimManager.Instance.Idle))
                    Status = PlayerStatus.Idle;
                if (_animInfo.IsName(PlayerAnimManager.Instance.Drink))
                    Status = PlayerStatus.Drinking;
            }

            if (_animInfo.normalizedTime > 1f)
            {
                if (!_animInfo.IsName(PlayerAnimManager.Instance.Attack01) ||
                    !_animInfo.IsName(PlayerAnimManager.Instance.Attack02) ||
                    !_animInfo.IsName(PlayerAnimManager.Instance.Attack03))
                {
                    _anim.SetInteger(PlayerAnimManager.Instance.Parameters_PlayerActionCMD, 0);
                    CurrentCombo = 0;

                    Status = PlayerStatus.Idle;
                }
                else if (_animInfo.IsName(PlayerAnimManager.Instance.Idle))
                    Status = PlayerStatus.Idle;
                else if (_animInfo.IsName(PlayerAnimManager.Instance.Run))
                    Status = PlayerStatus.Run;
            }
        }

        private void GetButtomDownEvent()
        {  
            if (Input.GetButtonDown("Attack"))
                Attack();
            if (Input.GetButtonDown("SpecialAttack") && Status != PlayerStatus.SpecialAttack)
                SpecialAttack();
            if (Input.GetButtonDown("SwitchWeapon"))
                SwitchWeapon();
            if (Input.GetButtonDown("Roll"))
                Roll();
            //if (Input.GetButtonDown("Drink"))
            //    Drink();
        }

        #endregion

        #region Public Methods


        public void Attack()
        {

            if ((_animInfo.IsName(PlayerAnimManager.Instance.Idle) 
                || _animInfo.IsName(PlayerAnimManager.Instance.Run))
                && _animInfo.normalizedTime > 0 && CurrentCombo == 0)
            {
                Status = PlayerStatus.Attack;
                CurrentCombo = 1;
                _anim.SetInteger(PlayerAnimManager.Instance.Parameters_PlayerActionCMD, 1);
            }
            if (_animInfo.IsName(PlayerAnimManager.Instance.Attack01) 
                && _animInfo.normalizedTime > 0 && CurrentCombo == 1)
            {
                Status = PlayerStatus.Attack;
                CurrentCombo = 2;
                _anim.SetInteger(PlayerAnimManager.Instance.Parameters_PlayerActionCMD, 2);
            }
            if (_animInfo.IsName(PlayerAnimManager.Instance.Attack02) && _animInfo.normalizedTime > 0 && CurrentCombo == 2)
            {
                Status = PlayerStatus.Attack;
                CurrentCombo = 3;
                _anim.SetInteger(PlayerAnimManager.Instance.Parameters_PlayerActionCMD, 3);
            }
        }

        public void Roll()
        {
            _anim.SetTrigger("Roll");
        }

        public void SpecialAttack()
        {
            _anim.SetTrigger("SpecialAttack");
        }

        public void SwitchWeapon()
        {
            _anim.SetTrigger("SwitchWeapon");
        }

        public void Drink()
        {
            _anim.SetTrigger("Drink");
        }
        #endregion

        #region Animtor  Events
        public void Animtor_Audio(string name)
        {
            AudioManager.Instance.PlaySFX(name);
        }
        
        public void HideWeapon()
        {
            WeaponManager.Instance.GetCurrentWeapon().SetActive(false);
            string name;
            WeaponManager.Instance.GetNextWeapon(out name).SetActive(true);
            if (name == string.Empty)
            {
                Debug.LogError("Player/HideWeapon Error : Unable to obtain the mapping name of the animator !");
                return;
            }
            PlayerAnimManager.Instance.GetAnimByWeaponName(name);
        }

        public void ChangeAnimatorSpeed()
        {
            _anim.speed = DrinkSpeed;
        }

        public void ResetAnimatorSpeed()
        {
            _anim.speed = 1f;
        }
        #endregion

    }
}