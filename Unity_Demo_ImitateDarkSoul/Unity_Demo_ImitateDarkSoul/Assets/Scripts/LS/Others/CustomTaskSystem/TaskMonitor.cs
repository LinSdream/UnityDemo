using System;
using LS.Common.Message;

namespace LS.CustomTaskSystem
{
    /// <summary>
    /// 事件监听器，直接继承自MessageCenter，无其余而外操作
    /// </summary>
    public class TaskMonitor:MessageCenter<CustomTask, TaskEventArgs> {}
}
