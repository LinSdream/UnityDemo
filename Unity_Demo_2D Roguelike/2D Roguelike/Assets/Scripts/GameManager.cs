using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LS.Common;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameManager : ASingletonBasis<GameManager>
    {

        #region Fields
        public float TurnDelay = 0.1f;
        public int PlayerFoodPoints = 100;
        public float LevelStartDelay = 2f;
        [HideInInspector] public bool PlayersTurn = true;

        Text _levelText;
        GameObject _levelImg;
        List<Enemy> _enemies;
        BoardManager _boardMgr;
        int _level = 1;
        bool _enemiesMoving;
        bool _doingSetup;//检测是否在设置UI
        #endregion

        #region MonoBehaviour Callbacks
        private void Update()
        {
            Debug.Log("Update");
            if (PlayersTurn || _enemiesMoving || _doingSetup) return;
            StartCoroutine(MoveEnemies());
        }

        //允许在运行时加载游戏而无需用户采取行动的情况下初始化运行时类方法。
        //[RuntimeInitializeOnLoadMethod] 加载游戏后，将调用标记的方法。这是在Awake调用方法之后。
        //[RuntimeInitializeOnLoadMethod]不能保证标记方法的执行顺序。
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static public void CallbackInitialize()
        {
            SceneManager.sceneLoaded += OnSceneLoad;
        }

        private static void OnSceneLoad(Scene arg0, LoadSceneMode arg1)
        {
            Instance._level++;
            Instance.InitGame();
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
            _levelText.text = "After " + _level + " days , you starved.";
            _levelImg.SetActive(true);
            enabled = false;
        }
        #endregion

        #region Private Methods

        
        void InitGame()
        {
            _doingSetup = true;

            _levelImg = GameObject.Find("Level_Img");
            _levelText = GameObject.Find("Level_Text").GetComponent<Text>();

            _levelText.text = "Day " + _level;
            Invoke("HideLevelImg", LevelStartDelay);

            _enemies.Clear();
            _boardMgr.SetupScene(_level);
        }

        void HideLevelImg()
        {
            _levelImg.SetActive(false);
            _doingSetup = false;
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