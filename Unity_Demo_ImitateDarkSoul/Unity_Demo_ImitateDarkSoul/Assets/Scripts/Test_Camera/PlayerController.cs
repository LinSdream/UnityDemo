using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.TestCamera
{
    public class PlayerController : MonoBehaviour
    {
        public GameObject Model;
        public float MoveSpeed = 5f;

        PlayerInput _input;
        Vector3 _moveDir;
        Rigidbody _rigidbody;

        private void Awake()
        {
            _moveDir = Vector3.zero;
            _rigidbody = GetComponent<Rigidbody>();
            _input = GetComponent<PlayerInput>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            _moveDir = _input.InputVec * MoveSpeed;
        }

        private void FixedUpdate()
        {
            Movement();
        }

        void Movement()
        {
            _rigidbody.MovePosition(_moveDir*Time.fixedDeltaTime + _rigidbody.position);
        }

        void Rotate()
        {
            
        }
    }

}