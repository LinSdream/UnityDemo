using LS.Others;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class MenuUI : MonoBehaviour
    {
       
        public void Btn_StartGame()
        {
            CustomSceneManager.Instance.CustomLoadSceneAsync("01_Main");
        }

    }

}