using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS.AIFrameWork.Decisions
{
    public abstract class Decision : ScriptableObject,IDecision<FSMBase>
    {
        public string DecisionName => name;

        public abstract bool Decide(FSMBase controller);

    }

}