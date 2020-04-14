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
        public Animator Anim;
    }

    /// <summary>
    /// Animator Motion Message
    /// </summary>
    public class AnimatorMoveEventArgs : EventArgs
    {
        public Vector3 deltaPosition;
    }

    /// <summary>
    /// WeaponUI add new weapon
    /// </summary>
    public class WeaponUIEventArgs : EventArgs
    {
        public string WeaponName;
    }

    /// <summary>
    /// Switch weapons
    /// </summary>
    public class SwitchWeaponEventArgs : EventArgs
    {
        public string CurrentName;
        public string SwitchName;
    }

}