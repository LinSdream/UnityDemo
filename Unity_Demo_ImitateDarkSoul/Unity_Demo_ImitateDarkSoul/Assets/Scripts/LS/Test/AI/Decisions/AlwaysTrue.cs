using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS.Test.AI.Decisions
{

    [CreateAssetMenu(menuName = "LS/AI/Decisions/AlwaysTrue")]
    public class AlwaysTrue : Decision
    {
        public override bool Decide(FSMBase controller)
        {
            return true;
        }
    }

}