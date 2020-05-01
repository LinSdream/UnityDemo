using LS.Others;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class MenuUI : MonoBehaviour
    {

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void Btn_StartGame()
        {
            CustomSceneManager.Instance.CustomLoadSceneAsync("01_Main");
        }

    }

}