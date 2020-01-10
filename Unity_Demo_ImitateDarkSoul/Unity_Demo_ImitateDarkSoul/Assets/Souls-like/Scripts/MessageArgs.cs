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

    public class IsInGroundEventArgs : EventArgs
    {
        public bool IsInGround;
    }

}