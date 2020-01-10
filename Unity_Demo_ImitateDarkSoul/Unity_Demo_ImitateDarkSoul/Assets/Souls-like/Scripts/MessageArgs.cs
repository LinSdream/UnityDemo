using UnityEngine;
using UnityEditor;
using System;

namespace Souls
{
    public class MessageArgs : EventArgs { }

    /// <summary>
    /// FSM Message
    /// </summary>
    public class FSMEventArgs : EventArgs
    {
        public AnimatorStateInfo StateInfo;
        public int LayerIndex;
    }

    /// <summary>
    /// Animator Motion Message
    /// </summary>
    public class AnimatorMoveEventArgs : EventArgs
    {
        public Vector3 deltaPosition;
    }

    /// <summary>
    /// Camera Lock Message
    /// </summary>
    public class CameraLockOnEventArgs : EventArgs
    {
        public Vector3 OriginEuler;
        public Vector3 TargetEuler;
    }
}