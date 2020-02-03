using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.TestCamera
{
    public class CameraController : MonoBehaviour
    {

        public Transform CameraPivot;
        public Transform Model;
        public float TurnSpeed = 1.5f;

        PlayerInput _input;
        Transform _camera;

        float _lookAngle;
        Quaternion _transformTargetRot;
        private void Awake()
        {
            _input = GetComponent<PlayerInput>();
            _camera = CameraPivot.GetChild(0);

            _transformTargetRot = transform.localRotation;
        }

        private void Update()
        {
            Rotation();
        }

        void Rotation()
        {

            var rotation = Model.rotation;

            _lookAngle += _input.CameraHorizontal * TurnSpeed;
            _transformTargetRot = Quaternion.Euler(0f, _lookAngle, 0f);
            transform.localRotation = _transformTargetRot;

            ///TODO: 待定：旋转transform的时候，将Model的正方向与transfrom的正方向进行校正
            Model.rotation = rotation;
        }

    }
}