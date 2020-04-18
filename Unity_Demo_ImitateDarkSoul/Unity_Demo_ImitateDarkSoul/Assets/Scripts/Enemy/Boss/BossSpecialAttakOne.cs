using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class BossSpecialAttakOne : MonoBehaviour
    {
        public GameObject GroundExplosion;
        public float Force = 30f;
        public float DamageValue = 100f;

        Collider _checkCollider;
        ActorManager _playerAM;

        

        private void Awake()
        {
            _checkCollider = GetComponent<Collider>();
        }

        private void OnEnable()
        {
            GroundExplosion.SetActive(true);
            _checkCollider.enabled=true;

        }
        private void OnDisable()
        {
            GroundExplosion.SetActive(false);
            _checkCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                if(_playerAM==null)
                {
                    _playerAM = other.GetComponent<ActorManager>();
                }
                _playerAM.CalculateWeaponData(WeaponType.Special, 50f,SpecialAttack);
                _checkCollider.enabled = false;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (_playerAM == null)
                {
                    _playerAM = other.GetComponent<ActorManager>();
                }
                _playerAM.CalculateWeaponData(WeaponType.Special, 50f, SpecialAttack);
                _checkCollider.enabled = false;
            }
        }

        void SpecialAttack()
        {
            (_playerAM.Controller as PlayerController).ResetMoveDirZero();
            _playerAM.Controller.Rig.AddForce((transform.forward + Vector3.up) * Force);
            _playerAM.SetAnimAfterDoDamg(_playerAM.SM.AddHP(-DamageValue));
            StartCoroutine(WaitForAttackEnd(_playerAM.Controller as PlayerController));
        }

        IEnumerator WaitForAttackEnd(PlayerController _playerController)
        {
            _playerController.SetInputLock(true);
            yield return new WaitForSeconds(3f);
            _playerController.SetInputLock(false);
            enabled = false;
        }

    }

}