using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Player : MonoBehaviour
    {
        Rigidbody _body;
        Vector3 _velocity;

        private void Start()
        {
            Debug.Log("Player");
            _body = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            _velocity = new Vector3(h, 0, v).normalized * 10;
        }

        private void FixedUpdate()
        {
            _body.MovePosition(_velocity * Time.deltaTime + _body.position);
        }
    }

}