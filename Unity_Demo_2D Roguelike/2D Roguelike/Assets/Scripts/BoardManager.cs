using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace Game
{
    /// <summary>
    /// 随机生成的时候，用来限制随机生成的程度
    /// </summary>
    [Serializable]
    public class Count
    {
        public int Minimum;
        public int Maximum;

        public Count(int min,int max)
        {
            Minimum = min;
            Maximum = max;
        }
    }

    public class BoardManager : MonoBehaviour
    {
        #region Public Fields
        public int Columns = 8;
        public int Rows = 8;
        public Count InWallCount = new Count(5, 9);//最少5面，最多9面
        public Count FoodCount = new Count(1, 5);

        public GameObject Exit;
        public GameObject[] FloorTiles;
        public GameObject[] InWallTiles;
        public GameObject[] OutWallTTilse;
        public GameObject[] FoodTiles;
        public GameObject[] EnemyTiles;
        #endregion

        #region Private Fields
        Transform _boardHolder;//所有的子物体层级在该transform中
        List<Vector3> _gridPositions = new List<Vector3>();//存放所有可能性的位置
        #endregion

        #region MonoBehaviours Callbacks
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        #endregion

        #region Private Methods
        void InitialiseList()
        {
            _gridPositions.Clear();
            //填充所有可以填的方块，最外围不设为可填充，为了确保必然存在解
            for (int x = 1; x < Columns - 1; x++)//循环x轴
            {
                for (int y = 1; y < Rows - 1; y++)//y轴
                {
                    _gridPositions.Add(new Vector3(x, y, 0f));
                }
            }
        }

        void BoardSetup()
        {
            _boardHolder = new GameObject("Board").transform;

            for(int x = -1; x <= Columns; x++)
            {
                for(int y = -1; y <= Rows; y++)
                {
                    GameObject toInstantiate = FloorTiles[Random.Range(0, FloorTiles.Length)];
                    if (x == -1 || x == Columns || y == -1 || y == Rows)
                        toInstantiate = OutWallTTilse[Random.Range(0, OutWallTTilse.Length)];

                    GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(_boardHolder);

                }
            }
        }

        Vector3 RandomPos()
        {
            int index = Random.Range(0, _gridPositions.Count);
            Vector3 pos = _gridPositions[index];
            _gridPositions.RemoveAt(index);
            return pos;
        }

        void LayoutObjectAtRandom(GameObject[] tileArr,int min,int max)
        {
            int count = Random.Range(min, max+1);
            for(int i = 0; i < count; i++)
            {
                Vector3 randomPos = RandomPos();
                GameObject tileChoice = tileArr[Random.Range(0, tileArr.Length)];
                Instantiate(tileChoice, randomPos, Quaternion.identity);
            }
        }

        #endregion

        #region Public Methods
        public void SetupScene(int level)
        {
            BoardSetup();
            InitialiseList();
            LayoutObjectAtRandom(InWallTiles, InWallCount.Minimum, InWallCount.Maximum);
            LayoutObjectAtRandom(FoodTiles, FoodCount.Minimum, FoodCount.Maximum);
            int enemyNum = (int)Mathf.Log(level, 2f);
            LayoutObjectAtRandom(EnemyTiles, enemyNum, enemyNum);
            Instantiate(Exit, new Vector3(Columns - 1, Rows - 1, 0f), Quaternion.identity);
        }
        #endregion
    }

}