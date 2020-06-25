using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LS.Common;
using LS.AssetsFrameWork;
using LS.Test.Others;

namespace Game
{

    public class GameManager :MonoSingletionBasisDontClear<GameManager>
    {

        /// <summary> 资源是否加载完成 </summary>
        public bool IsLoadCompleted = false;

        private void Start()
        {
            StartCoroutine(LoadAssets());
        }

        IEnumerator LoadAssets()
        {
            //加载环境音
            yield return LAssets.Instance.LoadAB("audio/ambience.u3d", name =>
             {
                 AudioManager.Instance.LoadAudioAssetsFromAB(name);
             }, LoadAssetFromWhere.Local);

            //加载走路音效
            yield return LAssets.Instance.LoadAB("audio/movement.u3d", name =>
             {
                 AudioManager.Instance.LoadAudioAssetsFromAB(name);
             }, LoadAssetFromWhere.Local);

            IsLoadCompleted = true;

            //播放音频，后续把播放音频挪到当前场景，资源加载在另一个场景或者是把关卡隐藏，必须要有一个资源加载的过程
            var gameObject = FindObjectOfType<AudioManager>().gameObject;
            if (gameObject == null)
                yield break;
            var clip=AudioManager.Instance.GetAudioClip("Wind_stones");
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.volume = 0.5f;
            audioSource.loop = true;
            audioSource.Play();

            AudioManager.Instance.PlayMusic("Main Music");
        }

    }

}