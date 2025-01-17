﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Enemy : MoveBase
    {
        #region Fields
        public int PlayerDamage = 1;
        public AudioClip[] AttackSounds;

        Transform _target;
        Animator _anim;
        /// <summary> 是否跳过移动 </summary>
        bool _skipMove;
        #endregion

        #region Public Methods
        public void EnemyMove()
        {
            int xDir = 0;
            int yDir = 0;
            if (Mathf.Abs(_target.position.x - transform.position.x) < float.Epsilon)
            {
                yDir = _target.position.y > transform.position.y ? 1 : -1;
            }
            else
            {
                xDir = _target.position.x > transform.position.x ? 1 : -1;
            }

            AttemptMove<Player>(xDir, yDir);
        }
        #endregion

        #region Override Methods
        protected override void Init()
        {
            GameManager.Instance.AddEnemyToList(this);
            _target = GameObject.FindGameObjectWithTag("Player").transform;
            _anim = GetComponent<Animator>();
        }

        protected override void AttemptMove<T>(float xDir, float yDir)
        {
            if (_skipMove)
            {
                _skipMove = false;
                return;
            }
            base.AttemptMove<T>(xDir, yDir);

            _skipMove = true;
        }

        protected override void OnCantMove<T>(T component)
        {
            Player playerHit = component as Player;
            _anim.SetTrigger("EnemyAttack");
            playerHit.LoseFood(PlayerDamage);
            SoundManager.Instance.RandomPitchSFX(AttackSounds);
        }
        #endregion
    }

}