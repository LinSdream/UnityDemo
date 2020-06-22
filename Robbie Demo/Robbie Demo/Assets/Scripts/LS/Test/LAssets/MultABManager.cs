using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Object = UnityEngine.Object;

namespace LS.AssetsFrameWork
{
    public class MultABManager
    {

        #region Fileds

        ///<summary>引用类"单个AB包加载"</summary>
        SingleAssetBundleLoader _currentSingleABLoader;
        ///<summary>AB包的缓存集合，用于防止重复加载(name,asset)</summary>
        Dictionary<string, SingleAssetBundleLoader> _singleABLoaderCache = new Dictionary<string, SingleAssetBundleLoader>();
        ///<summary>AB包的包名</summary>
        string _currentABName;
        ///<summary>AB包与其对应依赖关系 name- abrelation</summary>
        Dictionary<string, ABRelation> _abRelation = new Dictionary<string, ABRelation>();
        ///<summary>完成加载后的回调</summary>
        DelLoadCompleteHandle LoadAllABPackageCompleteCallback;

        #endregion

        #region Publkic Methods

        public MultABManager(string abName, DelLoadCompleteHandle cb = null)
        {
            _currentABName = abName;
            LoadAllABPackageCompleteCallback = cb;
        }

        /// <summary>
        /// 加载AB包
        /// </summary>
        /// <param name="abName">AB包包名</param>
        public IEnumerator LoadAB(string abName, LoadAssetFromWhere where = LoadAssetFromWhere.OnLine)
        {
            ABRelation rela;
            //建立AB包关系
            if (!_abRelation.ContainsKey(abName))
            {
                rela = new ABRelation(abName);
                _abRelation.Add(abName, rela);
            }
            rela = _abRelation[abName];

            //获取AB包的所有依赖关系
            string[] dependeceArr = ABManifestLoader.Instance.GetDependce(abName);
            foreach (var cell in dependeceArr)
            {
                //添加依赖项
                rela.AddDependence(cell);
                //加载引用项
                yield return LoadReference(cell, abName, where);
            }

            //加载AB包
            if (_singleABLoaderCache.ContainsKey(abName))
                yield return _singleABLoaderCache[abName].LoadAB();
            else
            {
                _currentSingleABLoader = new SingleAssetBundleLoader(abName, CompleteLoadAB, where);
                _singleABLoaderCache.Add(abName, _currentSingleABLoader);
                yield return _currentSingleABLoader.LoadAB();
            }
            yield return null;
        }



        /// <summary>
        /// 加载AB包中的资源
        /// </summary>
        /// <param name="abName">包名</param>
        /// <param name="assetName">资源名</param>
        /// <param name="isCache">是否缓存</param>
        public Object LoadAsset(string abName, string assetName, bool isCache)
        {

            //foreach (var cell in _singleABLoaderCache.Keys)
            //{
            //    if (cell == abName)
            //        return _singleABLoaderCache[cell].LoadAsset(assetName, isCache);
            //}
            if (_singleABLoaderCache.ContainsKey(abName))
                return _singleABLoaderCache[abName].LoadAsset(assetName, isCache);
            Debug.LogError($"MultABManager/LoadAsset Error: Don't find AB package, the ab package name is {abName} , asset name is {assetName}");
            return null;
        }

        public Object LoadAsset(string abName, string assetName, Type type, bool isCache)
        {
            //foreach (var cell in _singleABLoaderCache.Keys)
            //{
            //    if (cell == abName)
            //        return _singleABLoaderCache[cell].LoadAsset(assetName, type, isCache);
            //}
            if (_singleABLoaderCache.ContainsKey(abName))
                return _singleABLoaderCache[abName].LoadAsset(assetName, type, isCache);
            Debug.LogError($"MultABManager/LoadAsset Error: Don't find AB package, the ab package name is {abName} , asset name is {assetName}");
            return null;
        }

