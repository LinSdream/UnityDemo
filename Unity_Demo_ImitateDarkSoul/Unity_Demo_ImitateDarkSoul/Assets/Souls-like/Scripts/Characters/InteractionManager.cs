using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    /// <summary>交互管理，从BoxCollider碰撞来进行检测是否发生交互 </summary>
    public class InteractionManager : AbstractActorManager
    {
        public List<EventCasterManger> OverlapEcastms = new List<EventCasterManger>();

        CapsuleCollider _collider;

        // Start is called before the first frame update
        void Start()
        {
            _collider = GetComponent<CapsuleCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            EventCasterManger[] eventCasters = other.GetComponents<EventCasterManger>();
            foreach(var cell in eventCasters)
            {
                if (!OverlapEcastms.Contains(cell))
                    OverlapEcastms.Add(cell);

            }
        }

        private void OnTriggerExit(Collider other)
        {
            EventCasterManger[] eventCasters = other.GetComponents<EventCasterManger>();
            foreach (var cell in eventCasters)
            {
                if (OverlapEcastms.Contains(cell))
                    OverlapEcastms.Remove(cell);
            }
        }

    }

}