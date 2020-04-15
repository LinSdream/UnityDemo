using LS.Common.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class BoxGetWeaponController : BaseController
    {
        public override void Attack()
        {
        }

        public override void Die()
        {
            var go=GameManager.Instance.CreateWeapon("Mace");
            MessageCenter.Instance.SendMessage("WeaponAdd", go, new WeaponUIEventArgs() { WeaponName = "Mace" });
            go = GameManager.Instance.CreateWeapon("Falchion");
            MessageCenter.Instance.SendMessage("WeaponAdd", go, new WeaponUIEventArgs() { WeaponName = "Falchion" });
        }

        public override void Hit()
        {

        }

        protected override void AnimatorUpdate()
        {
        }

    }

}