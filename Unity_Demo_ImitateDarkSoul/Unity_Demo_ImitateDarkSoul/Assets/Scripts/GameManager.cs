using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LS.Common;
using System.Text;
using LS.Common.Message;
using System;
using UnityEngine.UI;
using LS.Others;

namespace Souls
{

    public class GameManager : MonoSingletonBasis<GameManager>
    {
        #region Fields
        public Image SettlementPanel;

        Text _loseText;
        Text _winText;
        WeaponFactory _factory;
        ActorManager _player;
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
            _player = GameObject.Find("Player").GetComponent<ActorManager>();

            var texts = SettlementPanel.GetComponentsInChildren<Text>();
            _loseText = texts[0];
            _winText = texts[1];
        }

        private void OnEnable()
        {
            MessageCenter.Instance.AddListener("ChangeWeapon", ChangeWeapon);
        }

        private void Start()
        {

            _winText.gameObject.SetActive(false);
            _loseText.gameObject.SetActive(false);

            //武器初始化
            _factory.CreateWeapon("MoonSword", _player.WM);
            _player.WM.SetWeaponData(0);
            _factory.CreateWeapon("Shield", _player.WM, false);
            _player.WM.SetWeaponData(0, false);
            //给玩家的Collider赋值
            _player.WM.ChangeWeaponCollider(0);
           
        }

        #endregion

        #region Public Methods

        public GameObject CreateWeapon(string name)
        {
            return _factory.CreateWeapon(name, _player.GetComponentInChildren<WeaponManager>());
        }

        public void Settlement(bool isWin)
        {
            if(isWin)
            {
                _winText.gameObject.SetActive(true);
            }
            else 
            {
                _loseText.gameObject.SetActive(true);
                AudioManager.Instance.StopMusic();
                CustomSceneManager.Instance.CustomLoadScene("00_Menu");
            }
        }

        public bool PlayerIsDie()
        {
            return _player.SM.CharacterState.IsDie;
        }
        #endregion

        #region Messages 
        private void ChangeWeapon(GameObject render, EventArgs e)
        {
            SwitchWeaponEventArgs args = e as SwitchWeaponEventArgs;
            WeaponManager wm = _player.GetComponent<ActorManager>().WM;
            //显示
            int index = wm.GetWeaponIndex(args.SwitchName);
            wm.RightWC.WeaponEnable(index, true);
            if (index == -1)
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