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

            if (_instance == null)
                _instance = this as GameManager;

            AudioManager.Instance.LoadGroupAssetsLabel = "AudioGroup";
            AudioManager.Instance.LoadAudioGroupAssets(()=> { Debug.Log("Completed");

                string[] names = AudioManager.Instance.GetAudiosNames(AudioManager.Instance.DefaultGroup);
                foreach (string cell in names)
                    Debug.Log(cell);
                
            });
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}