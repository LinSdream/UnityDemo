using UnityEngine;
using System.Collections;

namespace LS.Test.AI
{
    public interface IDecision<T>
    {
        bool Decide(T controller);
    }

}