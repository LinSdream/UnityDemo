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

        GameObject WeaponRight;
        GameObject WeaponLeft;
        Collider WeaponCollider;
        Animator _anim;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            
            WeaponRight = SharedMethods.DeepFindTransform(transform, "WeaponHandle").gameObject;
            WeaponLeft = SharedMethods.DeepFindTransform(transform, "ShieldHandle").gameObject;
            WeaponCollider = WeaponRight.GetComponentsInChildren<Collider>()[0];
        }

        #region Public Methods
        public Collider ChangeWeaponCollider(int index, bool isRight=true)
        {

            if (!isRight)
                WeaponCollider = WeaponRight.GetComponentsInChildren<Collider>()[index];
            else
                WeaponCollider = WeaponLeft.GetComponentsInChildren<Collider>()[index];
            if (WeaponCollider == null)
                WeaponCollider = ChangeWeaponCollider(0,isRight);
            return WeaponCollider;
        }
        #endregion

        #region AnimatorEvents
        public void ResetTrigger(string name)
        {
            _anim.ResetTrigger(name);
        }

        public void WeaponEnable()
        {
            WeaponCollider.enabled = true;
        }

        public void WeaponDisable()
        {
           WeaponCollider.enabled = false;
        }
        #endregion
    }

}