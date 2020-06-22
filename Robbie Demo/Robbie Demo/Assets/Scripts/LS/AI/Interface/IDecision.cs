using UnityEngine;
using System.Collections;

namespace LS.AIFrameWork.Decisions
{
    public interface IDecision<T>
    {
        bool Decide(T controller);
    }

}