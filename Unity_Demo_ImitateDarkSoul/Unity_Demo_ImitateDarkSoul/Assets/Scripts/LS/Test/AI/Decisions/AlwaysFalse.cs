using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS.Test.AI.Decisions
{

    [CreateAssetMenu(menuName = "LS/AI/Decisions/AlwaysFalse")]
    public class AlwaysFalse : Decision
    {
        public override bool Decide(FSMBase controller)
        {
            return false;
        }
    }

}