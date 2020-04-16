using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class BossActorManager : ActorManager
    {
        #region Override Methods

        public override void DoInterationEvent()
        {

        }

        protected override void Register()
        {
            BossMessageCnter.Instance.AddListener("BossAttackEnable", BossAttackEnable);
        }

        protected override void UnRegister()
        {
            BossMessageCnter.Instance.RemoveListener("BossAttackEnable", BossAttackEnable);
        }

        #endregion

        #region Messages

        private void BossAttackEnable(GameObject render, EventArgs e)
        {
            var on = (e as BossAttackEnableEventArgs).On;
            if (on==1)
                WM.WeaponEnable();
            else
                WM.WeaponDisable();
        }

        #endregion

    }

}