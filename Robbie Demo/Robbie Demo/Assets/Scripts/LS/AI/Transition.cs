using LS.AIFrameWork.Decisions;
using LS.AIFrameWork.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS.AIFrameWork
{
    /// <summary>
    /// 状态过渡器
    /// </summary>
    [System.Serializable]
    public struct Transition
    {
        /// <summary> 过渡条件判断 </summary>
        [Tooltip("过渡条件判断")]public Decision Condition;
        /// <summary> 满足条件状态</summary>
        [Tooltip("满足条件转化的状态")]public State TrueState;
        /// <summary> 不满足条件状态</summary>
        [Tooltip("不满足条件转化的状态")] public State FalseState;
    }

}