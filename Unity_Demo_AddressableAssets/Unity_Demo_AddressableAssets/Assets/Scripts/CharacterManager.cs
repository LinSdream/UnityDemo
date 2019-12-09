using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets; //TODO: Mention the use of this namespace
using UnityEngine.ResourceManagement.AsyncOperations; // TODO: Mention that this is needed to do the async operations over the lists?

public class CharacterManager : MonoBehaviour
{
    #region Fields
    public List<AssetReference> Characters;//AddressAssets对象列表

    bool _isAssetsReady = false;//异步加载，资源是否加载完成
    int _charactorsAssetsCount;//加载资源的数量，用于辅助判断IsAssetsReady是否完成
    #endregion

    #region MonoBehaviour Callbacks
    private void Start()
    {
        _charactorsAssetsCount = Characters.Count;
        
        //LoadAssets
        foreach(AssetReference cell in Characters)
        {
            //如果当前加载资源完成，进行回调
            cell.LoadAssetAsync<GameObject>().Completed += OnCharacterAssetLoaded;
        }

    }

    private void OnDestroy()
    {
        foreach(AssetReference cell in Characters)
        {
            cell.ReleaseAsset();
        }
    }

    #endregion

    #region Callbacks Events
    /// <summary>
    /// 资源加载完成后的回调
    /// </summary>
    void OnCharacterAssetLoaded(AsyncOperationHandle<GameObject> obj)
    {
        //所有待加载资源数--
        _charactorsAssetsCount--;
        //如果所有资源加载完成
        if (_charactorsAssetsCount <= 0)
            _isAssetsReady = true;//解锁
    }

    /// <summary>
    /// 资源实例化后的回调
    /// </summary>
    private void InstantiateCompleted(AsyncOperationHandle<GameObject> obj)
    {
        GameObject gameObj = obj.Result;
        gameObj.name = "Character";
        Debug.Log(gameObj.name);
    }
    #endregion

    #region Buttons
    //Instantiate assets
    public void SpawnCharacter(int characterType)
    {
        //只有所有资源加载完成后，才能进行实例化
        if(_isAssetsReady)
        {
            Vector3 pos = Random.insideUnitSphere * 5;
            pos.Set(pos.x, 0, pos.z);
            var obj = Characters[characterType].InstantiateAsync(pos, Quaternion.identity);//异步实例化
            obj.Completed += InstantiateCompleted;
        }
    }

    #endregion


    #region Completed

    //public GameObject m_archerObject;

    ////public AssetReference m_ArcherObject;

    ////public List<AssetReference> m_Characters;
    ////bool m_AssetsReady = false;
    ////int m_ToLoadCount;
    ////int m_CharacterIndex = 0;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    //m_ToLoadCount = m_Characters.Count;

    //    //foreach (var character in m_Characters)
    //    //{
    //    //    character.LoadAssetAsync<GameObject>().Completed += OnCharacterAssetLoaded;
    //    //}
    //}


    //public void SpawnCharacter(int characterType)
    //{
    //    Instantiate(m_archerObject);

    //    //m_ArcherObject.InstantiateAsync();

    //    //if (m_AssetsReady)
    //    //{
    //    //    Vector3 position = Random.insideUnitSphere * 5;
    //    //    position.Set(position.x, 0, position.z);
    //    //    m_Characters[characterType].InstantiateAsync(position, Quaternion.identity);
    //    //}
    //}

    ////void OnCharacterAssetLoaded(AsyncOperationHandle<GameObject> obj)
    ////{
    ////    m_ToLoadCount--;

    ////    if (m_ToLoadCount <= 0)
    ////        m_AssetsReady = true;
    ////}

    ////private void OnDestroy() //TODO: Should we teach instantiate with game objects and then manually release?
    ////{
    ////    foreach (var character in m_Characters)
    ////    {
    ////        character.ReleaseAsset();
    ////    }
    ////}
    #endregion
}
