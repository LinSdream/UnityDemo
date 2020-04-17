using LS.Common;
using Souls.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{

    [CreateAssetMenu(menuName ="Souls/Info/BossAttackWeighted")]
    public class BossAttackWeighted : ScriptableObject
    {
        public WeightedRandomValue[] Weighted;
    }
}
