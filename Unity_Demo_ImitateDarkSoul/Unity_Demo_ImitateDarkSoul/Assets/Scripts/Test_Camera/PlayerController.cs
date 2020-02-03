using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.TestCamera
{
    public class PlayerController : MonoBehaviour
    {
        public GameObject Model;
        public float MoveSpeed = 5f;
        [Tooltip("旋转速度")] public float RotationSpeed = 10f;

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
            _moveDir = (_input.Horizontal * transform.right + _input.Vertical * transform.forward) * MoveSpeed;
            Rotate();
        }

        private void FixedUpdate()
        {
            Movement();
        }

        void Movement()
        {
            _rigidbody.MovePosition(_moveDir * Time.fixedDeltaTime + _rigidbody.position);
        }

        void Rotate()
        {
            if (_input.Horizontal != 0||_input.Vertical!=0)
            {
                //旋转以transform的正方向为基准
                var forward = _input.Horizontal * transform.right + _input.Vertical * transform.forward;
                Quaternion quaternion = Quaternion.LookRotation(forward, Vector3.up);
                quaternion = Quaternion.Slerp(Model.transform.rotation, quaternion, Time.deltaTime * RotationSpeed);
                Model.transform.rotation = quaternion;
            }
        }
    }

}