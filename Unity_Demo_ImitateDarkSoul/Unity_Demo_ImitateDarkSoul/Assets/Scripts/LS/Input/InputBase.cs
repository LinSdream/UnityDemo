using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS.CustomInput
{
    public abstract class InputBase : MonoBehaviour
    {
        //output signal
        [HideInInspector] public float Horizontal;
        [HideInInspector] public float Vertical;
        [HideInInspector] public float CameraHorizontal;
        [HideInInspector] public float CameraVertical;

        //others
        public bool LockCursor = true;
    }

}