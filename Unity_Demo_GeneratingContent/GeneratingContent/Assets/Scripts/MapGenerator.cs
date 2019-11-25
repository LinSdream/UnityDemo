using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Game
{
    public class MapGenerator : MonoBehaviour
    {
        #region Fields

        public int Width;
        public int Hight;
        public string Seed;
        public bool UseRandomSeed;
        [Range(0,100)] public int RandomFillPercent;

        int[,] _map;

        #endregion

        #region MonoBehaiour Callbacks
        private void Start()
        {
            
            GeneratorMap();
        }

        private void OnDrawGizmos()
        {
            if (_map != null)
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Hight; y++)
                    {
                        Gizmos.color = (_map[x, y] == 1) ? Color.black : Color.white;
                        Vector3 pos = new Vector3(-Width / 2 + x + .5f, 0, Hight / 2 + y + .5f);
                        Gizmos.DrawCube(pos, Vector3.one);
                    }
                }
            }
        }


        #endregion

        #region Private Methods
        private void GeneratorMap()
        {
            _map = new int[Width, Hight];
            RandomFillMap();
        }

        private void RandomFillMap()
        {
            if (UseRandomSeed)
            {
                Seed = Time.time.ToString();//伪随机取值以时间为种子
            }
            Random pasudoRandom = new Random(Seed.GetHashCode());
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Hight; y++)
                {
                    //填充map，如果随机值大于填充百分比则作为墙否则就是空白
                    _map[x, y] = pasudoRandom.Next(0, 100) < RandomFillPercent ? 1 : 0;
                }
        }
        #endregion
    }

}