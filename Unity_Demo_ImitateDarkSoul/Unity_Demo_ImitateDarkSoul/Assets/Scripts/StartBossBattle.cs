using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class StartBossBattle : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                BossMessageCenter.Instance.SendMessage("BeginBossBattle");
                gameObject.SetActive(false);
            }
        }
    }

}