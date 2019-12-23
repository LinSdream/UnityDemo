using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LS.Common;
using UnityEngine.ResourceManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;
using System;

namespace LS.Test
{

    public class AudioManager : ASingletonBasis<AudioManager>
    {
        #region Private Fields

        Dictionary<string, AudioGroup> _groups;
        List<AudioSourceInfo> _audioSourceInfoList;
        List<AudioSource> _playingSounds;
        AudioSource _musicAudioSource;

        #endregion

        #region Public Fields

        public string LoadGroupAssetsLabel = "AudioGroupAsset";
        public string DefaultGroup = "DefaultGroup";

        public float MusicVolume = 1f;
        public float SfxVolume = 1f;

        public List<AudioSource> PlayingSounds => _playingSounds;
        public AudioSource BgmAudioSource => _musicAudioSource;
        #endregion

        #region Override Methods
        protected override void Init()
        {
            _groups = new Dictionary<string, AudioGroup>();
            _playingSounds = new List<AudioSource>();
            _musicAudioSource = gameObject.AddComponent<AudioSource>();
        }
        #endregion

        #region Public Methods
        public void LoadAudioGroupAssets(System.Action cb=null)
        {
            Addressables.LoadAssetsAsync<AudioGroup>(LoadGroupAssetsLabel, value =>
            {
                _groups.Add(value.Name, value);
            }).Completed += obj =>
            {
                foreach (KeyValuePair<string, AudioGroup> pair in _groups)
                {
                    if (pair.Value.Preload)
                    {
                        pair.Value.LoadAudiosAssets();
                    }
                }
                cb?.Invoke();
            };

        }

        public void LoadAudioGroupAssets(System.Action cb,params AssetReference[] groupAssets)
        {
            Addressables.LoadAssetsAsync<AudioGroup>(new List<object>(groupAssets), value =>
             {
                 _groups.Add(value.Name, value);
             }).Completed += obj =>
             {
                 foreach (KeyValuePair<string, AudioGroup> pair in _groups)
                 {
                     if (pair.Value.Preload)
                     {
                         pair.Value.LoadAudiosAssets();
                     }
                 }
                 cb?.Invoke();
             };
        }

        public void PlayBgm(string groupName,string name)
        {
            if(_musicAudioSource==null)
            {
                Debug.LogError("AudioManager/PlayBgm Error : can't get the audio source ");
                return;
            }
            _groups[groupName].Play(_musicAudioSource, name, MusicVolume);
        }

        public void PauseBgm()
        {
            _musicAudioSource.Pause();
        }

        public void UnPauseBgm()
        {
            _musicAudioSource.UnPause();
        }

        public void StopBgm()
        {
            _musicAudioSource.Stop();
        }

        public void PlaySFX(string groupName, string name, System.Action cb = null)
        {

            if (!_groups.ContainsKey(groupName))
            {
                Debug.LogError("AudioManager/PlaySFX Error : can't get the audio group ,the group name is " + groupName
                    + " maybe the group is loading or the group don't exist");
                return;
            }

            var group = _groups[groupName];
            
            group.PlaySfx(this, name, cb,_playingSounds, _audioSourceInfoList);
        }

        public void PlaySFX(string name, Action cb = null)
        {
            if (!_groups.ContainsKey(DefaultGroup))
            {
                Debug.LogError("AudioManager/PlaySFX Error : can't get the audio group ,the group name is " + DefaultGroup
                    +" maybe the group is loading or the group don't exist");
                return;
            }

            var group = _groups[DefaultGroup];

            group.PlaySfx(this,name,cb,_playingSounds, _audioSourceInfoList);

        }

        public void PauseSfx()
        {
            if (_playingSounds.Count == 0)
            {
                Debug.LogWarning("AudioManager/PauseSFX Warning :the list of PlayingSounds is empty");
                return;
            }
            foreach (AudioSource audioSource in _playingSounds)//在播放音效列表中逐个暂停音效
            {
                audioSource.Pause();
            }
        }

        /// <summary>继续播放音效</summary>
        public void UnPauseSfx()
        {
            if (_playingSounds.Count == 0)
            {
                Debug.LogWarning("AudioManager/ContinueSFX Warning : the list of  PlayingSounds is empty");
                return;
            }
            foreach (AudioSource audioSource in _playingSounds)//在播放音效列表中逐个继续音效
            {
                audioSource.UnPause();
            }
        }

        public void StopSfx()
        {
            if (_playingSounds.Count == 0)
            {
                Debug.LogWarning("AudioManager/PauseSFX Warning :the list of PlayingSounds is empty");
                return;
            }
            foreach (AudioSource audioSource in _playingSounds)//在播放音效列表中逐个暂停音效
            {
                audioSource.Stop();
            }
            StopAllCoroutines();
            _playingSounds.Clear();
        }

        public string[] GetAudiosNames(string groupName)
        {
            return _groups[groupName].GetAudiosName();
        }

        public AudioInfo GetAudioInfo(string name)
        {
            return _groups[DefaultGroup].Value[name];
        }

        public void ReleaseAssets(string name)
        {
            _groups[name].ReleaseAssets();
        }

        public void ReleaseAsset(string groupName,string name)
        {
            _groups[groupName].ReleaseAsset(name);
        }
        #endregion

    }
}