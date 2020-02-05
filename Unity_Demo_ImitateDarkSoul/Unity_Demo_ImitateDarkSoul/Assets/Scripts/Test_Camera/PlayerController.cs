using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.TestCamera
{
    public class PlayerController : MonoBehaviour
    {
        public GameObject Model;
        public CameraController CameraModel;
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
            _moveDir = Vector3.Scale(CameraModel.CameraObj.forward, new Vector3(1, 0, 1)) * _input.Vertical + _input.Horizontal * CameraModel.CameraObj.right;
            if (_moveDir.magnitude > 1f)
                _moveDir.Normalize();
            _moveDir = transform.InverseTransformDirection(_moveDir);

            //_moveDir = (_input.Horizontal * transform.right + _input.Vertical * transform.forward) * MoveSpeed;
            Rotate();
        }

        private void FixedUpdate()
        {
            Movement();
        }

        void Movement()
        {
            _rigidbody.MovePosition(Vector3.Scale(CameraModel.CameraObj.forward, new Vector3(1, 0, 1)) * _input.Vertical + _input.Horizontal * CameraModel.CameraObj.right * Time.fixedDeltaTime + _rigidbody.position);
        }

        void Rotate()
        {
            if (_input.Horizontal != 0||_input.Vertical!=0)
            {
                //transform.Rotate(0, Mathf.Lerp(180f,360f,  _moveDir.z*Time.deltaTime), 0);
                //旋转以Camera的transform的正方向为基准
                var forward = Vector3.Scale(CameraModel.CameraObj.forward, new Vector3(1, 0, 1)) * _input.Vertical + _input.Horizontal * CameraModel.CameraObj.right; ;//_input.Horizontal * transform.right + _input.Vertical * transform.forward;
                Quaternion quaternion = Quaternion.LookRotation(forward, Vector3.up);
                quaternion = Quaternion.Slerp(Model.transform.rotation, quaternion, Time.deltaTime * RotationSpeed);
                Model.transform.rotation = quaternion;
            }
        }
    }

}