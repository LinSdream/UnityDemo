using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class AnimatorEvents : MonoBehaviour
    {

        public string WeaponUrl = "mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand/WeaponHandle";

        GameObject Weapon;
        Collider WeaponCollider;
        Animator _anim;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
            Weapon = gameObject.transform.Find(WeaponUrl).gameObject;
            WeaponCollider = Weapon.GetComponentsInChildren<Collider>()[0];
        }

        #region Public Methods
        public Collider ChangeWeaponCollider(int index)
        {
            WeaponCollider = Weapon.GetComponentsInChildren<Collider>()[index];
            if (WeaponCollider == null)
                WeaponCollider = ChangeWeaponCollider(0);
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