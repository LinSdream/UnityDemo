using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.CustomTaskSystem
{
    [System.Serializable]
    /// <summary> 任务条件 </summary>
    public class TaskCondition
    {
        /// <summary> 条件ID </summary>
        string _id;
        /// <summary> 条件描述 </summary>
        string _description;

        public string ID => _id;
        public string Description => _description;
        /// <summary> 任务的当前进度 </summary>
        public int CurrentAmount;
        /// <summary> 任务的总进度 </summary>
        public int TargetAmount;
        /// <summary> 是否满足完成任务的条件 </summary>
        public bool IsFinish = false;

        public TaskCondition(string iD, string description, int currentAmount, int targetAmount, bool isFinish)
        {
            _id = iD;
            _description = description;
            CurrentAmount = currentAmount;
            TargetAmount = targetAmount;
            IsFinish = isFinish;
        }

    }
}
