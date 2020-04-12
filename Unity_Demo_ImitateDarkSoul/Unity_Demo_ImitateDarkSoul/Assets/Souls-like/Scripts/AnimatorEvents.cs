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

       
        #endregion
    }

}