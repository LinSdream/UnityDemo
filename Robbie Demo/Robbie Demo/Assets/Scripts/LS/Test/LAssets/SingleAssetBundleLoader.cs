using LS.Common;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace LS.AssetsFrameWork
{
    /// <summary>
    /// AB包类
    /// <para>单个的AB包的加载卸载与包中的资源加载</para>
    /// </summary>
    public class SingleAssetBundleLoader : IDisposable
    {
        /// <summary>
        /// 当前AB的资源类
        /// </summary>
        AssetsLoader _abAssets;

        DelLoadCompleteHandle _loadComplete;

        /// <summary>
        /// 包名
        /// </summary>
        string _abName;
        /// <summary>
        /// ab包下载的路径
        /// </summary>
        string _abDownLoadPath;

        public SingleAssetBundleLoader(string abName, DelLoadCompleteHandle loadComplete = null, LoadAssetFromWhere where = LoadAssetFromWhere.OnLine)
        {
            _loadComplete = loadComplete;
            _abName = abName;
            if (where == LoadAssetFromWhere.OnLine)
                _abDownLoadPath = PathTools.GetWWWPath() + "/" + _abName;
            else
                _abDownLoadPath = PathTools.LocalAB_OutPath + "/" + _abName;
        }

        /// <summary>
        /// 加载AB包
        /// </summary>
        /// <returns></returns>
        public IEnumerator LoadAB()
        {
            //https://developer.51cto.com/art/200908/147158.htm
            //using：用于处理 非托管资源，不受GC的控制的资源，在using结束后会隐性调用Dispose方法，因此需要实现Dispose方法
            //UnityWebRequest :https://docs.unity3d.com/ScriptReference/Networking.UnityWebRequest.html
            //Unity已经将 'WWW' 给弃用，现在应该逐步过渡到使用UnityWebRequest
            using (UnityWebRequest req = UnityWebRequestAssetBundle.GetAssetBundle(_abDownLoadPath))
            {
                yield return req.SendWebRequest();

                if (req.downloadProgress >= 1)
                {
                    //返回在www上下载的资源包，如果有，否则null
                    AssetBundle ab = DownloadHandlerAssetBundle.GetContent(req);
                    if (ab != null)
                    {
                        _abAssets = new AssetsLoader(ab);
                        _loadComplete?.Invoke(_abName);
                    }
                }
                else
                    Debug.LogError($"SingleAssetLoader/LoadAB Error : the {_abDownLoadPath} is null !");

            }
        }

        /// <summary>
        /// 加载AB包指定资源
        /// </summary>
        public Object LoadAsset(string assetName, bool isCache)
        {
            if (_abAssets != null)
                return _abAssets.LoadAsset(assetName, isCache);
            Debug.LogError("SingleAssetLoader/LoadAsset Erro : the _loader is null ");
            return null;
        }

        /// <summary>
        /// 加载AB包指定资源
        /// </summary>
        public Object LoadAsset(string assetName, Type type, bool isCache)
        {
            if (_abAssets != null)
                return _abAssets.LoadAsset(assetName, type, isCache);
            Debug.LogError("SingleAssetLoader/LoadAsset Erro : the _loader is null ");
            return null;
        }

        /// <summary>
        /// 加载AB包指定资源
        /// </summary>
        public T LoadAsset<T>(string assetName, bool isCache) where T : Object
        {
            if (_abAssets != null)
                return _abAssets.LoadAsset<T>(assetName, isCache);
            Debug.LogError("SingleAssetLoader/LoadAsset Erro : the _loader is null ");
            return null;
        }

        /// <summary>
        /// 卸载AB包的指定资源
        /// </summary>
        public bool UnLoadAsset(Object asset)
        {
            if (_abAssets != null)
                return _abAssets.UnLoadAsset(asset);
            else
            {
                Debug.LogError("SingleAssetLoader/UnLoader Error ： can't unload the asset the asset is null");
                return false;
            }
        }

        public void Dispose()
        {
            if (_abAssets != null)
                _abAssets.Dispose();
            else
                Debug.LogError("SingleAssetLoader/Dispose Error : can't dispose the asset,_loder is null");
        }

        public void DisposeAll()
        {
            if (_abAssets != null)
                _abAssets.DisposeAll();
            else
                Debug.LogError("SingleAssetLoader/DisposeAll Error : can't dispose the asset,_loder is null");

        }

        /// <summary>
        /// 查询AB包中所有资源
        /// </summary>
        public string[] GetAllAssetName()
        {
            if (_abAssets != null)
                return _abAssets.GetAllAssetName();
            else
            {
                Debug.LogError("SingleAssetLoader/GetAllAssetName Error : _loder is null");
                return null;
            }
        }
    }
}
