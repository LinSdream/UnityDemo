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

        public Coord(int x, int y)
        {
            TileX = x;
            TileY = y;
        }
    }
    #endregion

    #region Class Room

    class Room
    {
        public List<Coord> Tiles;
        public List<Coord> EdgeTiles;
        public List<Room> ConnectedRooms;

        public int RoomSize;

        public Room() { }

        public Room(List<Coord> roomTiles,int [,] map)
        {
            Tiles = roomTiles;
            RoomSize = Tiles.Count;
            ConnectedRooms = new List<Room>();

            EdgeTiles = new List<Coord>();
            foreach(Coord cell in Tiles)
            {
                for(int x=cell.TileX-1;x<=cell.TileX+1;x++)
                {
                    for(int y=cell.TileY;y<=cell.TileY+1;y++)
                    {
                        if(x==cell.TileX||y==cell.TileY)
                        {
                            if (map[x, y] == 1)
                                EdgeTiles.Add(cell);
                        }
                    }
                }
            }
        }

        public static void ConnectRooms(Room roomA,Room roomB)
        {
            roomA.ConnectedRooms.Add(roomB);
            roomB.ConnectedRooms.Add(roomA);
        }

        public bool IsConnected(Room otherRoom)
        {
            return ConnectedRooms.Contains(otherRoom);
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
        public int WallThresholdSize = 50;
        public int RoomThresholdSize = 50;

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

            ProcessMap();

            int[,] borderMap = new int[Width + BorderSize * 2, Hight + BorderSize * 2];

            int borderX = borderMap.GetLength(0);
            int borderY = borderMap.GetLength(1);

            for (int x = 0; x < borderX; x++)
            {
                for (int y = 0; y < borderY; y++)
                {
                    if ((x >= BorderSize && x < BorderSize + Width) && (y >= BorderSize && y < BorderSize + Hight))
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
                    if (IsInWallRange(neighbourX, neighbourY))
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

        List<Coord> GetRegionTiles(int startX, int startY)
        {
            List<Coord> tiles = new List<Coord>();
            Queue<Coord> queue = new Queue<Coord>();

            int[,] mapFlags = new int[Width, Hight];
            int tileType = _map[startX, startY];

            //第一个点进入
            queue.Enqueue(new Coord(startX, startY));
            _map[startX, startY] = 1;

            while (queue.Count > 0)
            {
                Coord tile = queue.Dequeue();
                tiles.Add(tile);

                for (int x = tile.TileX - 1; x <= tile.TileX + 1; x++)
                {
                    for (int y = tile.TileY - 1; y <= tile.TileY + 1; y++)
                    {
                        //只需要上下左右四个点的贴图信息，因此必然存在一个x or y 等于该点的x or y
                        if (IsInWallRange(x, y) && (y == tile.TileY || x == tile.TileX))
                        {
                            if (mapFlags[x, y] == 0 && _map[x, y] == tileType)
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

        List<List<Coord>> GetRegions(int tileType)
        {
            List<List<Coord>> regions = new List<List<Coord>>();
            int[,] mapFlags = new int[Width, Hight];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Hight; y++)
                {
                    if (mapFlags[x, y] == 0 && _map[x, y] == tileType)
                    {
                        List<Coord> newRegions = GetRegionTiles(x, y);
                        regions.Add(newRegions);

                        foreach (Coord cell in newRegions)
                        {
                            mapFlags[cell.TileX, cell.TileY] = 1;
                        }
                    }
                }
            }
            return regions;
        }

        void ProcessMap()
        {
            List<List<Coord>> wallRegions = GetRegions(1);

            foreach (List<Coord> cell in wallRegions)
            {
                if (cell.Count < WallThresholdSize)
                {
                    foreach (Coord tile in cell)
                    {
                        _map[tile.TileX, tile.TileY] = 0;
                    }
                }
            }

            List<List<Coord>> roomRegions = GetRegions(0);
            List<Room> survivingRooms = new List<Room>();
            foreach (List<Coord> cell in roomRegions)
            {
                if (cell.Count < RoomThresholdSize)
                {
                    foreach (Coord tile in cell)
                    {
                        _map[tile.TileX, tile.TileY] = 1;
                    }
                }
                else
                {
                    survivingRooms.Add(new Room(cell, _map));
                }
            }
        }

        void ConnectClosestRooms(List<Room> allRooms)
        {
            int bestDistance = 0;
            Coord bestTileA = new Coord();
            Coord bestTileB = new Coord();
            Room bestRoomA = new Room();
            Room bestRoomB = new Room();

            bool possibleConnectionFound = false;

            foreach(Room roomA in allRooms)
            {
                possibleConnectionFound = false;
                foreach (Room roomB in allRooms)
                {
                    if (roomA == roomB)
                        continue;
                    if (roomA.IsConnected(roomB))
                        break;
                    for (int tileIndexA = 0; tileIndexA < roomA.EdgeTiles.Count; tileIndexA++)
                    {
                        for (int tileIndexB = 0; tileIndexB < roomB.EdgeTiles.Count; tileIndexB++)
                        {
                            Coord tileA = roomA.EdgeTiles[tileIndexA];
                            Coord tileB = roomB.EdgeTiles[tileIndexB];

                            int distanceBetweenRooms = (int)(Mathf.Pow(tileA.TileX - tileB.TileX, 2) + Mathf.Pow(tileA.TileY - tileB.TileY, 2));
                            if (distanceBetweenRooms < bestDistance || !possibleConnectionFound)
                            {
                                bestDistance = distanceBetweenRooms;
                                possibleConnectionFound = true;
                                bestTileA = tileA;
                                bestTileB = tileB;
                                bestRoomA = roomA;
                                bestRoomB = roomB;
                            }
                        }
                    }

                    if (possibleConnectionFound)
                        CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);

                }
            }
        }

        void CreatePassage(Room roomA,Room roomB,Coord tileA,Coord coord)
        {

        }

        bool IsInWallRange(int x, int y)
        {
            return (x >= 0 && x < Width) && (y > -0 && y < Hight);
        }

        #endregion
    }

}