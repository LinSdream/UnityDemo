using LS.Common;
using Souls.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{

    public class EnemyManager:MonoSingletonBasis<EnemyManager>
    {

        struct EnemyStatus
        {
            public BlackKnightFSM FSM;
            public EnemyStatus(BlackKnightFSM fsm)
            {
                FSM = fsm;
            }
        }

        List<EnemyStatus> _enemies = new List<EnemyStatus>();
        int _timer = 0;

        private void Update()
        {
            _timer++;
            if (_timer >= 25)//每25帧重置一次
            {
                //攻击敌人更换
                EnemiesCanAttack();
                _timer = 0;
            }

        }

        /// <summary>添加FSM</summary>
        public int AddEnemies(BlackKnightFSM fsm)
        {
            if(_enemies.FindIndex((value) =>
            {
                return (value.FSM == fsm);
            }) != -1)
            {
                Debug.LogWarning("GameManager/AddEnemies Warning : the fsm already exist ");
                return -1;
            }
            _enemies.Add(new EnemyStatus() { FSM = fsm});
            return _enemies.Count - 1;
        }

        public void RemoveEnemies(int index)
        {
            if(index==-1||index>=_enemies.Count)
            {
                Debug.LogError("EnemyManager/RemoveEnemies Error : Invalid value . index is " + index);
                return;
            }
            _enemies.RemoveAt(index);
            //重置所有的状态机的index
            for(int i=0;i<_enemies.Count;i++)
            {
                _enemies[i].FSM.CurrentEnemiesIndex = i;
            }
        }

        void EnemiesCanAttack()
        {
            if (_enemies.Count == 0)
                return;
            int random = Random.Range(1, _enemies.Count);
            
            EnemyStatus[] arr = _enemies.GetRandomArrayFromList(1, random);
            for(int i=0;i<arr.Length;i++)
            {
                arr[i].FSM.IsReady = !arr[i].FSM.IsReady;
            }
            
        }

    }
}