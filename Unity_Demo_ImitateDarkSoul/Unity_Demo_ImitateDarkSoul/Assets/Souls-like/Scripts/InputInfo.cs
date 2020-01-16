using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public struct InputInfo
    {
        private string _inputName;

        public InputInfo(string name)
        {
            _inputName = name;
        }

        public bool OnTrigger => Input.GetButtonDown(_inputName);
        public bool OnPressed => Input.GetButton(_inputName);
        public string Name => _inputName;
    }

}