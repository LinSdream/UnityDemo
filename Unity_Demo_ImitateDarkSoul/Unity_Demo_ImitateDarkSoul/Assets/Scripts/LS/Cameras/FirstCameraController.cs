using LS.CustomInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS.Cameras
{

    public class FirstCameraController : MonoBehaviour
    {
        [Header("Model")]
        [Tooltip("CameraInput")] public InputBase InputModel;
        [Tooltip("Character")] public Transform CharacterModel;

        [Header("Settings")]
        [Tooltip("Limit the angle of the y-axis ( min , max )")] public Vector2 LimitAngle = new Vector2(-45f, 75f);
        public bool Smooth;
        public float SmoothTime = 5f;
        public float XSensitivity = 2f;
        public float YSensitivity = 2f;

        Quaternion _characterTargetRot;
        Quaternion _cameraTargetRot;

        private void Start()
        {
            _characterTargetRot = CharacterModel.localRotation;
            _cameraTargetRot = transform.localRotation;
        }

        private void Update()
        {
            LookRotation();
        }

        public void LookRotation()
        {

            float yRot = InputModel.CameraHorizontal * XSensitivity;
            float xRot = InputModel.CameraVertical * YSensitivity;

            _characterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            _cameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

            _cameraTargetRot = ClampRotationAroundXAxis(_cameraTargetRot);

            if (Smooth)
            {
                CharacterModel.localRotation = Quaternion.Slerp(CharacterModel.localRotation, _characterTargetRot,
                   SmoothTime * Time.deltaTime);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, _cameraTargetRot,
                    SmoothTime * Time.deltaTime);
            }
            else
            {
                CharacterModel.localRotation = _characterTargetRot;
                transform.localRotation = _cameraTargetRot;
            }
        }


        Quaternion ClampRotationAroundXAxis(Quaternion q)
        {

            //根据四元数的公式，这个运算得出来一个tan(angle)值
            //q.x=n* sin(angle/2) 在这里sin里面是一个弧度值
            //q.w=cos(angle/2)
            //q.x=sin(angle/2)/cos(angle/2)=tan(angle/2)
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            //q.x=tan(angle/2);
            //angle= 2*Atan(q.x)(弧度，要转角度)=2 * Mathf.Rad2Deg * Mathf.Atan (q.x);
            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

            angleX = Mathf.Clamp(angleX, LimitAngle.x, LimitAngle.y);
            //如果angle的值没有超过min，max，那转换前后的值是一样的。
            //如果angle的值小于min或者大于max了，那么angle的取值就发送了变化，这时候就又要重新转换下
            //这个就是上面那个angle公式的逆运算了
            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }
    }

}