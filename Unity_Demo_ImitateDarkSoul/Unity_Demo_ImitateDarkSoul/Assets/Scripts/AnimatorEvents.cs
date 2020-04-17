using LS.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class AnimatorEvents : MonoBehaviour
    {

        //public string RightWeaponURL = "mixamorig:Hips/mixamorig:Spine" +
        //    "/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/WeaponHandle";
        //public string LeftWeaponURL = "mixamorig:Hips/mixamorig:Spine" +
        //    "/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm/mixamorig:LeftForeArm/mixamorig:LeftHand/ShieldHandle";

        Animator _anim;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
        }

        #region AnimatorEvents
        public void ResetTrigger(string name)
        {
            _anim.ResetTrigger(name);
        }

        /// <summary>
        /// Boss攻击动画的音效
        /// </summary>
        public void PlayWeaponAudioInBossBattle()
        {
            AudioManager.Instance.PlaySFX("BigSword");
        }

        /// <summary>
        /// 攻击时候对武器的Collider进行开关，如果关闭则要讲动画的参数置0
        /// </summary>
        /// <param name="on"></param>
        public void BossAttackEnable(int on)
        {
            BossMessageCenter.Instance.SendMessage("BossAttackEnable", null, new BossAttackEnableEventArgs() { On = on });
            if(on==0)
            {
                BossMessageCenter.Instance.SendMessage("ResetCombo");
            }
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        public void PlayAudioInBossBattle(string audioName)
        {
            AudioManager.Instance.PlaySFX(audioName);
        }

        public void BossSpecialAttack()
        {

        }
        #endregion
    }

}