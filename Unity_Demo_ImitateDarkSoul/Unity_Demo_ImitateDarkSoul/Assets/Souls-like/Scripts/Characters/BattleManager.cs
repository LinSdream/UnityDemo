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
            Debug.Log(other.name);
            if (other.CompareTag("Sword"))
                AM.Damage();
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.gameObject.name);
        }
    }

}