using UnityEngine;
using LS.Common;
using System.Collections;
using UnityEngine.Networking;

namespace LS.AssetsFrameWork
{
    public class ABManifestLoader : SingletonBasisNoMono<ABManifestLoader>, System.IDisposable
    {

        /// <summary> 系统ab清单文件 </summary>
        AssetBundleManifest _manifest;

        /// <summary> ab清单文件路径 </summary>
        string _manifestPath;

        /// <summary> 读取AB清单文件的AB</summary>
        AssetBundle _abReadManifest;

        /// <summary> 是否加载完成 </summary>
        public bool IsLoadFinish { get; private set; } = false;

        public bool ManifestIsNull => _manifest == null;

        public ABManifestLoader()
        {
            _manifestPath = PathTools.GetWWWPath() + "/" + PathTools.GetPlatfromName();
        }

        /// <summary>
        /// 加载Mainfest清单文件
        /// </summary>
        public IEnumerator LoadManifestFile(LoadAssetFromWhere where = LoadAssetFromWhere.OnLine)
        {
            using (UnityWebRequest req = UnityWebRequestAssetBundle.GetAssetBundle(where == LoadAssetFromWhere.OnLine ? _manifestPath : PathTools.LocalAB_OutPath + "/AssetBundle"))
            {
                var operation = req.SendWebRequest();

                //yield return req.SendWebRequest();

                while (!operation.isDone)
                {
                    yield return null;
                }

                if (req.downloadProgress >= 1)
                {
                    AssetBundle ab = DownloadHandlerAssetBundle.GetContent(req);
                    if (ab != null)
                    {
                        _abReadManifest = ab;
                        _manifest = ab.LoadAsset(ABDefine.AB_MANIFEST) as AssetBundleManifest;
                        IsLoadFinish = true;
                    }
                    else
                    {
                        Debug.LogError($"ABManifestLoader/LoadManifestFile Error : can't down the asset ,the manifset path is {_manifestPath}");
                    }
                }
            }
        }

        // <summary>
        /// 获取ABManifest
        /// </summary>
        public AssetBundleManifest GetABManifest()
        {
            if (IsLoadFinish)
            {
                if (_manifest != null)
                    return _manifest;
                else
                {
                    Debug.LogError("ABManifestLoader/GetManifest Error : the manifest is null ");
                }
            }
            else
                Debug.LogError("ABManifestLoader/GetManifest Error : the manifest doesn't finish loading ");
            return null;
        }

        /// <summary>
        /// 获取ABManifest系统类的依赖项
        /// </summary>  
        public string[] GetDependce(string abName)
        {
            if (!string.IsNullOrEmpty(abName))
                return _manifest.GetAllDependencies(abName);
            Debug.LogWarning("ABManifestLoader/GetDependce Warning : assetbundle name is null or empty");
            return null;
        }

        /// <summary>
        /// 释放本类资源
        /// </summary>
        public void Dispose()
        {
            _abReadManifest?.Unload(true);
        }
    }
}
