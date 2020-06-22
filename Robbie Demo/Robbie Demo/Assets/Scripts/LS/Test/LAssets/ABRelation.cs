using System;
using System.Collections.Generic;
using UnityEngine;

namespace LS.AssetsFrameWork
{
    /*
     *  AB包的关系类
     * 
     *  Description：
     *      用于存储AB包之间的所有的依赖关系包与应用关系包
     */

    /// <summary>
    /// AB包的关系类
    /// </summary>
    public class ABRelation
    {
        string _abName;

        List<string> _allDependenceAB = new List<string>();
        List<string> _allReferenceAB = new List<string>();

        public ABRelation(string abName)
        {
            _abName = abName;
        }

        ///<summary>添加依赖关系</summary>
        public void AddDependence(string abName)
        {
            if (_allDependenceAB.Contains(abName))
                _allDependenceAB.Add(abName);
        }

        /// <summary>
        /// 移除依赖关系
        /// <para>true 此AB包没有依赖项</para>
        /// <para>false 此AB包还有其他的依赖项</para>
        /// </summary>
        /// <param name="abName"></param>
        public bool RemoveDependence(string abName)
        {
            if (_allDependenceAB.Contains(abName))
            {
                _allDependenceAB.Remove(abName);
            }

            if (_allDependenceAB.Count > 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 获取所有的依赖包，包名
        /// </summary>
        public string[] GetAllDependenceABs()
        {
            return _allDependenceAB.ToArray();
        }

        /// <summary>
        /// 增加引用关系
        /// </summary>
        public void AddReference(string abName)
        {
            if (!_allReferenceAB.Contains(abName))
                _allReferenceAB.Add(abName);
            else
                Debug.LogWarning($"ABRelation/AddReference Warning : the asset is already have the reference package whitch name is {abName}");
        }

        /// <summary>
        /// 移除引用关系
        /// <para>true 此AB包没有依赖项</para>
        /// <para>false 此AB包还有其他的依赖项</para>
        /// </summary>
        public bool RemoveReference(string abName)
        {
            if (_allReferenceAB.Contains(abName))
                _allReferenceAB.Remove(abName);
            if (_allReferenceAB.Count > 0)
                return false;
            else return true;
        }

        /// <summary>
        /// 获取所有的引用包的包名
        /// </summary>
        public string[] GetAllReference()
        {
            return _allReferenceAB.ToArray();
        }
    }
}
