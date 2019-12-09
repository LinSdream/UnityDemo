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
    public List<AssetLabelReference> AssetLabels;
    public IList<GameObject> Towers;
    public Button[] TowerCards;

    #endregion

    #region MonoBehaviour Callbacks
    private void Start()
    {
        ///TODO:研究多标签loadassets
        //Addressables.LoadAssetsAsync<GameObject>()
        Addressables.LoadAssetsAsync<GameObject>(AssetLabels, null).Completed += OnLoadCompleted;
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
        if(Towers!=null)
        {
            Vector3 pos = Random.insideUnitSphere * 5;
            pos.Set(pos.x, 0, pos.z);
            Instantiate(Towers[index],pos, Quaternion.identity);
        }
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
