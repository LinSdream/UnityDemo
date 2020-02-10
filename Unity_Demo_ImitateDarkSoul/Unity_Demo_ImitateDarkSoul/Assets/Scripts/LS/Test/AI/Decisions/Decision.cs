using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS.Test.AI.Decisions
{
    public abstract class Decision : ScriptableObject,IDecision<FSMBase>
    {
        public virtual string DecisionName =>name;
        public abstract bool Decide(FSMBase controller);
    }

}