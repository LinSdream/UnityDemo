using LS.InputFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS.Cameras
{
    // This script is designed to be placed on the root object of a camera rig,
    // comprising 3 gameobjects, each parented to the next:

    // 	Camera Rig
    // 		Pivot
    // 			Camera
    public class FreeLookCameraController : AbstractTargetFollower
    {
        public enum TargetMoveType
        {
            NONE,
            LERP,
            SMOOTHDAMP
        }
        [Tooltip("Track the update mode of the TargetModel")] public TargetMoveType TargetModelUpdateType = TargetMoveType.LERP;
        [Tooltip("The speed of rig rotate")] [Range(0f, 10f)] public float TurnSpeed = 1.5f;
        [Tooltip("The speed of camera track to TargetModel")] public float MoveSpeed = 1f;
        [Tooltip("Limit the angle of the y-axis ( min , max )")] public Vector2 LimitAngle = new Vector2(-45f, 75f);
        [Tooltip("How much smoothing to apply to the turn input, to reduce mouse-turn jerkiness")]public float TurnSmoothing = 0f;
        [Tooltip("Set wether or not the vertical axis should auto return")]public bool VerticalAutoReturn = false;

        [Header("Model")]
        [Tooltip("Pivot,second level")] public Transform CameraPivot;
        [Tooltip("Camera input signal model")] public InputBase InputModel;

        /// <summary>Get Camera Transform </summary>
        public Transform CameraObj { get; protected set; }

        float _lookAngle;
        float _limitAngle;
        Vector3 _pivotEulers;
        Quaternion _pivotTargetRot;
        Quaternion _transformTargetRot;
        Vector3 _targetDampVelocity;

        protected virtual void Awake()
        {
            CameraObj = CameraPivot.GetChild(0);

            _pivotEulers = CameraPivot.transform.eulerAngles;

            _pivotTargetRot = CameraPivot.transform.localRotation;
            _transformTargetRot = transform.localRotation;
        }

        private void Update()
        {
            Rotate();
        }

       protected  virtual void Rotate()
        {
            _lookAngle += InputModel.CameraHorizontal * TurnSpeed;
            _transformTargetRot = Quaternion.Euler(0f, _lookAngle, 0f);

            if (VerticalAutoReturn)
                _limitAngle = InputModel.CameraVertical > 0 ? Mathf.Lerp(0, LimitAngle.x, InputModel.CameraVertical) 
                    : Mathf.Lerp(0, LimitAngle.y, -InputModel.CameraVertical);
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
        }

        protected override void FollowTarget(float deltaTime)
        {
            if (TargetModel == null)
                return;
            switch (TargetModelUpdateType)
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