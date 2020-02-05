using UnityEngine;
using System.Collections;

namespace LS.Test.AI
{
    public interface IAction<T>
    {
        void Act(T controller);
    }

}