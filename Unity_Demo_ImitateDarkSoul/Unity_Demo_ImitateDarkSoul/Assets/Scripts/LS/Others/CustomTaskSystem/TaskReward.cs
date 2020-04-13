using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LS.CustomTaskSystem
{
    [System.Serializable]
    public class TaskReward
    {
        /// <summary> 奖励ID </summary>
        public string ID;
        /// <summary> 奖励描述 </summary>
        public string Description;
        /// <summary> 奖励 </summary>
        public TaskRewardInfo Reward;//用事件来决定具体的奖励内容
    }
}
