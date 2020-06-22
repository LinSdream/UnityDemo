using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LS.PorpFrameWork
{
    public interface IPropEvents<T> : IEventSystemHandler where T:Component
    {
        /// <summary> 获得道具 </summary>
        void OnPropCollected(BaseProp<T> prop, T player);

        /// <summary> 道具效果到期 </summary>
        void OnPropExpired(BaseProp<T> prop, T player);
    }

}