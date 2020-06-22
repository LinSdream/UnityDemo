using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS.AIFrameWork.States
{
    /// <summary>
    /// 所有FSM的状态的接口
    /// </summary>
    /// <typeparam name="T">FSM类型</typeparam>
    public interface IState<T>
    {
        void OnEnter(T controller);
        void OnUpdate(T controller);
        void OnExit(T controller);
    }

}