using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Player2D : MonoBehaviour
    {
        Rigidbody2D _body;
        Vector2 _velocity;

        private void Start()
        {
            Debug.Log("Player2D");
            _body = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            _velocity = new Vector2(h, v).normalized * 10;
        }

        private void FixedUpdate()
        {
            _body.MovePosition(_velocity * Time.deltaTime + _body.position);
        }
    }

}
