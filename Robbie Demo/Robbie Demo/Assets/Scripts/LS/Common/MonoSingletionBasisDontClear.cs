using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS.Common

{
    /*
     * 参考文档：https://blog.csdn.net/ycl295644/article/details/49487361/
     */

    /// 单例类基类，继承MonoBehaviour,切换场景后不会自动删除
    /// Time:  2018.5.12
    /// Latest Time：2019.11.29
    /// 
    /// Update Log:
    /// 2019.11.29：
    ///         删除  protected virtual void Start() 这个空函数
    ///         更改Init访问级别 public -> protected
    /// 
    /// <summary>
    /// 单例类基类，继承MonoBehaviour,切换场景后不会自动删除
    /// </summary>
    public abstract class MonoSingletionBasisDontClear<T> : MonoBehaviour where T : MonoSingletionBasisDontClear<T>//where 约束子类
    {

        protected bool _IsDestroy = false;

        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)//如果不存在
                {
                    _instance = FindObjectOfType(typeof(T)) as T;//在目前已加载的脚本中查找该单例
                    if (_instance == null)//创建单例
                    {
                        GameObject obj = new GameObject("Main :" + typeof(T).Name);
                        _instance = obj.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Awake,尽可能不要去重写该方法，如需初始化，请调用Init方法
        /// </summary>
        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);

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

    }
}