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
        [Tooltip("自定义种子")]
        public string Seed;
        [Tooltip("随机种子")]
        public bool UseRandomSeed;
        [Tooltip("平滑级别，将周围黑色相互融合，级别越高，融合次数越多")]
        [Range(0, 10)] public int SmoothLevel = 5;
        [Tooltip("黑色块占比")]
        [Range(0, 100)] public int RandomFillPercent;

        int[,] _map;

        #endregion

        #region MonoBehaiour Callbacks
        private void Start()
        {
            GeneratorMap();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                GeneratorMap();
            }
        }

        private void OnDrawGizmos()
        {
            //if (_map != null)
            //{
            //    for (int x = 0; x < Width; x++)
            //    {
            //        for (int y = 0; y < Hight; y++)
            //        {
            //            Gizmos.color = (_map[x, y] == 1) ? Color.black : Color.white;
            //            Vector3 pos = new Vector3(-Width / 2 + x + .5f, 0, -Hight / 2 + y + .5f);
            //            Gizmos.DrawCube(pos, Vector3.one);
            //        }
            //    }
            //}
        }

        #endregion

        #region Private Methods
        void GeneratorMap()
        {
            _map = new int[Width, Hight];
            RandomFillMap();
            for (int i = 0; i < SmoothLevel; i++)
            {
                SmoothMap();
            }

            MeshGenerator meshGen = GetComponent<MeshGenerator>();
            meshGen.GeneratorMesh(_map, 1);
        }

        void RandomFillMap()
        {
            if (UseRandomSeed)
            {
                Seed = Time.time.ToString();//伪随机取值以时间为种子
            }
            Random pseuaoRandom = new Random(Seed.GetHashCode());
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Hight; y++)
                {
                    if (x == 0 || x == Width - 1 || y == 0 || y == Hight - 1)
                    {
                        _map[x, y] = 1;
                    }
                    else
                    {
                        //填充map，如果随机值大于填充百分比则作为墙否则就是空白
                        _map[x, y] = pseuaoRandom.Next(0, 100) < RandomFillPercent ? 1 : 0;
                    }
                }
        }

        void SmoothMap()
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Hight; y++)
                {
                    int neighbourCountTiles = GetSurrondWallCount(x, y);
                    if (neighbourCountTiles > 4)
                    {
                        _map[x, y] = 1;
                    }
                    else if (neighbourCountTiles < 4)
                        _map[x, y] = 0;
                }
        }

        /// <summary>
        /// 获取该tile周围八个的颜色情况（上下左右，左上左下右上右下）
        /// </summary>
        int GetSurrondWallCount(int girX, int girY)
        {
            int wallCount = 0;
            for (int neighbourX = girX - 1; neighbourX <= girX + 1; neighbourX++)
            {
                for (int neighbourY = girY - 1; neighbourY <= girY + 1; neighbourY++)
                {
                    if (neighbourX >= 0 && neighbourX < Width && neighbourY >= 0 && neighbourY < Hight)
                    {
                        if (neighbourX != girX || neighbourY != girY)
                        {
                            wallCount += _map[neighbourX, neighbourY];
                        }
                    }
                    else
                        wallCount++;
                }
            }
            return wallCount;
        }
        #endregion
    }

}