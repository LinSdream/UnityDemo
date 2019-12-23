using LS.Test;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        #region Fields

        private static GameManager _instance;

        public static GameManager Instance => _instance;

        #endregion

        private void Awake()
        {
            AudioManager.Instance.LoadGroupAssetsLabel = "AudioGroup";
            AudioManager.Instance.LoadAudioGroupAssets(()=> { Debug.Log("Completed"); });
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}