using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class BattleManager : MonoBehaviour
    {

        [HideInInspector] public ActorManager AM;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("AttackWeapon"))
            {
                Debug.Log(other.tag);
                AM.Damage();
            }
        }

    }

}