using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LS.AssetsFrameWork
{
    /// <summary>
    /// 单个的AB包中的资源加载类
    /// <para>处理的是包内的Asset资源</para>
    /// </summary>
    public class AssetsLoader : IDisposable
    {
        /// <summary>
        /// 当前AB包
        /// </summary>
        AssetBundle _currentAssets;

        /// <summary>缓存集</summary>
        Dictionary<string, Object> _cache = new Dictionary<string, Object>();

        public AssetsLoader(AssetBundle asset)
        {
            if (asset != null)
                _currentAssets = asset;
            else
                Debug.LogError("ABLoader/ABLoader(AssetsBundle)  Error：invalid param , please check it");
        }

        /// <summary>
        /// 加载当前包中指定数据
        /// </summary>
        public Object LoadAsset(string assetName,bool isCache=false)
        {
            return LoadRes<Object>(assetName, isCache);
        }

        /// <summary>
        /// 加载当前包中指定数据
        /// </summary>
        public Object LoadAsset(string assetName,Type type,bool isCache = false)
        {
            return LoadRes(assetName, type, isCache);
        }

        /// <summary>
        /// 加载当前包中指定数据
        /// </summary>
        /// <typeparam name="T">T 类型继承自 UnityEngine.Object</typeparam>
        /// <param name="assetName">资源名</param>
        /// <param name="isCache">是否缓存</param>
        public T LoadAsset<T>(string assetName, bool isCache = false) where T : Object
        {
            return LoadRes<T>(assetName, isCache);
        }

        /// <summary>
        /// 释放单一资源文件
        /// </summary>
        public bool UnLoadAsset(Object asset)
        {
            if (asset != null)
            {
                string tmp = string.Empty;
                //先移除
                foreach (var pair in _cache)
                    if (pair.Value == asset)// ReferenceEquals
                        tmp = pair.Key;

                if (string.IsNullOrEmpty(tmp))
                    _cache.Remove(tmp);

                //再释放
                Resources.UnloadAsset(asset);
                return true;
            }
            Debug.LogError("ABLoader/UnLoadAsset Error: the param of asset is null");
            return false;
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T">T 类型继承自 UnityEngine.Object</typeparam>
        /// <param name="assetName">资源名</param>
        /// <param name="isCache">是否缓存</param>
        T LoadRes<T>(string assetName, bool isCache) where T : Object
        {
            //缓存集中是否存在
            if (_cache.ContainsKey(assetName))
                return _cache[assetName] as T;
            //加载
            T res = _currentAssets.LoadAsset<T>(assetName);
            if (res != null && isCache)
                _cache.Add(assetName, res);
            else if (res == null)
                Debug.LogError("ABLoader/LoadRes<T> Error: loadasset is null");
            return res;
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="assetName">资源名</param>
        /// <param name="isCache">是否缓存</param>
        Object LoadRes(string assetName,Type type,bool isCache)
        {
            //缓存集中是否存在
            if (_cache.ContainsKey(assetName))
                return _cache[assetName] as Object;
            //加载
            Object res = _currentAssets.LoadAsset(assetName,type);
            if (res != null && isCache)
                _cache.Add(assetName, res);
            else if (res == null)
                Debug.LogError("ABLoader/LoadRes<T> Error: loadasset is null");
            return res;
        }

        /// <summary>
        /// 释放当前AB内存镜像资源
        /// </summary>
        public void Dispose()
        {
            _currentAssets.Unload(false);
            _cache.Clear();
        }

        /// <summary>
        /// 释放当前AB内存镜像资源,且释放内存资源
        /// </summary>
        public void DisposeAll()
        {
            _currentAssets.Unload(true);
            _cache.Clear();
        }

        /// <summary>
        /// 查询当前AB包包含的所有资源名称
        /// </summary>
        public string[] GetAllAssetName()
        {
            return _currentAssets.GetAllAssetNames();
        }

    }
}
