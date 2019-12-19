using UnityEngine;
using System.Collections;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace LS.Test
{
    public class AudioInfo
    {
        public string Name;
        public AssetReference Asset;
        public AudioClip Clip;
        public bool IsLoad;
    }

    public struct AudioSourceInfo
    {
        public string Name;
        public AudioSource SfxAudioSource;
    }

}