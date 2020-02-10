using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS.Test.AI.Actions
{

    [System.Serializable]
    public abstract class Action: ScriptableObject,IAction<FSMBase>
    {
        public abstract void Act(FSMBase controller);

        public string GetActionName()
        {
            return name;
        }
    }

}