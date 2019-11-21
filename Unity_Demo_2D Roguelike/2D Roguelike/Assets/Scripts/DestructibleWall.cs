using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class DestructibleWall : MonoBehaviour
    {
        public Sprite DmgSprite;
        public int HP;

        SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void DamageWall(int loss)
        {
            _spriteRenderer.sprite = DmgSprite;
            HP -= loss;
            if (HP <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

}