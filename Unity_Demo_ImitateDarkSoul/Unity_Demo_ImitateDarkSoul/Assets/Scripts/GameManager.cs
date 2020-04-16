using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LS.Common;
using System.Text;
using LS.Common.Message;
using System;

namespace Souls
{

    public class GameManager : MonoSingletionBasis<GameManager>
    {
        #region Fields
        WeaponFactory _factory;
        GameObject _player;
        #endregion

        #region Mono Callbacks
        protected override void Init()
        {
            //keyValuePairs.Add("Falchion", new WeaponValue() { ATK = 20, DEF = 30,Type=WeaponType.Sword});
            //keyValuePairs.Add("Sword", new WeaponValue() { ATK = 10, DEF = 10,Type = WeaponType.Sword });
            //keyValuePairs.Add("Mace", new WeaponValue() { ATK = 50, DEF = 30, Type = WeaponType.WoodenClub });
            //keyValuePairs.Add("Test", new WeaponValue() { ATK = 0, DEF = 0, Type = WeaponType.None });

            //var s = IOHelper.SerializeObject(keyValuePairs, Newtonsoft.Json.Formatting.Indented);
            //IOHelper.Stream_FileWriter(Application.dataPath + "/Resources/WeaponData.json", s, false, Encoding.UTF8);
            _factory = new WeaponFactory((Resources.Load("WeaponData") as TextAsset).text);
            //_factory.Log();
            _player = GameObject.Find("Player");
        }

        private void OnEnable()
        {
            MessageCenter.Instance.AddListener("ChangeWeapon", ChangeWeapon);
        }

        private void Start()
        {
            //武器初始化
            var wm = _player.GetComponentInChildren<WeaponManager>();
            _factory.CreateWeapon("MoonSword", wm);
            wm.SetWeaponData(0);
            _factory.CreateWeapon("Shield", wm, false);
            wm.SetWeaponData(0, false);
            //给玩家的Collider赋值
            wm.ChangeWeaponCollider(0);
            var list = new List<string>();
            IOHelper.GetFileNameToArray(ref list, "/Resources/Audio/Used");
            foreach (var cell in list)
            {
                AudioManager.Instance.SetAudioPath(cell, "Audio/Used/" + cell);
            }
            AudioManager.Instance.SetSFXVolume(1f);
            AudioManager.Instance.PoolLock = true;//保护程序不会崩掉


            //test

            BossMessageCenter.Instance.SendMessage("BeginBossBattle");
        }

        #endregion

        #region Public Methods

        public GameObject CreateWeapon(string name)
        {
            return _factory.CreateWeapon(name, _player.GetComponentInChildren<WeaponManager>());
        }


        #endregion

        #region Messages 
        private void ChangeWeapon(GameObject render, EventArgs e)
        {
            SwitchWeaponEventArgs args = e as SwitchWeaponEventArgs;
            WeaponManager wm = _player.GetComponent<ActorManager>().WM;
            //显示
            int index = wm.GetWeaponIndex(args.SwitchName);
            Debug.Log("GameManger" + index);
            wm.RightWC.WeaponEnable(index, true);
            if(index==-1)
            {
                Debug.LogError("GameManger/ChangeWeapon Error : can't get the weapon data");
                return;
            }
            wm.ChangeWeaponCollider(index);
            //关闭
            index = wm.GetWeaponIndex(args.CurrentName);
            if (index == -1)
            {
                Debug.LogError("GameManger/ChangeWeapon Error : can't get the weapon data");
                return;
            }
            AudioManager.Instance.PlaySFX("SwitchWeapon");
            wm.RightWC.WeaponEnable(index, false);
        }

        #endregion
    }

}