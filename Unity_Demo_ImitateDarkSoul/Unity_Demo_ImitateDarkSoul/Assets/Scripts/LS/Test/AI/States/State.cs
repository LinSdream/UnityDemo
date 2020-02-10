using LS.Test.AI.Actions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LS.Test.AI.States
{
    [System.Serializable]
    public class State : ScriptableObject,IState<FSMBase>
    {

        [Tooltip("事件列表")] public Action[] Actions;
        [Tooltip("判断列表")] public Transition[] Transitions;

        #region Callbacks Methods
        public virtual void OnEnter(FSMBase controller)
        {

        }

        public virtual void OnUpdate(FSMBase controller)
        {
            DoActions(controller);
            CheckTransition(controller);
        }

        public virtual void OnExit(FSMBase controller)
        {

        }

        #endregion

        #region Private Methods
        private void DoActions(FSMBase controller)
        {
            for (int i = 0; i < Actions.Length; i++)
                Actions[i].Act(controller);
        }

        //检查状态转换器中的所有过渡条件
        private void CheckTransition(FSMBase controller)
        {
            for (int i = 0; i < Transitions.Length; i++)
            {
                bool res = Transitions[i].Condition.Decide(controller);
                if (res)
                {
                    if (Transitions[i].TrueState == null)
                        continue;
                    controller.TransitionToState(Transitions[i].TrueState);
                }
                else
                {
                    if (Transitions[i].FalseState == null)
                        continue;
                    controller.TransitionToState(Transitions[i].FalseState);
                }
            }
        }
        #endregion

        #region Public Methods

        public string GetStateName()
        {
            return name;
        }

        public bool HasAction(string name)
        {
            var res = from action in Actions where action.GetActionName() == name select action;
            if (res == null)
                return false;
            return true;
        }
        #endregion

        #region ScriptableObject Callbacks
        private void OnValidate()
        {
            for (int i = 0; i < Transitions.Length; i++)
                if (Transitions[i].Condition == null)
                    Debug.LogError(name + "/Error : Invalid Value in Transition list,list number is " + i);
        }

        #endregion
    }

}