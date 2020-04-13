using System;
using UnityEngine;
using LS.Common;
using System.Collections.Generic;
using LS.Common.Message;

namespace LS.CustomTaskSystem
{

    /// <summary>
    /// 将所有的Task有关的消息进行进一步的处理
    /// </summary>
    public class TaskUIManagerBase<T>:MonoSingletionBasis<T> where T : MonoSingletionBasis<T>
    {


        #region Register and UnRegister
        protected override void Init()
        {

            TaskMonitor.Instance.AddListener("GetTaskUI", GetTaskUI);
            TaskMonitor.Instance.AddListener("UpdateTaskUI", UpdateTaskUI);
            TaskMonitor.Instance.AddListener("FinishTaskUI", FinishTaskUI);
            TaskMonitor.Instance.AddListener("GetRewardUI", GetRewardUI);
        }


        protected  void OnDisable()
        {
            TaskMonitor.Instance.RemoveListener("GetTaskUI", GetTaskUI);
            TaskMonitor.Instance.RemoveListener("UpdateTaskUI", UpdateTaskUI);
            TaskMonitor.Instance.RemoveListener("FinishTaskUI", FinishTaskUI);
            TaskMonitor.Instance.RemoveListener("GetRewardUI", GetRewardUI);

        }

        #endregion

        #region Message
        protected virtual void GetTaskUI(CustomTask render, TaskEventArgs e)
        {

        }

        protected virtual void UpdateTaskUI(CustomTask render, TaskEventArgs e)
        {

        }

        protected virtual void GetRewardUI(CustomTask render, TaskEventArgs e)
        {

        }

        protected virtual void FinishTaskUI(CustomTask render, TaskEventArgs e)
        {

        }
        #endregion

    }
}
