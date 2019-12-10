using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets; //TODO: Mention the use of this namespace
using UnityEngine.ResourceManagement.AsyncOperations; // TODO: Mention that this is needed to do the async operations over the lists?
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.UI;


public class TowerManager : MonoBehaviour
{
    #region Fileds
    public AssetLabelReference AssetLabel;
    public List<AssetLabelReference> AssetLabels;
    public Button[] TowerCards;
    [HideInInspector]public bool IsAddressableToInit = false;

    //two ways to instantiate
    //youtube:https://www.youtube.com/watch?v=iauWgEXjkEY
    public IList<GameObject> Towers;//GameObject.Instantate
    public IList<IResourceLocation> TowersResources;//Addressable.Instantate

    #endregion

    #region MonoBehaviour Callbacks
    private void Start()
    {

        //var a = Addressables.LoadResourceLocationsAsync(AssetLabel,null);
        if(false)
        {
            Addressables.LoadAssetsAsync<IResourceLocation>(AssetLabel.RuntimeKey, null).Completed += OnLoadCompletedByIResourceLocaion;
        }
        else
        {
            Addressables.LoadAssetsAsync<GameObject>(AssetLabel, null).Completed += OnLoadCompleted;
        }

    }

    private void OnLoadCompletedByIResourceLocaion(AsyncOperationHandle<IList<IResourceLocation>> obj)
    {
        TowersResources = obj.Result;
        foreach (var cell in TowerCards)
        {
            cell.interactable = true;
        }
    }

    private void OnLoadCompleted(AsyncOperationHandle<IList<GameObject>> obj)
    {
        Towers = obj.Result;
        foreach(var cell in TowerCards)
        {
            cell.interactable = true;
        }
    }
    #endregion

    #region Buttons
    public void InstantiateTower(int index)
    {
        Vector3 pos = Random.insideUnitSphere * 5;
        pos.Set(pos.x, 0, pos.z);

        if (IsAddressableToInit)
        {
            if(TowersResources!=null)//利用Addressable来实例化，该实例化为异步实例
            {
                Addressables.InstantiateAsync(TowersResources[index], pos, Quaternion.identity).Completed += InstantiateCompleted;
            }
        }
        else
        {
            if (Towers != null)//以往的实例方法
            {
                Instantiate(Towers[index], pos, Quaternion.identity);
            }
        }
    }

    private void InstantiateCompleted(AsyncOperationHandle<GameObject> obj)
    {
        Debug.Log("Addressables.InstantiateAsync completed");
    }

    #endregion

    #region Completed
    //public IList<GameObject> m_Towers;

    //public AssetLabelReference m_TowerLabel;

    //public Button[] m_TowerCards;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    Addressables.LoadAssetsAsync<GameObject>(m_TowerLabel, null).Completed += OnResourcesRetrieved;
    //}

    //private void OnResourcesRetrieved(AsyncOperationHandle<IList<GameObject>> obj)
    //{
    //    m_Towers = obj.Result;

    //    //Activate the tower cards since their assets are now loaded
    //    foreach(var towerCard in m_TowerCards)
    //    {
    //        towerCard.interactable = true;
    //    }
    //}

    //public void InstantiateTower(int index)
    //{
    //    if(m_Towers != null)
    //    {
    //        Vector3 position = Random.insideUnitSphere * 5;
    //        position.Set(position.x, 0, position.z);
    //        Instantiate(m_Towers[index], position, Quaternion.identity, null);
    //    }
    //}
    #endregion
}
