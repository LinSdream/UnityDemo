using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.AddressableAssets;

namespace LS.Test.Audio
{
    [CreateAssetMenu(menuName ="LS/Test/Audio/AudioGroup")]
    public class AudioGroup : ScriptableObject
    {
        #region Public Fields
        public bool Preload;
        public string GroupName;
        public int Pool;

        [Range(0f, 1f)] public float Volume;

        public AudioMixerGroup MixerGroup;
        public AssetLabelReference[] AssetsLabels;
        #endregion

    }

}