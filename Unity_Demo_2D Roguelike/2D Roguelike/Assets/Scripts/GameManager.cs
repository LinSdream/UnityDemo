using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LS.Common;

namespace Game
{
    public class GameManager : ASingletonBasis<GameManager>
    {

        #region Fields
        public float TurnDelay = 0.1f;
        public int PlayerFoodPoints = 100;
        [HideInInspector] public bool PlayersTurn = true;

        List<Enemy> _enemies;
        BoardManager _boardMgr;
        int _level = 3;
        bool _enemiesMoving;
        #endregion

        #region MonoBehaviour Callbacks
        private void Update()
        {
            if (PlayersTurn || _enemiesMoving) return;
            StartCoroutine(MoveEnemies());
        }
        #endregion

        #region Override Methods
        public override void Init()
        {
            _enemies = new List<Enemy>();

            _boardMgr = GetComponent<BoardManager>();
            InitGame();
        }

        public void AddEnemyToList(Enemy enemy)
        {
            _enemies.Add(enemy);
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
            _enemies.Clear();
            _boardMgr.SetupScene(_level);
        }
        #endregion

        #region Coroutinues
        IEnumerator MoveEnemies()
        {
            _enemiesMoving = true;
            yield return new WaitForSeconds(TurnDelay);
            if (_enemies.Count == 0)
            {
                yield return new WaitForSeconds(TurnDelay);
            }
            foreach(Enemy cell in _enemies)
            {
                cell.EnemyMove();
                yield return new WaitForSeconds(cell.MoveTime);
            }
            PlayersTurn = true;
            _enemiesMoving = false;
        }
        #endregion
    }

}