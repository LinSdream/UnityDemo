using System;
using UnityEngine;
using LS.Common;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace LS.AssetsFrameWork
{
    /*
     *  框架原型：
     *      ABFramework github : https://github.com/LinSdream/ABFrameWorkWithSence
     *      ABFramework框架的知乎：https://www.jianshu.com/p/94081178567f
     *      
     *  LaterTime: 2020.6.11
     *  
     *  Use：
     *      1. 调用LoadAB 进行AB包的加载
     *      2. 调用LoadAsset 进行资源的加载
     *  
     *  Description：
     *      负责资源加载，三种资源加载方式 AssetBundle 、Resource和 Addressables 后两者暂时未实现。
     *      通过场景名与AssetBundle 包进行关联。
     * 
     *  BugsOrQuestions:
     *      1. 同名但是不同路径的资源加载
     *      2. 同一Assetbundle的包资源加载，通过的方式是包名进行判断，应该改为hash，看论坛中有一些包名相同，但是资源不同，这里暂时不知道如何模拟
     *      3. 目前同名的包可以进行一个简单的判断，同步可行，但是异步就会报错
     * 
     */


    /// <summary>
    /// 资源加载
    /// </summary>
    public class LAssets : MonoSingletionBasisDontClear<LAssets>
    {
        public enum AssetsType
        {
            /// <summary>
            /// AB包资源加载
            /// </summary>
            AssetBundle,
            /// <summary>
            /// Resource方式加载
            /// </summary>
            Resource,
            /// <summary>
            /// 可寻访地址方式加载（内核还是AB）
            /// </summary>
            Addressables,
        }

        /// <summary>
        /// 默认的资源加载方式
        /// </summary>
        public AssetsType LAssetsDefaultType = AssetsType.AssetBundle;

        #region AssetsBundle

        /// <summary>
        /// AB清单文件
        /// </summary>
        AssetBundleManifest _manifest;

        /// <summary>
        /// 建立AB包的索引关系(ab name - ab asset)
        /// </summary>
        Dictionary<string, MultABManager> _allAssetsBundlePackages = new Dictionary<string, MultABManager>();

        /// <summary>
        /// 加载AB包
        /// </summary>
        /// <param name="abName">AB包名</param>
        /// <param name="loadAllCompletedCallback">全部资源加载完成后的回调事件</param>
        public IEnumerator LoadAB(string abName, DelLoadCompleteHandle loadAllCompletedCallback = null, LoadAssetFromWhere where = LoadAssetFromWhere.OnLine)
        {
            if (string.IsNullOrEmpty(abName))
            {
                Debug.LogError("LAssets/LoadAB Error ： abName is null or empty");
            }

            //如果已经存在，说明已经加载过了，无需再次加载
            if (_allAssetsBundlePackages.ContainsKey(abName))
            {
                Debug.LogWarning($"LAssets/LoadAB Warning : the assetbundle file is already loaded the name is {abName}");
                yield break;
            }
            else
            {
                //从依赖包中查找信息，如果已经加载则停止
                foreach (var item in _allAssetsBundlePackages.Values)
                {
                    if (item.ContainsDependentAB(abName))
                    {
                        Debug.LogWarning($"LAssets/LoadAB Warning : the assetbundle file is already loaded the name is {abName}");
                        yield break;
                    }
                }
            }

            if (ABManifestLoader.Instance.ManifestIsNull)
            {
                StartCoroutine(ABManifestLoader.Instance.LoadManifestFile(where));
            }

            //等待Manifest清单文件的加载完成
            while (!ABManifestLoader.Instance.IsLoadFinish)
            {
                yield return null;
            }
            _manifest = ABManifestLoader.Instance.GetABManifest();//获取清单文件
            if (_manifest == null)
            {
                Debug.LogError($"LAssets/LoadAB Error: the manifest is null");
                yield break;
            }

            MultABManager multABMgr = null;

            //把当前的包添加进集合中
            if (!_allAssetsBundlePackages.ContainsKey(abName))
            {
                multABMgr = new MultABManager(abName, loadAllCompletedCallback);
                _allAssetsBundlePackages.Add(abName, multABMgr);
            }
            else
            {
                multABMgr = _allAssetsBundlePackages[abName];
            }

            if (multABMgr == null)
            {
                Debug.LogError("LAssets/LoadAB Error : multAbMgr is null");
                yield break;
            }
            yield return multABMgr.LoadAB(abName, where);
        }

        /// <summary>
        /// 加载AB包的资源
        /// </summary>
        /// <param name="abName">包名</param>
        /// <param name="assetName">资源名</param>
        /// <param name="isCache">是否缓存</param>
        public Object LoadAsset(string abName, string assetName, bool isCache)
        {
            if (_allAssetsBundlePackages.ContainsKey(abName))
                return _allAssetsBundlePackages[abName].LoadAsset(abName, assetName, isCache);
            Debug.LogError($"LAsset/LoadAsset Error ： don't find the AssetBundle name , so can't load asset ,the scene name is{abName}");
            return null;
        }

        /// <summary>
        /// 根据Type加载AB包的资源
        /// </summary>
        /// <param name="abName">包名</param>
        /// <param name="assetName">资源名</param>
        /// <param name="type">类型</param>
        /// <param name="isCache">是否缓存</param>
        public Object LoadAsset(string abName, string assetName, Type type, bool isCache)
        {
            if (_allAssetsBundlePackages.ContainsKey(abName))
                return _allAssetsBundlePackages[abName].LoadAsset(abName, assetName, type, isCache);
            Debug.LogError($"LAsset/LoadAsset Error ： don't find the AssetBundle name , so can't load asset ,the scene name is{abName}");
            return null;
        }

        /// <summary>
        /// 加载AB包的资源
        /// </summary>
        /// <param name="abName">包名</param>
        /// <param name="assetName">资源名</param>
        /// <param name="isCache">是否缓存</param>
        public T LoadAsset<T>(string abName, string assetName, bool isCache) where T : Object
        {
            if (_allAssetsBundlePackages.ContainsKey(abName))
                return _allAssetsBundlePackages[abName].LoadAsset<T>(abName, assetName, isCache);
            Debug.LogError($"LAsset/LoadAsset Error ： don't find the AssetBundle name , so can't load asset ,the scene name is{abName}");
            return null;
        }

        /// <summary>
        /// 卸载单一资源
        /// <para>可以卸载Component和非GameObject的资源</para>
        /// <para>卸载需要将相关的引用清空</para>
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="asset"></param>
        public void UnloadAsset(string abName, Object asset)
        {
            if (_allAssetsBundlePackages.ContainsKey(abName))
                _allAssetsBundlePackages[abName].UnloadAsset(asset);
            else
                Debug.LogError($"LAsset/LoadAsset Error ： don't find the AssetBundle name , so can't load asset ,the scene name is{abName}");
        }

        /// <summary>
        /// 释放AB包资源
        /// <para>包括以加载的资源</para>
        /// </summary>
        public void DisposeAllAsstes(string abName)
        {
            if (_allAssetsBundlePackages.ContainsKey(abName))
            {
                _allAssetsBundlePackages[abName].DisposeAllAssets();
                _allAssetsBundlePackages.Remove(abName);
            }
            else
                Debug.LogError($"LAssets/DisposeAllAssets Error : don't find the AssetBundle name ,so can't dispose all assets the " +
                    $"asset bundle package's name is {abName}");
        }

        /// <summary>
        /// 释放AB包资源
        /// <para>不会释放已加载的资源文件</para>
        /// </summary>
        /// <param name="abName"></param>
        public void UnloadAssets(string abName)
        {
            if (_allAssetsBundlePackages.ContainsKey(abName))
            {
                _allAssetsBundlePackages[abName].UnloadAssets();
                _allAssetsBundlePackages.Remove(abName);
            }
            else
                Debug.LogError($"LAssets/UnloadAssets Error : don't find the AssetBundle name ,so can't dispose all assets  about this package " +
                    $"the asset bundle package's name is {abName}");
        }

        /// <summary>
        /// 获取AB包中的资源名
        /// </summary>
        /// <param name="abName"></param>
        public string[] GetAllAssetsInAB(string abName)
        {
            if (_allAssetsBundlePackages.ContainsKey(abName))
            {
                return _allAssetsBundlePackages[abName].GetAllAssetsName();
            }
            Debug.LogError($"LAssets/GetAllAssetsInAB Error : Can't find the ab ,the name is {abName}");
            return null;
        }

        public bool ContainsAB(string abName)
        {
            return default;
        }
        #endregion


        #region Resource

        #endregion

        #region Addressables

        #endregion

    }
}
