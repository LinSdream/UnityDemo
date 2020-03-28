using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    /// <summary>
    /// 战斗管理器
    /// </summary>
    [RequireComponent(typeof(CapsuleCollider))]
    public class BattleManager : AbstractActorManager
    {

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("AttackWeapon"))
            {
                Debug.Log(other.tag);
                AM.TryDoDamg();
            }
        }

    }

}