﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{

    public enum WeaponType
    {
        None,
        Shield,
        Sword,
        WoodenClub,
    }

    public class WeaponData : MonoBehaviour
    {
        public string WeaponName;
        public WeaponType WType = WeaponType.None;
        public float ATK = 5f;
    }

}