using LS.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Souls
{
    public class BossController : BaseController
    {

        public BossSpecialAttakOne SpecialOne;
        public Slider BossHP;
        BossActorManager _bossAM;
        public  BossInfo BossIF;

        protected override void Awake()
        {
            base.Awake();
            _bossAM = GetComponent<BossActorManager>();
            BossIF = Info as BossInfo;
        }


        protected override void Update()
        {
            base.Update();
            AnimatorUpdate();
        }

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
            GameManager.Instance.Settlement(true);
            _bossAM.BM.CloseCollider();
        }

        public override void Hit()
        {
            AudioManager.Instance.PlaySFX("EnemyGetHit");
            _anim.SetTrigger("GetHit");
        }

        protected override void AnimatorUpdate()
        {
            if(BossHP.gameObject.activeInHierarchy)
                BossHP.value = _bossAM.SM.TempInfo.HP / Info.HP;
        }

        public void SpecialAttack() { }

        public void BeginBossBattle()
        {
            BossHP.gameObject.SetActive(true);
            BossHP.value = 1;
        }

    }

}