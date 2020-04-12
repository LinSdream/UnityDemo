using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{

    /// <summary>
    /// 交互事件控制器
    /// </summary>
    public class EventCasterManger : AbstractActorManager
    {

        private void Awake()
        {
            if (AM == null)
                AM = GetComponentInParent<ActorManager>();
        }

        /// <summary>事件名称 </summary>
        public string EventName;
        /// <summary> 是否激活 </summary>
        public bool IsActive = false;
    }

}