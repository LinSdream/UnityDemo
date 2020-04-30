using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LS.Common;
using LS.Others;

namespace Souls
{
    public class GlobalManager : MonoSingletionBasisDontClear<GlobalManager>
    {
        #region Mono Callbacks


        private void Start()
        {
            //音频加载
            var list = new List<string>();
            IOHelper.GetFileNameToArray(ref list, "/Resources/Audio/Used");
            foreach (var cell in list)
            {
                AudioManager.Instance.SetAudioPath(cell, "Audio/Used/" + cell);
            }
            AudioManager.Instance.SetSFXVolume(.8f);
            AudioManager.Instance.SetMusicVolume(.8f);
            AudioManager.Instance.PoolLock = true;//保护程序不会崩掉

            CustomSceneManager.Instance.TransitionSceneName = "-1_Transition";

        }

        #endregion
    }

}