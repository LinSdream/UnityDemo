using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{

    [System.Serializable,CreateAssetMenu(menuName = "LS/Character Info")]
    public class CharacterInfo : ScriptableObject
    {
        public float HP;
        public float Damage;
    }

}