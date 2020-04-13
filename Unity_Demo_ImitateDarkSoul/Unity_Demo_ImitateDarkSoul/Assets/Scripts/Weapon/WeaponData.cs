using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{

    public enum WeaponType
    {
        None,
        Shield,
        Sword,
    }

    public class WeaponData : MonoBehaviour
    {
        public WeaponType WType = WeaponType.None;
        public float ATK = 5f;
    }

}