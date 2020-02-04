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

            ///TODO:镜头的正方向是Player的正方向；在不动镜头的时候，左右移动，镜头要平滑划过，也就是镜头也要旋转
            ///也就是说在不同镜头的时候按下x轴位移，Player的运动轨迹是圆形，在镜头与Player的控制中同样都是x轴位移的时候，也呈现圆形运动
            Model.rotation = rotation;
        }

    }
}