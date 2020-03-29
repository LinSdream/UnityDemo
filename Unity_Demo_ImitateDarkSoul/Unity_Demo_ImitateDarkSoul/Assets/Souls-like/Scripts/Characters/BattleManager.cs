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

        CapsuleCollider _defCol;

        private void Start()
        {
            _defCol = GetComponent<CapsuleCollider>();
            _defCol.center = Vector3.up * 1f;
            _defCol.height = 2f;
            _defCol.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.name);

            var wc = other.GetComponentInParent<WeaponController>();

            var attacker = wc.WM.AM.gameObject;
            var receiver = AM.gameObject;

            //攻击角度判断
            Vector3 attackDir = receiver.transform.position - attacker.transform.position;
            float attackAngle = Vector3.Angle(attacker.transform.forward, attackDir);

            //弹反
            Vector3 counterDir = attacker.transform.position - receiver.transform.position;
            float counterAngle1 = Vector3.Angle(receiver.transform.forward, counterDir);
            float counterAngle2 = Vector3.Angle(attacker.transform.forward, receiver.transform.forward);

            bool attackValid = (Mathf.Abs(attackAngle) < 45);
            bool counterValid = (Mathf.Abs(counterAngle1) < 30 && Mathf.Abs(counterAngle2 - 180) < 30f);

            if (other.CompareTag("Weapon"))
            {
                if(Mathf.Abs(attackAngle)<=45f)
                    AM.TryDoDamg(wc,attackValid,counterValid);
            }
        }

    }

}