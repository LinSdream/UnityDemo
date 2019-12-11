using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class SceneManager : MonoBehaviour
{
    public AssetReference NextLoadScene;

    public void Btn_LoadSceneAsync()
    {
        Addressables.LoadSceneAsync(NextLoadScene, UnityEngine.SceneManagement.LoadSceneMode.Single).Completed += LoadScene_Completed_Event;
    }

    private void LoadScene_Completed_Event(AsyncOperationHandle<SceneInstance> obj)
    {
        Debug.Log("End"); 
    }

    #region Completed
    //public string m_SceneAddressToLoad;

    //public void LoadGameplayScene()
    //{
    //    Addressables.LoadSceneAsync(m_SceneAddressToLoad, UnityEngine.SceneManagement.LoadSceneMode.Single).Completed += OnSceneLoaded;
    //}

    //void OnSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
    //{
    //    //Addressables.UnloadSceneAsync(new SceneInstance());
    //    // LOGIC THAT KICKSTARTS THE GAMEPLAY
    //}
    #endregion
}
