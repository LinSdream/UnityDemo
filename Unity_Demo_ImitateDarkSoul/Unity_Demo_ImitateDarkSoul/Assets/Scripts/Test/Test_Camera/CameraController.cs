using Souls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.TestCamera
{
    public class CameraController : MonoBehaviour
    {

        public enum UpdateType
        {
            /// <summary>Update in FixedUpdate </summary>
            FIXEDUPDATE,
            /// <summary>Update in LateUpdate </summary>
            LATEUPDATE,
            /// <summary>Update by user call </summary>
            MANUALUPDATE,
        };

        public enum TargetMoveType
        {
            NONE,
            LERP,
            SMOOTHDAMP
        }

        [Header("Model")]
        public UserInput InputModel;
        public Transform CameraPivot;
        public Transform TargetModel;

        [Header("Camera Settings")]
        public UpdateType CameraUpdateType = UpdateType.FIXEDUPDATE;
        public TargetMoveType TargetModelUpdateType = TargetMoveType.LERP;
        [Range(0f, 10f)] public float TurnSpeed = 1.5f;
        public float MoveSpeed = 1f;
        public float TurnSmoothing = 0f;
        [Tooltip("Limit the angle of the y-axis ( min , max )")]
        public Vector2 LimitAngle = new Vector2(-45f, 75f);
        public bool VerticalAutoReturn = false;

        public Transform CameraObj { get; private set; }

        float _lookAngle;
        float _limitAngle;
        Vector3 _pivotEulers;
        Quaternion _pivotTargetRot;
        Quaternion _transformTargetRot;
        Vector3 _targetDampVelocity;
        private void Awake()
        {
            CameraObj = CameraPivot.GetChild(0);

            _pivotEulers = CameraPivot.transform.eulerAngles;

            _pivotTargetRot = CameraPivot.transform.localRotation;
            _transformTargetRot = transform.localRotation;
        }

        private void Update()
        {
            Rotation();
        }

        private void FixedUpdate()
        {
            if (TargetModel == null)
                return;
            if (CameraUpdateType == UpdateType.FIXEDUPDATE)
                FollowTarget(Time.deltaTime);
        }

        private void LateUpdate()
        {
            if (TargetModel == null)
                return;
            if (CameraUpdateType == UpdateType.LATEUPDATE)
                FollowTarget(Time.deltaTime);
        }

        public void ManualUpdate()
        {
            if (TargetModel == null)
                return;
            if (CameraUpdateType == UpdateType.MANUALUPDATE)
                FollowTarget(Time.deltaTime);
        }

        void Rotation()
        {

            //var rotation = TargetModel.rotation;

            _lookAngle += InputModel.CameraHorizontal * TurnSpeed;
            _transformTargetRot = Quaternion.Euler(0f, _lookAngle, 0f);

            if (VerticalAutoReturn)
                _limitAngle = InputModel.CameraVertical > 0 ? Mathf.Lerp(0, LimitAngle.x, InputModel.CameraVertical) : Mathf.Lerp(0, LimitAngle.y, -InputModel.CameraVertical);
            else
            {
                _limitAngle -= InputModel.CameraVertical * TurnSpeed;
                _limitAngle = Mathf.Clamp(_limitAngle, LimitAngle.x, LimitAngle.y);
            }

            _pivotTargetRot = Quaternion.Euler(_limitAngle, _pivotEulers.y, _pivotEulers.z);

            if (TurnSmoothing > 0)
            {
                CameraPivot.localRotation = Quaternion.Slerp(CameraPivot.localRotation, _pivotTargetRot, TurnSmoothing * Time.deltaTime);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, _transformTargetRot, TurnSmoothing * Time.deltaTime);

            }
            else
            {
                CameraPivot.localRotation = _pivotTargetRot;
                transform.localRotation = _transformTargetRot;
            }

            /////TODO:镜头的正方向是Player的正方向；在不动镜头的时候，左右移动，镜头要平滑划过，也就是镜头也要旋转
            /////也就是说在不同镜头的时候按下x轴位移，Player的运动轨迹是圆形，在镜头与Player的控制中同样都是x轴位移的时候，也呈现圆形运动
            //TargetModel.rotation = rotation;
        }

        void FollowTarget(float deltaTime)
        {
            if (TargetModel == null)
                return;
            switch(TargetModelUpdateType)
            {
                case TargetMoveType.NONE:
                    transform.position = TargetModel.position;
                    break;
                case TargetMoveType.LERP:
                default:
                    transform.position = Vector3.Lerp(transform.position, TargetModel.position, deltaTime * MoveSpeed);
                    break;
                case TargetMoveType.SMOOTHDAMP:
                    transform.position = Vector3.SmoothDamp(transform.position, TargetModel.position, ref _targetDampVelocity, deltaTime);
                    break;
            }
        }

    }
}