        public T LoadAsset<T>(string abName, string assetName, bool isCache) where T : Object
        {
            //foreach (var cell in _singleABLoaderCache.Keys)
            //{
            //    if (cell == abName)
            //        return _singleABLoaderCache[cell].LoadAsset<T>(assetName, isCache);
            //}
            if (_singleABLoaderCache.ContainsKey(abName))
                return _singleABLoaderCache[abName].LoadAsset<T>(assetName, isCache);
            Debug.LogError($"MultABManager/LoadAsset Error: Don't find AB package, the ab package name is {abName} , asset name is {assetName}");
            return null;
        }

        public bool UnloadAsset(Object asset)
        {
            return _currentSingleABLoader.UnLoadAsset(asset);
        }


        public void UnloadAssets()
        {
            try
            {
                foreach (var item in _singleABLoaderCache.Values)
                {
                    item.Dispose();
                }
            }
            finally
            {
                _singleABLoaderCache.Clear();
                _singleABLoaderCache = null;

                //释放其他对象占用资源
                _abRelation.Clear();
                _abRelation = null;
                _currentABName = null;
                LoadAllABPackageCompleteCallback = null;

                //卸载没有用到的资源
                Resources.UnloadUnusedAssets();
                //强制GC回收
                System.GC.Collect();
            }
        }

        /// <summary>
        /// 释放所有的ab包资源
        /// </summary>
        public void DisposeAllAssets()
        {
            try
            {
                //逐一释放加载过多的AB包中的资源
                foreach (var item in _singleABLoaderCache.Values)
                {
                    item.DisposeAll();
                }
            }
            finally
            {
                _singleABLoaderCache.Clear();
                _singleABLoaderCache = null;

                //释放其他对象占用资源
                _abRelation.Clear();
                _abRelation = null;
                _currentABName = null;
                LoadAllABPackageCompleteCallback = null;

                //卸载没有用到的资源
                Resources.UnloadUnusedAssets();
                //强制GC回收
                System.GC.Collect();
            }
        }

        public bool ContainsDependentAB(string abName)
        {
            return _singleABLoaderCache.ContainsKey(abName);
        }

        /// <summary>
        /// 获取当前Assets的资源名,包含依赖包
        /// </summary>
        public string[] GetAllAssetsNameIncludeDepence()
        {
            List<string> names = new List<string>();
            foreach (var pair in _singleABLoaderCache)
            {
                names.AddRange(pair.Value.GetAllAssetName());
            }
            return names.ToArray();
        }

        /// <summary>
        /// 获取当前Assets的资源名
        /// </summary>
        public string[] GetAllAssetsName()
        {
            return _currentSingleABLoader.GetAllAssetName();
        }

        public string[] GetAllAssetBundleNames()
        {
            string[] names = new string[_singleABLoaderCache.Count];
            int index = 0;
            foreach (var name in _singleABLoaderCache.Keys)
            {
                names[index] = name;
                index++;
            }
            return names;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 完成指定AB包调用
        /// </summary>
        void CompleteLoadAB(string abName)
        {
            if (_currentABName == abName)
                LoadAllABPackageCompleteCallback?.Invoke(abName);
        }

        /// <summary>
        /// 加载引用AB包
        /// </summary>
        IEnumerator LoadReference(string abName, string refName, LoadAssetFromWhere where)
        {
            ABRelation tmpABRelation;
            if (_abRelation.ContainsKey(abName))
            {
                tmpABRelation = _abRelation[abName];
                //添加AB包引用关系(被依赖)
                tmpABRelation.AddReference(refName);
            }
            else
            {
                tmpABRelation = new ABRelation(abName);
                tmpABRelation.AddReference(refName);
                _abRelation.Add(abName, tmpABRelation);

                //开始加载依赖包(递归)
                yield return LoadAB(abName, where);
            }

            yield return null;
        }

        #endregion
    }
}
