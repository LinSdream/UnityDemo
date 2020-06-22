using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LS.Common;

namespace LS.PorpFrameWork
{

    public class PropEventSystemListeners : SingletonBasisNoMono<PropEventSystemListeners>
    {

        private List<GameObject> _listeners = new List<GameObject>();
        public List<GameObject> Listeners => _listeners;

        public void AddListener(GameObject listener)
        {
            if (!_listeners.Contains(listener))
                _listeners.Add(listener);
        }

        public void RemoveListener(GameObject listener)
        {
            _listeners.Remove(listener);
        }

        public void Clear()
        {
            _listeners.Clear();
        }
    }
}
