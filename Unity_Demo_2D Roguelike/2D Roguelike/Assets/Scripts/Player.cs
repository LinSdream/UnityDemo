using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public class Player : MoveBase
    {

        #region Fields
        public int WallDamage = 1;
        public int PointsPerFood = 10;
        public int PointsPerSoda = 20;
        public float RestartLevelDlay = 1f;
        public Text FoodText;

        Animator _anim;
        int _food;
        #endregion

        #region MonoBehaviour Callbacks

        private void Update()
        {
            if (!GameManager.Instance.PlayersTurn) return;
            int horizontal = (int)Input.GetAxisRaw("Horizontal");
            int vertical = (int)Input.GetAxisRaw("Vertical");

            //防止对角移动
            if (horizontal != 0)
                vertical = 0;

            if(horizontal!=0 || vertical != 0)
            {
                AttemptMove<DestructibleWall>(horizontal, vertical);
            }
            
        }

        private void OnDisable()
        {
            GameManager.Instance.PlayerFoodPoints = _food;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            string tag = collision.tag;
            if (tag == "Exit")
            {
                Invoke("Restart", RestartLevelDlay);
                enabled = false;
            }else if (tag == "Food")
            {
                _food += PointsPerFood;
                FoodText.text = "+" + PointsPerFood + " Food: " + _food;
                collision.gameObject.SetActive(false);
            }else if (tag == "Soda")
            {
                _food += PointsPerSoda;
                FoodText.text = "+" + PointsPerSoda + " Food: " + _food;
                collision.gameObject.SetActive(false);
            }
        }
        #endregion

        #region Private Methods
        void CheckGameOver()
        {
            if (_food <= 0)
                GameManager.Instance.GameOver();
        }

        void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        #endregion

        #region Override Methods

        protected override void Init()
        {
            _anim = GetComponent<Animator>();
            _food = GameManager.Instance.PlayerFoodPoints;
            FoodText.text = "Food : " + _food;
        }

        protected override void AttemptMove<T>(float xDir, float yDir)
        {
            _food--;
            FoodText.text = "Food : " + _food;
            base.AttemptMove<T>(xDir, yDir);

            RaycastHit2D hit;

            CheckGameOver();
            GameManager.Instance.PlayersTurn = false;
        }

        protected override void OnCantMove<T>(T component)
        {
            DestructibleWall hitWall = component as DestructibleWall;
            hitWall.DamageWall(WallDamage);
            _anim.SetTrigger("PlayerChop");
        }
        #endregion

        #region Public Methods
        public void LoseFood(int loss)
        {
            _anim.SetTrigger("PlayerHit");
            _food -= loss;
            FoodText.text = "-" + loss + " Food: " + _food;
            CheckGameOver();
        }
        #endregion
    }

}