using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MapGenerator : MonoBehaviour
    {
        #region Fields

        public int Width;
        public int Hight;

        [Range(0,100)]
        public int RandomFillPercent;

        int[,] _map;
        #endregion

        #region MonoBehaiour Callbacks
        private void Start()
        {
            _map = new int[Width, Hight];
            GeneratorMap();
        }

        private void GeneratorMap()
        {

        }
        #endregion

    }

}