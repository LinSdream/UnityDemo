﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets; //TODO: Mention the use of this namespace
using UnityEngine.ResourceManagement.AsyncOperations; // TODO: Mention that this is needed to do the async operations over the lists?

public class CharacterManager : MonoBehaviour
{
    public GameObject m_archerObject;

    //public AssetReference m_ArcherObject;

    //public List<AssetReference> m_Characters;
    //bool m_AssetsReady = false;
    //int m_ToLoadCount;
    //int m_CharacterIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        //m_ToLoadCount = m_Characters.Count;

        //foreach (var character in m_Characters)
        //{
        //    character.LoadAssetAsync<GameObject>().Completed += OnCharacterAssetLoaded;
        //}
    }


    public void SpawnCharacter(int characterType)
    {
        Instantiate(m_archerObject);

        //m_ArcherObject.InstantiateAsync();

        //if (m_AssetsReady)
        //{
        //    Vector3 position = Random.insideUnitSphere * 5;
        //    position.Set(position.x, 0, position.z);
        //    m_Characters[characterType].InstantiateAsync(position, Quaternion.identity);
        //}
    }

    //void OnCharacterAssetLoaded(AsyncOperationHandle<GameObject> obj)
    //{
    //    m_ToLoadCount--;

    //    if (m_ToLoadCount <= 0)
    //        m_AssetsReady = true;
    //}

    //private void OnDestroy() //TODO: Should we teach instantiate with game objects and then manually release?
    //{
    //    foreach (var character in m_Characters)
    //    {
    //        character.ReleaseAsset();
    //    }
    //}
}
