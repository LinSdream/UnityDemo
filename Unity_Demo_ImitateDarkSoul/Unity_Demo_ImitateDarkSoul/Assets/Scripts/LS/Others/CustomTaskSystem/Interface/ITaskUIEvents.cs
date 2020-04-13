using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.EventSystems;

namespace LS.CustomTaskSystem
{
    public interface ITaskUIEvents: IEventSystemHandler
    {
        void GetTask(CustomTask task);
        void UpdateTask(CustomTask task ,TaskEventArgs e);
         void FinishTask(CustomTask task);
    }
}