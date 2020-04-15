using LS.Test.AI;
using LS.Test.AI.Decisions;
using Souls.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    [CreateAssetMenu(menuName = "Souls/EnemyAI/Decision/FindPlayer")]
    public class FindPlayerDecision : Decision
    {
        public float SqrDistance = 2500f;
        public float HalfAngle = 45;

        public override bool Decide(FSMBase controller)
        {
            var check = (controller as BlackKnightFSM).IsInArea(SqrDistance, HalfAngle);
            return check;
        }
    }

}