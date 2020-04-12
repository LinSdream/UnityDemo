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
        //战斗判定的collider
        CapsuleCollider _defCol;

        private void Start()
        {
            _defCol = GetComponent<CapsuleCollider>();
            if (_defCol == null)
            {
                _defCol.center = Vector3.up * 1f;
                _defCol.height = 2f;
                _defCol.isTrigger = true;

            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var wc = other.GetComponentInParent<WeaponController>();
            //这里有两种碰撞，一种是Layer Weapon的碰撞，一种是Layer Caster的碰撞，Layer Caster上面不存在WeaponController
            if (wc == null)
                return;
            //Debug.Log(wc == null ? 1 : 0);
            var attacker = wc.WM.AM;
            var receiver = AM;

            //攻击角度判断
            Vector3 attackDir = receiver.transform.position - attacker.transform.position;//攻击者到受击者的方向
            float attackAngle = Mathf.Acos(Vector3.Dot(attacker.Controller.Model.transform.forward, attackDir)) * Mathf.Rad2Deg;//= Vector3.Angle(attacker.transform.forward, attackDir);

            //弹反
            Vector3 counterDir = attacker.transform.position - receiver.transform.position;
            float counterAngle1 = Vector3.Angle(receiver.transform.forward, counterDir);
            float counterAngle2 = Vector3.Angle(attacker.transform.forward, receiver.transform.forward);

            ///TODO:仔细思考后，360度都算是攻击有效范围，应该是距离的判定，之后有时间补
            bool attackValid = (attackAngle < 360);
            bool counterValid = counterAngle1 < 180f;
            //bool counterValid = (counterAngle1 < 180f && Mathf.Abs(counterAngle2 - 180) < 45f);
            //Debug.Log(counterAngle1 + " " + counterAngle2);
            if (other.CompareTag("Weapon"))
            {
                AM.TryDoDamg(wc, true, counterValid);
            }
        }

    }

}