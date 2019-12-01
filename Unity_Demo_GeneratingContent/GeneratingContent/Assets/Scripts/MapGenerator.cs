using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Game
{
    #region Struct Coord
    struct Coord
    {
        public int TileX;
        public int TileY;

        public Coord(int x,int y)
        {
            TileX = x;
            TileY = y;
        }
    }
    #endregion

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
        [Tooltip("边缘轮廓尺寸")]
        public int BorderSize = 5;
        
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

            
            int[,] borderMap = new int[Width + BorderSize * 2, Hight + BorderSize * 2];

            int borderX = borderMap.GetLength(0);
            int borderY = borderMap.GetLength(1);

            for(int x = 0; x < borderX; x++)
            {
                for(int y = 0; y < borderY; y++)
                {
                    if((x >= BorderSize && x < BorderSize + Width) && (y >= BorderSize && y < BorderSize + Hight))
                    {
                        borderMap[x, y] = _map[x - BorderSize, y - BorderSize];
                    }
                    else
                    {
                        borderMap[x, y] = 1;
                    }
                }
            }

            MeshGenerator meshGen = GetComponent<MeshGenerator>();
            meshGen.GeneratorMesh(borderMap, 1);
        }

        void RandomFillMap()
        {
            if (UseRandomSeed)
            {
                Seed = DateTime.Now.ToString();
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
                    if (IsInWallRange(neighbourX,neighbourY))
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

        List<Coord> GetRegionTiles(int startX,int startY)
        {
            List<Coord> tiles = new List<Coord>();
            Queue<Coord> queue = new Queue<Coord>();

            int[,] mapFlags = new int[Width, Hight];
            int tileType = _map[startX, startY];

            //第一个点进入
            queue.Enqueue(new Coord(startX, startY));
            _map[startX, startY] = 1;
            
            while(queue.Count>0)
            {
                Coord tile = queue.Dequeue();
                tiles.Add(tile);

                for(int x=tile.TileX-1;x<=tile.TileX+1;x++)
                {
                    for(int y=tile.TileY-1;y<=tile.TileY+1;y++)
                    {
                        //只需要上下左右四个点的贴图信息，因此必然存在一个x or y 等于该点的x or y
                        if (IsInWallRange(x, y) && (y == tile.TileY || x == tile.TileX))
                        {
                            if(mapFlags[x,y]==0&&mapFlags[x,y]==tileType)
                            {
                                mapFlags[x, y] = 1;
                                queue.Enqueue(new Coord(x, y));
                            }
                        }
                    }
                }
            }
            return tiles;
        }

        bool IsInWallRange(int x,int y)
        {
            return (x >= 0 && x < Width) && (y >- 0 && y < Hight);
        }

        #endregion
    }

}