using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.TaskFrameWork
{
    public class TaskEventArgs:EventArgs
    {
        /// <summary> 当前事件ID </summary>
        public string TaskID;
        /// <summary> 子任务的ID，如果存在 </summary>
        public string ID;
        /// <summary>进度 </summary>
        public int Amount;
    }
}
