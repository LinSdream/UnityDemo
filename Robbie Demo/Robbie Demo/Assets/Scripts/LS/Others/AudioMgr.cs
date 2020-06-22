using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using LS.Common;
//using DG.Tweening;

//参考：https://gameinstitute.qq.com/community/detail/117779

namespace LS.Old
{

    /// <summary>
    ///  音频管理器
    ///  播放音频前，调用SetAudioPath，加载音频文件
    ///  场景切换后，单例类不被销毁
    /// </summary>
    [Obsolete]
    public class AudioMgr : MonoSingletionBasisDontClear<AudioMgr>
    {
        #region Private Methods
        private AudioSource _musicAudioSource;//背景音源
        private float _musicVolume = 0.05f;//声音大小
        private float _soundVolume = 0.05f;//音效声音大小
        private string _musicVolumePrefs = "MusicVolume";//持久化MusicVolume key
        private string _soundVolumePrefs = "SoundVolume";//持久化SoundVolume key
        private int _poolCount = 10;//池大小
        private Dictionary<string, string> _audioPath;//音频路径
        private List<AudioSource> _unusedSoundAudioSourceList;//可使用音频组件集合
        private List<AudioSource> _usedSoundAudioSourceList;//正在使用的音频组件集合
        private Dictionary<string, AudioClip> _audioClip;//音频剪辑集合
        private List<AudioSource> _playingSounds;//音效暂停集合
        #endregion

        #region Public Methods
        /// <summary>  背景音BGM的AudioSource </summary>
        public AudioSource MusicAudioSource => _musicAudioSource;
        /// <summary>  正在播放的音效列表SFX列表 </summary>
        public List<AudioSource> PlayingSounds => _playingSounds;
        /// <summary> 物件池锁，加锁情况下如果超出物件池的数目的时候，不会再新增音频 </summary>
        public bool PoolLock = false;

        public int PoolCount
        {
            get
            {
                return _poolCount;
            }
            set
            {
                if (value > 0)
                    _poolCount = value;
                else
                    _poolCount = 10;
            }
        }
        #endregion

        #region MonoBehaivour Callbacks
        /// <summary>从本地获取音量信息</summary>
        void Start()
        {
            if (PlayerPrefs.HasKey(_musicVolumePrefs))
                _musicVolume = PlayerPrefs.GetFloat(_musicVolumePrefs);
            if (PlayerPrefs.HasKey(_soundVolumePrefs))
                _soundVolume = PlayerPrefs.GetFloat(_soundVolumePrefs);
        }
        #endregion

        #region Override Methods

        /// <summary>场景不销毁，变量初始化</summary>
        protected override void Init()
        {
            _audioPath = new Dictionary<string, string>();
            _musicAudioSource = gameObject.AddComponent<AudioSource>();
            _usedSoundAudioSourceList = new List<AudioSource>();
            _unusedSoundAudioSourceList = new List<AudioSource>();
            _audioClip = new Dictionary<string, AudioClip>();
            _playingSounds = new List<AudioSource>();
        }

        public  void OnDestroy()
        {
            UnloadAssets();
        }
        #endregion

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
            //判断是否在已使用列表中存在该音频，如果有则移除
            if (_usedSoundAudioSourceList.Contains(audioSource))
                _usedSoundAudioSourceList.Remove(audioSource);
            //如果未使用列表中总数小于等于物件池，则移除当前音频
            if (_unusedSoundAudioSourceList.Count >= _poolCount)
                Destroy(audioSource);
            //如果该音频存在且不在未使用音频中，则添加到未使用音频列表中
            else if (audioSource != null && !_unusedSoundAudioSourceList.Contains(audioSource))
                _unusedSoundAudioSourceList.Add(audioSource);
        }

        /// <summary>获取音频剪辑同时将音频载入内存 </summary>
        /// <param name="name">音源字典中的音频剪辑name</param>
        /// <returns>音频剪辑</returns>
        private AudioClip GetAudioClip(string name)
        {
            if (!_audioClip.ContainsKey(name))//确认是否有包含指定的键
            {
                if (!_audioPath.ContainsKey(name))
                {
                    Debug.LogError("AudioManager/GetAudioClip Error :can't find the audioClip ,the name is " + name);
                    return null;
                }
                AudioClip ac = Resources.Load(_audioPath[name]) as AudioClip;//开始从audioPath中中加载音频文件到audioClip集合中
                _audioClip.Add(name, ac);
            }
            return _audioClip[name];
        }

