using System;
using System.Collections.Generic;
using UnityEngine;
using LS.AssetsFrameWork;
using LS.Common;
using System.IO;

namespace LS.Test.Others
{

    public class AudioAssetsLoader:IDisposable
    {

        public struct AudioAssetInfo
        {
            public string FileName;
            public string Extension;
            public string AssetPath;
            public AudioClip Clip;

            public AudioAssetInfo(string fileName, string extension, string assetPath)
            {
                FileName = fileName;
                Extension = extension;
                AssetPath = assetPath;
                Clip = null;
            }

        }

        private string _assetBundleName;

        public string AssetBundleName => _assetBundleName;
        /// <summary>
        /// 音频资源格式
        /// </summary>
        private readonly string[] _assetFormat = { ".mp3", ".wav", ".aif", ".ogg" };

        private Dictionary<string, AudioAssetInfo> _assetsMap = new Dictionary<string, AudioAssetInfo>();

        public AudioAssetsLoader(string assetBundleName)
        {
            _assetBundleName = assetBundleName;
        }

        public void LoadAudioAssetsFromAB()
        {
            LAssets.Instance.LoadAB(_assetBundleName);
            CreateAssetsMap();
        }

        /// <summary>
        /// 将包含音频的ab包资源初始化
        /// <para>建立音频名-资源索引</para>
        /// </summary>
        public void CreateAssetsMap()
        {
            var list = LAssets.Instance.GetAllAssetsInAB(_assetBundleName);
            foreach (var cell in list)
            {
                var ex = Path.GetExtension(cell);
                for (int i = 0; i < _assetFormat.Length; i++)
                    if (ex == _assetFormat[i])
                        _assetsMap.Add(Path.GetFileNameWithoutExtension(cell),
                            new AudioAssetInfo(Path.GetFileName(cell), _assetFormat[i], cell));
            }

        }

        /// <summary>
        /// 获取音频
        /// </summary>
        /// <param name="audioName"></param>
        public AudioClip GetAudio(string audioName)
        {
            var tmp = audioName.ToLower();
            if (_assetsMap.ContainsKey(tmp))
            {
                if (_assetsMap[tmp].Clip != null)
                    return _assetsMap[tmp].Clip;
                else
                {
                    var info = _assetsMap[tmp];
                    _assetsMap.Remove(tmp);
                    info.Clip = LAssets.Instance.LoadAsset<AudioClip>(_assetBundleName, info.FileName, false);
                    _assetsMap.Add(tmp, info);
                    return _assetsMap[tmp].Clip;
                }
            }
            return null;
        }

        public void Dispose()
        {
            _assetsMap.Clear();
            _assetsMap = null;
        }
    }
}
