using LS.Common.Math;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Souls
{
    public class CameraController : MonoBehaviour
    {
        public UserInput _input;
        public GameObject ModelCamera;
        public LayerMask CheckMask;
        public Image AimPointImg;
        [Tooltip("平滑阻尼")] [Range(0, 1)] public float DampCoefficient;

        [Header("Inversion")]
        public bool HorizontalInversion = false;
        public bool VerticalInversion = false;

        [Header("CameraRotationSpeed")]
        public float HorizontalSpeed;
        public float VerticalSpeed;

        [Header("Limit Angle,x:min , y:max")]
        public Vector2 VerticalAngle;

        [HideInInspector] public bool LockState = false;

        Transform VerticalAxis;
        Transform HorizontalAxis;
        float _tmpEulerX = 0;
        Transform _model;
        Vector3 _dampVec;
        [SerializeField] GameObject _lockTarget;

        #region MonoBehaviour Callbacks
        private void Awake()
        {
            VerticalAxis = transform.parent;
            HorizontalAxis = transform.parent.parent;
            AimPointImg.enabled = false;
            LockState = false;

            _model = _input.GetComponent<PlayerController>().Model.transform;

        }

        // Start is called before the first frame update
        void Start()
        {
            if (HorizontalInversion)
                HorizontalSpeed = -HorizontalSpeed;
            else
                HorizontalSpeed = Mathf.Abs(HorizontalSpeed);
            if (VerticalInversion)
                VerticalSpeed = -VerticalSpeed;
            else
                VerticalSpeed = Mathf.Abs(VerticalSpeed);
        }

        private void FixedUpdate()
        {
            if (_lockTarget == null)
            {
                Vector3 modelEuler = _model.eulerAngles;

                //计算垂直的镜头角度
                _tmpEulerX -= _input.CameraVertical * VerticalSpeed * Time.deltaTime;
                _tmpEulerX = Mathf.Clamp(_tmpEulerX, VerticalAngle.x, VerticalAngle.y);

                VerticalAxis.localEulerAngles = new Vector3(_tmpEulerX, 0, 0);
                //垂直旋转
                HorizontalAxis.Rotate(Vector3.up, _input.CameraHorizontal * HorizontalSpeed * Time.deltaTime);
                //VerticalAxis.Rotate(Vector3.right, _input.CameraVertical * VerticalSpeed * Time.deltaTime);

                ///TODO:为了方便理解，将镜头旋转分成了两个向量来计算，之后为了性能优化，合并在一起，不在使用Model来控制水平旋转
                //把模型的角度重新赋回去
                _model.eulerAngles = modelEuler;
            }
            else
            {
                Vector3 tmpForward = _lockTarget.transform.position - _model.transform.position;
                tmpForward.y = 0;
                VerticalAxis.transform.forward = tmpForward;
                HorizontalAxis.forward = tmpForward;
                AimPointImg.transform.position = Camera.main.WorldToScreenPoint(_lockTarget.transform.position);
            }

            //水平旋转
            ModelCamera.transform.LookAt(VerticalAxis);
            //Camera.transform.eulerAngles = transform.eulerAngles;

            //位置更新
            ModelCamera.transform.position = Vector3.SmoothDamp(ModelCamera.transform.position, transform.position, ref _dampVec, DampCoefficient);
        }

        #endregion

        #region Private Methods

        /// <summary> 解除锁定</summary>
        public void RelesaseLockOn()
        {
            AimPointImg.enabled = false;
            _lockTarget = null;
            LockState = false;
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// 水平垂直镜头反转
        /// </summary>
        public void Inversion(bool horizontal, bool vertical)
        {
            if (horizontal)
                HorizontalSpeed = -HorizontalSpeed;
            else
                HorizontalSpeed = Mathf.Abs(HorizontalSpeed);
            if (vertical)
                VerticalSpeed = -VerticalSpeed;
            else
                VerticalSpeed = Mathf.Abs(VerticalSpeed);
        }

        public void CameraLockOn()
        {

            if (_lockTarget != null)
            {
                RelesaseLockOn();
                return;
            }

            ///TODO: 从自由视角切换到锁定视角，由于是直接更改Rotation，会眩晕，需要做插值
            //try to lock
            Vector3 modelOrigin1 = _model.transform.position;
            Vector3 modelOrigin2 = modelOrigin1 + new Vector3(0, 1, 0);
            Vector3 boxCenter = modelOrigin2 + _model.transform.forward * 5f;

            ///TODO:不应该用box来检测，用球？还是扇形，需要实际测试比较一下
            var colliders = Physics.OverlapBox(boxCenter, new Vector3(8f, 1f, 5f), _model.transform.rotation, CheckMask);
            //var colliders = Physics.OverlapSphere(_model.position, 5f, CheckMask);
            if (colliders.Length == 0)
            {
                RelesaseLockOn();
                return;
            }
            _lockTarget = SharedMethods.CalculateNearestCollider(_model, colliders).gameObject;
            AimPointImg.enabled = true;
            LockState = true;
        }

        #endregion
    }

}