        /// <summary>获取音源文件</summary>
        /// <returns>要使用的音源文件</returns>
        private AudioSource AddAudioSource()
        {
            if (_unusedSoundAudioSourceList.Count != 0)
                return UnusedToUsed();
            else
            {
                AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                _unusedSoundAudioSourceList.Add(audioSource);//添加到未使用音频列表中
                return audioSource;
            }
        }

        /// <summary>音频播放完后，移动至未使用集合中</summary>
        IEnumerator WaitOfPlaySfxEnd(AudioSource audioSource, float time, Action callback)
        {
            yield return new WaitForSeconds(time);
            UsedToUnused(audioSource);
            if (_playingSounds.Count == 0)
                Debug.LogError("AudioManager/IEnumerator WaitOfPlaySfxEnd  Error: List of playingSounds is empty");
            else
                _playingSounds.Remove(audioSource);
            callback?.Invoke();
        }
        #endregion

        #region Public Methods
        /// <summary>播放背景音乐BGM</summary>
        /// <param name="name">要播放的音频id</param>
        /// <param name="loop">是否循环播放</param>
        /// <param name="lighten">是否淡入淡出</param>
        public void PlayMusic(string name, bool loop = true, bool lighten = false)
        {
            if (_musicAudioSource == null)
            {
                Debug.LogError("AudioManager/PlayMusic Error : the musice AudioSource is null");
                return;
            }
            _musicAudioSource.clip = GetAudioClip(name);//将要播放的音频存储到AudioSource中
            _musicAudioSource.loop = loop;//音频是否循环
                                          //if (lighten)
                                          //{
                                          //    DOTween.To(() => _musicAudioSource.volume, value => _musicAudioSource.volume = value, 0, 0.5f).OnComplete(() =>
                                          //    {
                                          //        _musicAudioSource.volume = _musicVolume;//音频音量
                                          //        _musicAudioSource.Play();//播放音频
                                          //        DOTween.To(() => _musicAudioSource.volume, value => _musicAudioSource.volume = value, _musicVolume, 0.5f);
                                          //    });
                                          //}
                                          //else
                                          //{
            _musicAudioSource.volume = _musicVolume;
            _musicAudioSource.Play();
            //}

        }


        /// <summary>暂停背景音</summary>
        public bool PauseMusic()
        {
            if (_musicAudioSource == null)
            {
                Debug.LogError("AudioManager/PauseMusic Error: the music AudioSource is null.");
                return false;
            }
            if (!_musicAudioSource.isPlaying)
                return false;
            _musicAudioSource.Pause();
            return true;
        }

        /// <summary>继续播放背景音</summary>
        public void ContinueMusic()
        {
            if (_musicAudioSource == null)
            {
                Debug.LogError("AudioManager/ContinueMusic Error : the music AudioSource is null.");
                return;
            }

            _musicAudioSource.UnPause();
        }

        /// <summary> 停止播放背景音</summary>
        public bool StopMusic()
        {
            if (_musicAudioSource == null)
            {
                Debug.LogError("AudioManager/StopMusic Error: the music AudioSource is null.");
                return false;
            }
            if (!_musicAudioSource.isPlaying)
                return false;
            _musicAudioSource.Stop();
            return true;
        }

        /// <summary>
        /// 背景音协程
        /// </summary>
        public IEnumerator WaitForMusicPlayEnd(Action callback = null)
        {
            yield return new WaitForSeconds(_musicAudioSource.clip.length);
            callback?.Invoke();
        }

        /// <summary> 播放音效 </summary>
        public void PlaySFX(string name, Action callback = null)
        {
            AudioSource audioSource;
            if (_unusedSoundAudioSourceList.Count == 0)
            {
                if (_usedSoundAudioSourceList.Count >= PoolCount)//如果已使用的数量超出物件池数
                    if (PoolLock)//并且还被加锁情况下
                        return;//直接返回，不在新增AudioSource,同时该音频不在播放
                audioSource = AddAudioSource();
            }
            else
                audioSource = UnusedToUsed();
            audioSource.clip = GetAudioClip(name);
            audioSource.volume = _soundVolume;
            _playingSounds.Add(audioSource);
            audioSource.Play();
            StartCoroutine(WaitOfPlaySfxEnd(audioSource, audioSource.clip.length, callback));//协程，一个音效一个协程

        }

