using LS.Common.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Souls
{
    public class WeaponUI : MonoBehaviour
    {
        public Text CurrentWeaponTxt;
        public GameObject WeaponUIItemPrefab;
        public GameObject Content;

        private void OnEnable()
        {
            MessageCenter.Instance.AddListener("WeaponUIChange", WeaponUIChange);
            MessageCenter.Instance.AddListener("WeaponAdd", WeaponAdd);
        }

        private void OnDestroy()
        {
            MessageCenter.Instance.RemoveListener("WeaponUIChange", WeaponUIChange);
            MessageCenter.Instance.RemoveListener("WeaponAdd", WeaponAdd);
        }

        private void WeaponAdd(GameObject render, EventArgs e)
        {

            var go = Instantiate(WeaponUIItemPrefab);
            go.transform.SetParent(Content.transform);
            go.transform.localScale = Vector3.one;
            go.GetComponentInChildren<Text>().text = (e as WeaponUIEventArgs).WeaponName;

            go.GetComponent<Button>().onClick.AddListener(() =>
            {
                var temp = CurrentWeaponTxt.text;
                CurrentWeaponTxt.text = go.GetComponentInChildren<Text>().text;
                go.GetComponentInChildren<Text>().text = temp;
                MessageCenter.Instance.SendMessage("ChangeWeapon",go.gameObject,new SwitchWeaponEventArgs() { CurrentName= temp,
                SwitchName= CurrentWeaponTxt.text
                });
            });
            render.SetActive(false);
        }

        private void WeaponUIChange(GameObject render, EventArgs e)
        {
            if (!render.CompareTag("Player"))
                return;
            CurrentWeaponTxt.text = (e as WeaponUIEventArgs).WeaponName;
        }

    }

}