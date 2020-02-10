using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS.Test.AI.Actions
{

    [System.Serializable]
    public abstract class Action: ScriptableObject,IAction<FSMBase>
    {
        public virtual string ActionName => name;
        public abstract void Act(FSMBase controller);

    }

}