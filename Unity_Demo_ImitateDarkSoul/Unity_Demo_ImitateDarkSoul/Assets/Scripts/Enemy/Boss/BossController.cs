using LS.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class BossController : BaseController
    {

        public void Attack(int value)
        {
            _anim.SetInteger("Attack", value);
        }

        public override void Attack()
        {

        }

        public override void Die()
        {
            _anim.SetTrigger("Die");
        }

        public override void Hit()
        {
            AudioManager.Instance.PlaySFX("EnemyGetHit");
            _anim.SetTrigger("GetHit");
        }

        protected override void AnimatorUpdate()
        {

        }
    }

}