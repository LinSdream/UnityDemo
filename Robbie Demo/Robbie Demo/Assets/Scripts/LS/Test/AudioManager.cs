using LS.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS.Test.Others
{
    /**
     *  LaterTime ：2020.6.13
     *  
     *  Use:
     *      1.调用LoadAudioAssetsFromAB，建立音频索引表(audio name - audio asset)
     *      2.调用相应的方法
     *      3.卸载资源需要调用Clear方法后才能用LAssets去卸载音频资源
     *      
     *  Decription：
     *      通过AB包进行的音频加载，满足一些简单的音频需求。
     *      本身不对AB包进行资源的加载卸载，PS：包加载可以实现
     *      核心思路就在于去创建一系列的索引表 单个AB包中的 name-asset info 索引，外部的所有的包管理的List -- 转化为 --> (name- audioClip)的索引
     *      
     *  BugsOrQuestions:
     *      1.暂时不支持指定包的资源卸载（核心问题在于相对应的AudioClip进入到了音频池后，不好找，找到的成本有点大）
     *      2.由于是通过获取包中的所有相关的音频资源路径，这里存在两个问题，一是字符串分割，二是资源是小写。因此这里有性能消耗
     *      3. 不能同名
     * 
     **/

    /// <summary>
    /// 音频管理
    /// </summary>
    public class AudioManager : MonoSingletonBasis<AudioManager>
    {
        #region Private Fields
        /// <summary>音频集合 </summary>
        Dictionary<string, AudioClip> _audios = new Dictionary<string, AudioClip>();
        /// <summary> 背景音</summary>
        AudioSource _musicAudioSource;
        /// <summary> 物件池大小 </summary>
        int _poolCount = 10;
        /// <summary>对音频池进行加锁 <para>如果不加锁，当音频池中不存在空闲AudioSource时，会自动添加</para>
        /// <para>加锁，超出音频池后，不会播放音频</para>
        /// <para>默认加锁</para>
        /// </summary>
        bool PoolLock = true;
        /// <summary> 已使用AudioSource</summary>
        List<AudioSource> _usedSoundAudioSourceList = new List<AudioSource>();
        /// <summary> 未使用AudioSource</summary>
        List<AudioSource> _unusedSoundAudioSourceList = new List<AudioSource>();
        /// <summary> 资源集</summary>
        List<AudioAssetsLoader> _assets = new List<AudioAssetsLoader>();
        /// <summary>正在播放的音频</summary>
        List<AudioSource> _playingSounds = new List<AudioSource>();
        /// <summary> 背景音音量 </summary>
        float _musicVolume;
        /// <summary> 音效音量</summary>
        float _soundVolume;
        #endregion

        #region Public Fields

        /// <summary> 背景音音量 </summary>
        public float MusicVolume => _musicVolume;
        /// <summary> 音效音量</summary>
        public float SoundVolume => _soundVolume;

        /// <summary>
        /// 背景音的AudioSOurce
        /// </summary>
        public AudioSource MusicAudioSource => _musicAudioSource;

        /// <summary>
        /// 音频池大小
        /// </summary>
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
        /// <summary>
        /// 正在播放的音频
        /// </summary>
        public List<AudioSource> PlayingSounds => _playingSounds;
        #endregion

        #region Mono Callbacks
        protected override void Init()
        {
            _musicAudioSource = gameObject.AddComponent<AudioSource>();

            _musicVolume = 0.5f;
            _soundVolume = 0.5f;
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

        /// <summary>
        /// 从AB包中加载资源
        /// <para>默认是已经通过LAssets加载过音频资源所在的包</para>
        /// </summary>
        /// <param name="abName">包名</param>
        /// <param name="isLoadAB">是否已经加载</param>
        public void LoadAudioAssetsFromAB(string abName, bool isLoadAB = true)
        {
            if (_assets.Find(value => value.AssetBundleName == abName) != null)
                return;

            var tmp = new AudioAssetsLoader(abName);
            if (isLoadAB)
                tmp.CreateAssetsMap();
            else
                tmp.LoadAudioAssetsFromAB();
            _assets.Add(tmp);
        }

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
        public Coroutine PlaySFX(string name, Action callback = null)
        {
            AudioSource audioSource;
            if (_unusedSoundAudioSourceList.Count == 0)
            {
                if (_usedSoundAudioSourceList.Count >= PoolCount)//如果已使用的数量超出物件池数
                    if (PoolLock)//并且还被加锁情况下
                        return null;//直接返回，不在新增AudioSource,同时该音频不在播放
                audioSource = AddAudioSource();
            }
            else
                audioSource = UnusedToUsed();
            audioSource.clip = GetAudioClip(name);
            audioSource.volume = _soundVolume;
            _playingSounds.Add(audioSource);
            audioSource.Play();
            return StartCoroutine(WaitOfPlaySfxEnd(audioSource, audioSource.clip.length, callback));//协程，一个音效一个协程

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
            foreach (AudioSource audioSource in _playingSounds)
                audioSource.volume = volume;
        }

        /// <summary>
        /// 获取音频资源
        /// </summary>
        /// <param name="audioName"></param>
        public AudioClip GetAudioClip(string audioName)
        {
            if (_audios.ContainsKey(audioName))
                return _audios[audioName];

            foreach (AudioAssetsLoader asset in _assets)
            {
                var tmp = asset.GetAudio(audioName);
                if (tmp != null)
                {
                    _audios.Add(audioName, tmp);
                    return _audios[audioName];
                }
            }

            Debug.LogError($"AudioManager/GetAudioClip Error : Can't find the audio,name is {audioName}");
            return null;
        }

        /// <summary>
        /// 停止所有音频
        /// </summary>
        public void StopAudio()
        {
            StopSFX();
            StopMusic();
        }

        /// <summary>
        /// 清理所有的音频
        /// </summary>
        public void Clear()
        {
            StopAudio();

            //清除所有引用
            _musicAudioSource.clip = null;
            foreach (var cell in _unusedSoundAudioSourceList)
                cell.clip = null;
            foreach (var cell in _usedSoundAudioSourceList)
                cell.clip = null;

            _audios.Clear();
            _assets.Clear();
        }

        #endregion

    }
}
