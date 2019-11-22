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

        public AudioClip[] MoveSounds;
        public AudioClip[] EatSounds;
        public AudioClip[] DrinkSounds;

        Vector2 _touchOrigin = -Vector2.one;//Used to store location of screen touch origin for mobile controls.
        Animator _anim;
        int _food;
        #endregion

        #region MonoBehaviour Callbacks

        private void Update()
        {
            if (!GameManager.Instance.PlayersTurn) return;

            int horizontal = 0;
            int vertical = 0;

#if  UNITY_EDITOR|| UNITY_STANDALONE || UNITY_WEBPLAYER	
            vertical = (int)Input.GetAxisRaw("Vertical");
            horizontal = (int)Input.GetAxisRaw("Horizontal");
            //防止对角移动
            if (horizontal != 0)
                vertical = 0;

#else
            if (Input.touchCount > 0)
            {
                Touch myTouch = Input.touches[0];
                if (myTouch.phase == TouchPhase.Began)
                {
                    _touchOrigin = myTouch.position;
                }else if(myTouch.phase==TouchPhase.Ended && _touchOrigin.x > 0)
                {
                    Vector2 touchEnd = myTouch.position;
                    float x = touchEnd.x - _touchOrigin.x;//本次touch的x轴位移
                    float y = touchEnd.y - _touchOrigin.y;//本次touch的y轴位移

                    _touchOrigin.x = -1;//重置touch，表示该次的touch结束，不需要其信息

                    if (Mathf.Abs(x) > Mathf.Abs(y))
                        horizontal = x > 0 ? 1 : -1;
                    else
                        vertical = y > 0 ? 1 : -1;
                }
            }
#endif
            if (horizontal != 0 || vertical != 0)
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
            }
            else if (tag == "Food")
            {
                _food += PointsPerFood;
                FoodText.text = "+" + PointsPerFood + " Food: " + _food;
                SoundManager.Instance.RandomPitchSFX(EatSounds);
                collision.gameObject.SetActive(false);
            }
            else if (tag == "Soda")
            {
                _food += PointsPerSoda;
                FoodText.text = "+" + PointsPerSoda + " Food: " + _food;
                SoundManager.Instance.RandomPitchSFX(DrinkSounds);
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

            if (Move(xDir, yDir, out hit))
            {
                SoundManager.Instance.RandomPitchSFX(MoveSounds);
            }

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