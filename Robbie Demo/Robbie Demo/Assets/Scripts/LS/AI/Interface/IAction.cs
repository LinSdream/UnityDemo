using UnityEngine;
using System.Collections;

namespace LS.AIFrameWork.Actions
{
    public interface IAction<T>
    {
        void Act(T controller);
    }

}