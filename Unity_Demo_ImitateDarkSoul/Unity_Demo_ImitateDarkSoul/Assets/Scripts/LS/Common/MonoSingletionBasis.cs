using UnityEngine;

namespace LS.Common
{
    public abstract class MonoSingletionBasis<T> : MonoBehaviour where T : MonoSingletionBasis<T>//where 约束子类
    {

        protected static T _instance;
        public static T Instance
        {
            get
            {
                //if (_instance == null)//如果不存在
                //{
                //    _instance = FindObjectOfType(typeof(T)) as T;//在目前已加载的脚本中查找该单例
                //    if (_instance == null)//创建单例
                //    {
                //        GameObject obj = new GameObject("Main :" + typeof(T).Name);
                //        _instance = obj.AddComponent<T>();
                //    }
                //}
                return _instance;
            }
        }

        /// <summary>
        /// Awake,尽可能不要去重写该方法，如需初始化，请调用Init方法
        /// </summary>
        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                Init();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// 初始化该单例
        /// </summary>
        protected virtual void Init()
        {

        }

        /// <summary>
        /// 销毁该单例
        /// </summary>
        public virtual void Release()
        {

        }
    }
}
