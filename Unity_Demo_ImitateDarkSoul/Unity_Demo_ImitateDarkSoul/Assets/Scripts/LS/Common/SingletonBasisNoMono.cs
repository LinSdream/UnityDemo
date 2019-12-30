using UnityEngine;
using System.Collections;

namespace LS.Common
{
    public class SingletonBasisNoMono<T> where T : class, new()
    {
        private static T _instance;
        private static readonly object _syncObj = new object();

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncObj)
                    {
                        if (_instance == null)
                            _instance = new T();
                    }
                }
                return _instance;
            }
        }
    }

}