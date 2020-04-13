using LS.Test.AI.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS.Test.AI
{

    [System.Serializable]
    public class FSMBase : MonoBehaviour
    {
        #region  Public Fields
        /// <summary> 进入FSM的初始状态 </summary>
        public State EnterState;
        /// <summary> 当前状态 </summary>
        [HideInInspector] public State CurrentState { get; protected set; }
        /// <summary> 上一个状态 </summary>
        [HideInInspector] public State PreviousState { get; protected set; }
        #endregion

        #region Public Fields
        /// <summary> 目标对象的GameObject，目标对象应Actions的不同具有不同含义 </summary>
        public GameObject TargetGameObject;
        #endregion  

        #region MonoBehaviour Callbacks
        private void Start()
        {
            OnStart();

            CurrentState = EnterState;
            CurrentState.OnEnter(this);
        }

        private void Update()
        {
            if (CurrentState != null)
                CurrentState.OnUpdate(this);

            OnUpdate();
        }
        #endregion

        #region Public Methods
        public void TransitionToState(State nextState)
        {
            CurrentState.OnExit(this);
            nextState.OnEnter(this);
            PreviousState = CurrentState;
            CurrentState = nextState;
        }

        #endregion

        #region Virtual Methods
        protected virtual void OnUpdate()
        {

        }

        protected virtual void OnStart()
        {

        }
        #endregion
    }


}