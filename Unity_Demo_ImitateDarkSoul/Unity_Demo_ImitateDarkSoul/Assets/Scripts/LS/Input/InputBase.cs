using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS.CustomInput
{
    public abstract class InputBase : MonoBehaviour
    {
        //output signal
        [HideInInspector] public float Horizontal { protected set; get; }
        [HideInInspector] public float Vertical { protected set; get; }
        [HideInInspector] public float CameraHorizontal { protected set; get; }
        [HideInInspector] public float CameraVertical { protected set; get; }

        //others
        public bool LockCursor = true;
    }

}