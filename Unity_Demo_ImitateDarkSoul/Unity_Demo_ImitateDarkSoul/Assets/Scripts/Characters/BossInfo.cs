using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    [System.Serializable, CreateAssetMenu(menuName = "LS/Boss Character Info")]
    public class BossInfo : CharacterInfo
    {
        public Vector2 BehaviourFrequency;
    }
}