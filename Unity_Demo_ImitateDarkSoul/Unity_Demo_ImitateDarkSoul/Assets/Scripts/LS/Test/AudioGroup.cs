using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;

namespace LS.Test
{
    [CreateAssetMenu(menuName = "LS/Audio/AudioGroup")]
    public class AudioGroup : ScriptableObject
    {
        #region Public Fields

        public bool Preload = false;
        public string Name = "DefaultGroup";

        [Range(0, 1)] public float Volume = .5f;

        public int Pool = 5;
        public AudioMixerGroup AudioMixer;
        public AssetReference[] AudioAssets;
        public AssetLabelReference[] AudioAssetsLabel;

        [HideInInspector] public float RealityVolume => AudioManager.Instance.SfxVolume * Volume;
        [HideInInspector] public Dictionary<string, AudioInfo> Value = new Dictionary<string, AudioInfo>();
        #endregion

        #region Private Fields
        List<AudioSource> _unusedSoundAudioSourceList = new List<AudioSource>();//可使用音频组件集合
        List<AudioSource> _usedSoundAudioSourceList = new List<AudioSource>();//正在使用的音频组件集合
        #endregion
        private void OnValidate()
        {
            foreach (AssetReference asset in AudioAssets)
            {
                string name = asset.editorAsset.name;
                if (Value.ContainsKey(name))
                    continue;
                var info = new AudioInfo();
                info.Name = name;
                info.Asset = asset;
                info.IsLoad = false;
                info.Clip = null;
                Value.Add(name, info);
            }
        }

        #region Private Methods
        /// <summary> 将未使用的音源集合中提取第一个  要使用的音源文件   到已使用集合中</summary>
        /// <returns> 要使用的音源文件 </returns>
        private AudioSource UnusedToUsed()
        {
            AudioSource audioSource = _unusedSoundAudioSourceList[0];//获得未使用列表的第一个音频文件
            _unusedSoundAudioSourceList.RemoveAt(0);//从未使用列表中移除第一个音频
            _usedSoundAudioSourceList.Add(audioSource);//将第一个音频装移到已使用列表中
            return audioSource;
        }

        /// <summary>将已使用完的音源移动至未使用的集合 </summary>
        /// <param name="audioSource">已使用的音源</param>
        private void UsedToUnused(AudioSource audioSource)
        {
            if (_usedSoundAudioSourceList.Contains(audioSource))//判断是否在已使用列表中存在该音频，如果有则移除
                _usedSoundAudioSourceList.Remove(audioSource);
            if (_unusedSoundAudioSourceList.Count >= Pool)//如果未使用列表中总数小于等于物件池，则移除当前音频
                Destroy(audioSource);
            else if (audioSource != null && !_unusedSoundAudioSourceList.Contains(audioSource))//如果该音频存在且不在未使用音频中，则添加到未使用音频列表中
                _unusedSoundAudioSourceList.Add(audioSource);
        }

        AudioSource GetAudioSource(MonoBehaviour mono, List<AudioSourceInfo> list)
        {
            Debug.Log(_unusedSoundAudioSourceList == null ? "Yes" : "No");
            Debug.Log(_unusedSoundAudioSourceList.Count);
            if (_unusedSoundAudioSourceList.Count != 0)
                return UnusedToUsed();
            Debug.Log("!!!!!!!!!!!!");

            //var audioSource = mono.gameObject.AddComponent<AudioSource>();
            //list.Add(new AudioSourceInfo()
            //{
            //    Name = Name,
            //    SfxAudioSource = audioSource
            //});
            //_unusedSoundAudioSourceList.Add(audioSource);

            var audioSource = mono.gameObject.AddComponent<AudioSource>();
            _unusedSoundAudioSourceList.Add(audioSource);
            return audioSource;

        }

        IEnumerator WaitOfPlaySfxEnd(AudioSource audioSource, float time, System.Action callback, List<AudioSource> list)
        {
            yield return new WaitForSeconds(time);
            UsedToUnused(audioSource);
            if (list.Count == 0)
                Debug.LogError("AudioManager/IEnumerator WaitOfPlaySfxEnd  Error: List of playingSounds is empty");
            else
                list.Remove(audioSource);
            callback?.Invoke();
        }

        #endregion

        public async Task<AudioClip> GetAudioClip(string name)
        {
            if (!Value.ContainsKey(name))
            {
                Debug.LogWarning("AudioGroup " + Name + " GetAudioClip Warning : the audio asset dont exist " +
                    ",the audio name is " + name);
                return null;
            }
            if (Value[name].IsLoad)
                return Value[name].Clip;
            Value[name].Clip = await Value[name].Asset.LoadAssetAsync<AudioClip>().Task;
            Value[name].IsLoad = true;
            return Value[name].Clip;
        }

        public async void LoadAudiosAssets()
        {
            foreach (KeyValuePair<string, AudioInfo> pair in Value)
            {
                if (pair.Value.IsLoad)
                    continue;
                pair.Value.Clip = await pair.Value.Asset.LoadAssetAsync<AudioClip>().Task;
                pair.Value.IsLoad = true;
            }
        }

        public async void Play(AudioSource audioSource, string name, float volume)
        {
            var clip = await GetAudioClip(name);
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.Play();
        }

        public async void PlaySfx(MonoBehaviour mono, string name, Action cb, List<AudioSource> list,
            List<AudioSourceInfo> info, bool openCoroutine = true)
        {
            AudioSource audioSource = GetAudioSource(mono, info);
            Debug.Log(audioSource == null ? "Yes" : "No");
            var clip = await GetAudioClip(name);
            if (clip == null)
                return;
            audioSource.clip = clip;
            audioSource.volume = RealityVolume;
            audioSource.outputAudioMixerGroup = AudioMixer;
            audioSource.Play();
            list.Add(audioSource);
            mono.StartCoroutine(WaitOfPlaySfxEnd(audioSource, audioSource.clip.length, cb, list));//协程，一个音效一个协程
        }

        public string[] GetAudiosName()
        {
            List<string> names = new List<string>();
            foreach (KeyValuePair<string, AudioInfo> pair in Value)
            {
                names.Add(pair.Key);
            }
            return names.ToArray();
        }

        public void ReleaseAssets()
        {
            foreach (KeyValuePair<string, AudioInfo> pair in Value)
            {
                pair.Value.Asset.ReleaseAsset();
                pair.Value.Clip = null;
                pair.Value.IsLoad = false;
            }
        }

        public void ReleaseAsset(string name)
        {
            Value[name].Asset.ReleaseAsset();
            //Value[name].Clip = null;
            Value[name].IsLoad = false;
        }

    }

}