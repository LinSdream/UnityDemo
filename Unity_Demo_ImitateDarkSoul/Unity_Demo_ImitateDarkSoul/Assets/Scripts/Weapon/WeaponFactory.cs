using LS.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{

    public struct WeaponValue
    {
        public float ATK;
        public float DEF;
        public WeaponType Type;
    }

    public class WeaponFactory
    {

        Dictionary<string, WeaponValue> _dir = new Dictionary<string, WeaponValue>();

        public WeaponFactory(string data)
        {
            _dir = IOHelper.DeserializeObject<Dictionary<string, WeaponValue>>(data);
        }

        public void Log()
        {
            foreach(var pair in _dir)
            {
                Debug.Log(pair.Key + " : ATK=" + pair.Value.ATK + ", DEF=" + pair.Value.DEF + ", Type="+pair.Value.Type);
            }
        }


        public GameObject CreateWeapon(string name,WeaponManager wm,bool isRight=true)
        {
            //左右手判定
            WeaponController wc;
            if (isRight)
                wc = wm.RightWC;
            else
                wc = wm.LeftWC;
            //创建实例
            var prefab = Resources.Load(name) as GameObject;
            var go=GameObject.Instantiate(prefab);
            go.transform.parent = wc.transform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            //添加WeaponData
            var data = go.AddComponent<WeaponData>();
            data.ATK = _dir[name].ATK;
            data.WType = _dir[name].Type;
            data.DEF = _dir[name].DEF;
            data.WeaponName = name;
            wc.AddWeaponData(data);
            wc.WM.AddWeaponCollider(data.GetComponent<Collider>(), isRight);
            return go;
        }
    }

}