        /// <summary>暂停音效</summary>
        public void PauseSFX()
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
        public void ContinueSFX()
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

        /// <summary>停止音效</summary>
        public void StopSFX()
        {
            if (_playingSounds.Count == 0)
            {
                Debug.LogWarning("AudioManager/StopSFX Warning :the list of PlayingSounds is empty");
                return;
            }
            foreach (AudioSource audioSource in _playingSounds)
            {
                audioSource.Stop();//将音效结束
                UsedToUnused(audioSource);//结束后的音效，移动至  未使用音频列表

            }
            StopAllCoroutines();
            _playingSounds.Clear();//同时清空播放音效列表
            return;
        }

        /// <summary>播放音效为3d，</summary>
        /// <param name="name">音频名称</param>
        /// <param name="position">方向</param>
        public void Play3dSFX(string name, Vector3 position)
        {
            AudioClip ac = GetAudioClip(name);
            AudioSource.PlayClipAtPoint(ac, position);
        }

        /// <summary>设置BGM的音量大小 </summary>
        /// <param name="volume">音量</param>
        public void SetMusicVolume(float volume)
        {
            _musicVolume = volume;
            _musicAudioSource.volume = volume;
            PlayerPrefs.SetFloat(_musicVolumePrefs, volume);
        }

        /// <summary>设置音效声音大小 </summary>
        /// <param name="volume">音效大小</param>
        public void SetSFXVolume(float volume)
        {
            _soundVolume = volume;
            for (int i = 0; i < _unusedSoundAudioSourceList.Count; i++)
                _unusedSoundAudioSourceList[i].volume = volume;
            for (int i = 0; i < _usedSoundAudioSourceList.Count; i++)
                _usedSoundAudioSourceList[i].volume = volume;
            PlayerPrefs.SetFloat(_soundVolumePrefs, volume);
            foreach (AudioSource audioSource in _playingSounds)
                audioSource.volume = volume;
        }

        /// <summary> 添加音源</summary>
        /// <param name="name">音频名称</param>
        /// <param name="path">路径</param>
        public void SetAudioPath(string name, string path)
        {
            if (_audioPath.ContainsKey(name))
            {
                Debug.LogWarning("AudioManager/LoadAudioPath Warning: name already exist, name is " + name);
                return;
            }
            _audioPath.Add(name, path);
        }

        /// <summary> 添加音源</summary>
        /// <param name="path"> 音频name-路径 字典 </param>
        public bool SetAudioPath(Dictionary<string, string> path)
        {
            foreach (KeyValuePair<string, string> pairs in path)
            {
                string key = pairs.Key;
                string value = pairs.Value;
                if (_audioPath.ContainsKey(key))
                {
                    Debug.LogWarning("AudioManager/LoadAudioPath Warning : name already exist ,name is " + key);
                    return false;
                }
                _audioPath.Add(key, value);
            }
            return true;
        }

        /// <summary>释放内存资源</summary>
        public void UnloadAssets()
        {
            if (_audioClip.Count == 0)
            {
                return;
            }

            if (_playingSounds.Count != 0)//如果还有正在播放的列表，则停止所有音效，然后在释放内存
            {
                Debug.LogWarning("AudioManager/UnloadAssets Warning : something audios is useing !");
                StopSFX();
            }

            foreach (KeyValuePair<string, AudioClip> pair in _audioClip)
                Resources.UnloadAsset(_audioClip[pair.Key]);//释放内存
            _audioClip.Clear();//音频缓存列表清空

        }

        /// <summary>
        /// 停止所有音频
        /// </summary>
        public void StopAudio()
        {
            StopSFX();
            StopMusic();
        }


        //public void Clear()
        //{
        //    StopAudio();
        //    foreach(AudioSource cell in _unusedSoundAudioSourceList)
        //    {
        //        if(cell)
        //        {
        //            Destroy(cell);
        //        }
        //    }
        //    _unusedSoundAudioSourceList.Clear();
        //    UnloadAssets();
        //}
        #endregion
    }

}