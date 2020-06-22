using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LS.TaskFrameWork
{
    public abstract class TaskRewardInfo:ScriptableObject
    {
        public abstract void Reward();
    }
}
