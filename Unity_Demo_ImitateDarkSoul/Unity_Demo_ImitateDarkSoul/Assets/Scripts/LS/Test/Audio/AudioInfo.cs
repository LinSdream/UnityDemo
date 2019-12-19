using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LS.Test.Audio
{
    public struct AudioInfo 
    {
        public string Name;
        public AssetReference Asset;
        public AudioClip Clip;
        public bool IsLoad;
    }
}