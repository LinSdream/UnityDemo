using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LS.Common;
using LS.Others;
using System.Text;

namespace Souls
{
    public class GlobalManager : MonoSingletionBasisDontClear<GlobalManager>
    {
        #region Mono Callbacks


        private void Start()
        {
            //音频加载
            List<string> list = new List<string>();
            list=IOHelper.GetData<List<string>>(Application.streamingAssetsPath+"/audioName.list");

            foreach (var cell in list)
            {
                AudioManager.Instance.SetAudioPath(cell, "Audio/"+cell);
            }
            AudioManager.Instance.SetSFXVolume(0.8f);
            AudioManager.Instance.SetMusicVolume(0.8f);
            AudioManager.Instance.PoolLock = true;//保护程序不会崩掉

            CustomSceneManager.Instance.TransitionSceneName = "-1_Transition";

        }

        #endregion
    }

}