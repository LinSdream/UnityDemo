using System.Collections.Generic;

namespace LS.TaskFrameWork
{
    /// <summary> 任务类 </summary>
    public class CustomTask
    {

        /// <summary> 任务ID ,ID=Name+int，即可以存在同名，但奖励不同的任务</summary>
        string _id;
        /// <summary> 任务名称 </summary>
        string _name;
        /// <summary> 任务描述 </summary>
        string _description;
        /// <summary> 任务奖励 </summary>
        TaskReward _reward;
        /// <summary> 任务目标</summary>
        List<TaskCondition> _taskConditions = new List<TaskCondition>();

        //封装字段，保护CustomTask的信息不被外界更改
        public string ID => _id;
        public string Name => _name;
        public string Description => _description;
        public TaskReward Reward => _reward;
        public List<TaskCondition> TaskConditions => _taskConditions;

        /// <summary> 是否完成任务 </summary>
        public bool IsFinish
        {
            get
            {
                foreach (var cell in _taskConditions)
                    if (!cell.IsFinish)
                        return false;
                return true;
            }
        }

        public CustomTask(string id, string name, string description, TaskReward taskReward, List<TaskCondition> taskConditions)
        {
            _id = id;
            _name = name;
            _description = description;
            _taskConditions = taskConditions;
            _reward = taskReward;
        }

        /// <summary>检查任务进度 </summary>
        /// <param name="e">外界进度消息</param>\
        /// <param name="flag"> 辅助CustomTaskManager.CheckTask的遍历 </param>
        public void Check(TaskEventArgs e)
        {
            foreach(var cell in _taskConditions)
            {
                if(cell.ID==e.ID)
                {
                    cell.CurrentAmount += e.Amount;
                    if (cell.CurrentAmount < 0)
                        cell.CurrentAmount = 0;
                    else if (cell.CurrentAmount >= cell.TargetAmount)
                        cell.IsFinish = true;
                    else
                        cell.IsFinish = false;

                    //确保传出去的消息体中包含任务的ID
                    e.TaskID = ID;
                    //将Amount更改为最新
                    e.Amount = cell.CurrentAmount;
                    //更新UI
                    TaskMonitor.Instance.SendMessage("UpdateTaskUI", this, e);
                    break;
                }
            }
            //如果完成任务
            if (IsFinish)
                CustomTaskManager.Instance.FinishTask(this,e);
        }

        /// <summary> 获得奖励 </summary>
        public void GetReward()
        {
            _reward.Reward.Reward();
        }

    }
}
