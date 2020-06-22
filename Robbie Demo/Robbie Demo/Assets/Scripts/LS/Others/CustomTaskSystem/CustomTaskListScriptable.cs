using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LS.TaskFrameWork
{
    [System.Serializable]
    public struct TaskConditionInfo
    {
        public string ID;
        public string Description;
        public int Amount;
    }

    /// <summary> 任务信息</summary>
    [System.Serializable]
    public struct TaskInfo
    {
        public string ID;
        /// <summary> 任务名称 </summary>
        public string Name;
        /// <summary> 任务描述 </summary>
        public string Description;
        /// <summary> 任务奖励 </summary>
        public TaskReward Reward;
        /// <summary> 任务目标</summary>
        [Tooltip("任务的条件")]public TaskConditionInfo[] TaskConditions;
    }

    /// <summary>/ 任务配置信息表 </summary>
    [CreateAssetMenu(menuName = "LS/Tasks")]
    public class CustomTaskListScriptable:ScriptableObject
    {
            public TaskInfo[] TaskInfos;
    }

}
