using UnityEngine;
using LS.Common;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;
using LS.Common.Message;

namespace LS.TaskFrameWork
{
    /// 任务管理器
    /// Time:  2020.4.5
    /// Latest Time：2020.4.9
    /// 
    ///log：添加对外push的列表

    public delegate void TaskEventArgsHandle(TaskEventArgs e);
    public delegate void CustomTaskHandle(CustomTask task, TaskEventArgs e);
    /// <summary> 任务管理器 </summary>
    public class CustomTaskManager : MonoSingletonBasis<CustomTaskManager>
    {

        #region Fields

        [Tooltip("任务配置表")] public CustomTaskListScriptable TaskListInScript;

        //事件组

        /// <summary> 成功获得任务事件 </summary>
        public event CustomTaskHandle GetTaskEvent;
        /// <summary> 检查任务进度事件 </summary>
        public event TaskEventArgsHandle CheckTaskEvent;//检查任务进度时候，没有具体的任务实体，只能通过消息体中的任务ID或条件ID去获取
        /// <summary> 完成任务事件 </summary>
        public event CustomTaskHandle FinishTaskEvent;
        /// <summary> 获得奖励事件 </summary>
        public event CustomTaskHandle GetRewardTaskEvent;

        /// <summary> 获取当前的任务表 </summary>
        public List<CustomTask> TaskList
        {
            get
            {
                var list = new List<CustomTask>();
                foreach (var cell in _taskDic)
                    list.Add(cell.Value);
                return list;
            }
        }

        private Dictionary<string, CustomTask> _taskDic = new Dictionary<string, CustomTask>();

        #endregion

        #region Task Methods

        /// <summary>获取新的任务</summary>
        /// <param name="id">任务ID</param>
        public void GetTask(string id)
        {
            if (_taskDic.ContainsKey(id))
            {
                Debug.LogError("CustomTaskManager/GetTask Warning : the task has exist ");
                return;
            }
            foreach (var cell in TaskListInScript.TaskInfos)
            {
                if (cell.ID == id)
                {
                    List<TaskCondition> conditions = new List<TaskCondition>();
                    for (int i = 0; i < cell.TaskConditions.Length; i++)
                    {
                        conditions.Add(new TaskCondition(cell.TaskConditions[i].ID,cell.TaskConditions[i].Description,
                            0, cell.TaskConditions[i].Amount, false));
                    }
                    CustomTask task = new CustomTask(id, cell.Name, cell.Description, cell.Reward, conditions);
                    _taskDic.Add(id, task);
                    var e = new TaskEventArgs() { TaskID = id };
                    //发送消息，让UI进行相对应的处理
                    TaskMonitor.Instance.SendMessage("GetTaskUI", task, e);
                    GetTaskEvent?.Invoke(task, e);
                    return;
                }
            }
            Debug.LogWarning("CustomTaskManager/GetTask Warning : the task doesn't exist");
        }

        /// <summary> 检查任务进度 </summary>
        public void CheckTask(TaskEventArgs e)
        {
            //如果获得到的消息中不包含具体的任务ID，则需要一个个找
            if (e.TaskID == default || e.TaskID == null)
            {
                foreach (var cell in _taskDic)
                {
                    //有可能多个任务中的条件是相同的，所以得确保当前所有的任务都检查一遍
                    cell.Value.Check(e);
                }
            }
            else
            {
                if (_taskDic.ContainsKey(e.TaskID))
                    _taskDic[e.TaskID].Check(e);
                else
                    Debug.LogWarning("CustomTaskManager/CheckTask Warning:can't find the task");
            }
            CheckTaskEvent?.Invoke(e);
        }

        public void FinishTask(CustomTask task, TaskEventArgs e)
        {
            //发送消息，通告UI任务完成
            TaskMonitor.Instance.SendMessage("FinishTaskUI", task, e);
            FinishTaskEvent?.Invoke(task, e);
        }

        public void GetReward(CustomTask task,TaskEventArgs e)
        {
            if (_taskDic.ContainsKey(task.ID))
                _taskDic.Remove(task.ID);

            task.GetReward();
            //发送消息，通告UI任务奖励已经领取
            TaskMonitor.Instance.SendMessage("GetRewardUI", task);
            GetRewardTaskEvent?.Invoke(task, e);
        }

        #endregion

        #region Save/Load Task and Other Methods
        /// <summary> 保存任务信息，将其保存到存档中 </summary>
        public void SaveTask()
        {

        }

        /// <summary> 从存档中获取任务 </summary>
        public Dictionary<string,CustomTask> LoadTask()
        {
            //获取到的Task list要把其中的Reward.TaskRewardInfo.Reward从TaskListInScript引用上对应的奖励
            return default;
        }

        /// <summary> 初始化任务列表 </summary>
        public bool InitTaskList()
        {
            //从存档中获取任务列表，存在true,不存在即_taskDic.Count==0返回false
            return false;
        }

        /// <summary> 获取任务实体 </summary>
        public CustomTask GetTaskInfo(string id)
        {
            return default;
        }


        public void ClearAllTask()
        {
            _taskDic.Clear();
        }
        #endregion
    }

}
