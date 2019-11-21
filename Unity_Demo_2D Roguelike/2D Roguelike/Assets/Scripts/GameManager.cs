using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LS.Common;

namespace Game
{
    public class GameManager : ASingletonBasis<GameManager>
    {

        #region Fields
        public int PlayerFoodPoints = 100;
        [HideInInspector] public bool PlayersTurn = true;

        BoardManager _boardMgr;
        int _level = 3;
        #endregion

        #region MonoBehaviour Callbacks

        #endregion

        #region Override Methods
        public override void Init()
        {
            _boardMgr = GetComponent<BoardManager>();
            InitGame();
        }
        #endregion

        #region Public Methods
        public void GameOver()
        {
            enabled = false;
        }
        #endregion

        #region Private Methods
        void InitGame()
        {
            _boardMgr.SetupScene(_level);
        }
        #endregion
    }

}