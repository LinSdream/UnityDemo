﻿using LS.Cameras;
using LS.Common;
using LS.Common.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Souls
{

    #region Struct Target
    public struct TargetObj
    {
        public GameObject Target;
        public ActorManager AM;
    }
    #endregion

    public class CameraController : FreeLookCameraController
    {
        #region Fields

        [Header("Other Settings")]
        public LayerMask CheckMask;
        public Image AimPointImg;
        [Tooltip("自动解锁距离：目标对象与自身的距离平方")] public float SqrDistanceTargetToSelf = 100f;

        public bool LockState = false;

        /// <summary>平滑过渡的阻尼值</summary>
        Vector3 _dampVec;
        /// <summary> 未锁定状态到锁定状态之间的镜头旋转插值是否完成 </summary>
        [SerializeField] bool _cameraRotateSuccess = false;
        /// <summary> 锁定对象</summary>
        [SerializeField] TargetObj _lockTarget = new TargetObj();
        /// <summary> 锁定的目标对象 </summary>
        public GameObject LockTarget => _lockTarget.Target;

        #endregion

        #region Override Methods

        protected override void Awake()
        {
            base.Awake();

            AimPointImg.enabled = false;
            LockState = false;

            transform.position = TargetModel.position;
        }

        protected override void OnFixedUpdate()
        {
            if (_lockTarget.Target != null)
            {
                if ((_lockTarget.Target.transform.position - transform.position).sqrMagnitude > SqrDistanceTargetToSelf)
                    RelesaseLockOn();
                if (_lockTarget.AM != null)
                    if (_lockTarget.AM.SM.CharacterState.IsDie)
                    {
                        RelesaseLockOn();
                    }

            }
        }

        protected override void Rotate()
        {
            if (_lockTarget.Target == null)
                base.Rotate();
            else
            {
                if (true)
                {
                    Vector3 forward = _lockTarget.Target.transform.position - TargetModel.position;
                    forward = Vector3.Scale(forward, new Vector3(1, 0, 1));
                    transform.forward = forward;
                    ////计算方向向量
                    //Vector3 tmpForward = _lockTarget.transform.position - TargetModel.position;
                    //tmpForward.y = 0;
                    //transform.forward = tmpForward;//父级Player的方向指向目标物体
                }
                CameraPivot.LookAt(_lockTarget.Target.transform);
                AimPointImg.rectTransform.position = Camera.main.WorldToScreenPoint(_lockTarget.Target.transform.position);
            }
        }

        #endregion

        #region Private Methods

        /// <summary> 锁定对象 </summary>
        void Lock()
        {
            ///TODO: 从自由视角切换到锁定视角，由于是直接更改Rotation，会眩晕，需要做插值
            //try to lock
            Vector3 modelOrigin1 = TargetModel.transform.position;
            Vector3 modelOrigin2 = modelOrigin1 + new Vector3(0, 1, 0);
            Vector3 boxCenter = modelOrigin2 + TargetModel.transform.forward * 5f;
            Debug.DrawLine(boxCenter, Vector3.up, Color.red, 100);
            ///TODO:不应该用box来检测，用球？还是扇形，需要实际测试比较一下
            var colliders = Physics.OverlapBox(boxCenter, new Vector3(8f, 1f, 5f), TargetModel.transform.rotation, CheckMask);
            //var colliders = Physics.OverlapSphere(_model.position, 5f, CheckMask);
            if (colliders.Length == 0)
            {
                RelesaseLockOn();
                return;
            }
            _lockTarget.Target = SharedMethods.CalculateNearestCollider(TargetModel, colliders).gameObject;
            _lockTarget.AM = _lockTarget.Target.GetComponent<ActorManager>();
            AimPointImg.enabled = true;
            LockState = true;
            Vector3 tmpForward = _lockTarget.Target.transform.position - TargetModel.transform.position;
            //StartCoroutine(WaitForCameraRotate());
        }

        #endregion

        /// <summary> Input System 控制锁定 </summary>
        public void CameraLockOn()
        {
            if (_lockTarget.Target != null)
            {
                RelesaseLockOn();
                return;
            }
            Lock();
        }

        /// <summary> 解除锁定</summary>
        public void RelesaseLockOn()
        {
            AimPointImg.enabled = false;
            _lockTarget.Target = null;
            _lockTarget.AM = null;
            LockState = false;
        }
    }

    #region  Bug Code
    //public class CameraController : MonoBehaviour
    //{
    //    public UserInput _input;
    //    public GameObject ModelCamera;
    //    public LayerMask CheckMask;
    //    public Image AimPointImg;
    //    [Tooltip("自动解锁距离：目标对象与自身的距离平方")] public float SqrDistanceTargetToSelf = 100f;
    //    [Tooltip("平滑阻尼")] [Range(0, 1)] public float DampCoefficient;

    //    [Header("Inversion")]
    //    public bool HorizontalInversion = false;
    //    public bool VerticalInversion = false;

    //    [Header("CameraRotationSpeed")]
    //    public float HorizontalSpeed;
    //    public float VerticalSpeed;

    //    [Header("Limit Angle,x:min , y:max")]
    //    public Vector2 VerticalAngle;

    //    [HideInInspector] public bool LockState = false;

    //    Transform CameraHandle;
    //    Transform PlayerHandle;
    //    float _xRot = 0;
    //    Transform _model;
    //    /// <summary>平滑过渡的阻尼值</summary>
    //    Vector3 _dampVec;

    //    /// <summary> 未锁定状态到锁定状态之间的镜头旋转插值是否完成 </summary>
    //    [SerializeField] bool _cameraRotateSuccess = false;
    //    /// <summary> 锁定对象</summary>
    //    [SerializeField] GameObject _lockTarget;

    //    /// <summary> 锁定的目标对象 </summary>
    //    public GameObject LockTarget => _lockTarget;

    //    #region MonoBehaviour Callbacks
    //    private void Awake()
    //    {
    //        CameraHandle = transform.parent;
    //        PlayerHandle = transform.parent.parent;
    //        AimPointImg.enabled = false;
    //        LockState = false;

    //        _model = _input.GetComponent<PlayerController>().Model.transform;

    //    }

    //    // Start is called before the first frame update
    //    void Start()
    //    {
    //        if (HorizontalInversion)
    //            HorizontalSpeed = -HorizontalSpeed;
    //        else
    //            HorizontalSpeed = Mathf.Abs(HorizontalSpeed);
    //        if (VerticalInversion)
    //            VerticalSpeed = -VerticalSpeed;
    //        else
    //            VerticalSpeed = Mathf.Abs(VerticalSpeed);
    //    }

    //    private void FixedUpdate()
    //    {

    //        if(_lockTarget!=null)
    //            if ((_lockTarget.transform.position - transform.position).sqrMagnitude > SqrDistanceTargetToSelf)
    //                RelesaseLockOn();

    //        Rotate();
    //        //位置更新
    //        ModelCamera.transform.position = Vector3.SmoothDamp(ModelCamera.transform.position
    //            , transform.position, ref _dampVec, DampCoefficient);
    //    }

    //    #endregion

    //    #region Private Methods

    //    void Rotate()
    //    {
    //        if (_lockTarget == null)
    //        {
    //            Vector3 modelEuler = _model.eulerAngles;

    //            //计算垂直的镜头角度
    //            _xRot -= _input.CameraVertical * VerticalSpeed * Time.deltaTime;
    //            _xRot = Mathf.Clamp(_xRot, VerticalAngle.x, VerticalAngle.y);

    //            CameraHandle.localEulerAngles = new Vector3(_xRot, 0, 0);
    //            //垂直旋转
    //            PlayerHandle.Rotate(Vector3.up, _input.CameraHorizontal * HorizontalSpeed * Time.deltaTime);
    //            //VerticalAxis.Rotate(Vector3.right, _input.CameraVertical * VerticalSpeed * Time.deltaTime);

    //            //把模型的角度重新赋回去
    //            _model.eulerAngles = modelEuler;
    //        }
    //        else
    //        {
    //            if (true)
    //            {
    //                //计算方向向量
    //                Vector3 tmpForward = _lockTarget.transform.position - _model.transform.position;
    //                tmpForward.y = 0;
    //                PlayerHandle.forward = tmpForward;//父级Player的方向指向目标物体
    //            }
    //            CameraHandle.LookAt(_lockTarget.transform);
    //            AimPointImg.rectTransform.position = Camera.main.WorldToScreenPoint(_lockTarget.transform.position);

    //        }

    //        //水平旋转
    //        ModelCamera.transform.LookAt(CameraHandle);
    //        //Camera.transform.eulerAngles = transform.eulerAngles;
    //    }

    //    /// <summary> 解除锁定</summary>
    //    void RelesaseLockOn()
    //    {
    //        AimPointImg.enabled = false;
    //        _lockTarget = null;
    //        LockState = false;
    //    }

    //    /// <summary> 锁定对象 </summary>
    //    void Lock()
    //    {
    //        ///TODO: 从自由视角切换到锁定视角，由于是直接更改Rotation，会眩晕，需要做插值
    //        //try to lock
    //        Vector3 modelOrigin1 = _model.transform.position;
    //        Vector3 modelOrigin2 = modelOrigin1 + new Vector3(0, 1, 0);
    //        Vector3 boxCenter = modelOrigin2 + _model.transform.forward * 5f;

    //        ///TODO:不应该用box来检测，用球？还是扇形，需要实际测试比较一下
    //        var colliders = Physics.OverlapBox(boxCenter, new Vector3(8f, 1f, 5f), _model.transform.rotation, CheckMask);
    //        //var colliders = Physics.OverlapSphere(_model.position, 5f, CheckMask);
    //        if (colliders.Length == 0)
    //        {
    //            RelesaseLockOn();
    //            return;
    //        }
    //        _lockTarget = SharedMethods.CalculateNearestCollider(_model, colliders).gameObject;
    //        AimPointImg.enabled = true;
    //        LockState = true;
    //        Vector3 tmpForward = _lockTarget.transform.position - _model.transform.position;
    //        //StartCoroutine(WaitForCameraRotate());
    //    }
    //    #endregion


    //    #region Public Methods
    //    /// <summary>
    //    /// 水平垂直镜头反转
    //    /// </summary>
    //    public void Inversion(bool horizontal, bool vertical)
    //    {
    //        if (horizontal)
    //            HorizontalSpeed = -HorizontalSpeed;
    //        else
    //            HorizontalSpeed = Mathf.Abs(HorizontalSpeed);
    //        if (vertical)
    //            VerticalSpeed = -VerticalSpeed;
    //        else
    //            VerticalSpeed = Mathf.Abs(VerticalSpeed);
    //    }

    //    /// <summary> Input System 控制锁定 </summary>
    //    public void CameraLockOn()
    //    {
    //        if (_lockTarget != null)
    //        {
    //            RelesaseLockOn();
    //            return;
    //        }
    //        Lock();
    //    }

    //    #endregion

    //    #region Coroutine

    //    ///TODO:大概率是要把所有角度的运算更改为利用四元数进行运算，该模块需要重新梳理
    //    IEnumerator WaitForCameraRotate()
    //    {
    //        _cameraRotateSuccess = false;
    //        var targetRotation = Quaternion.FromToRotation(PlayerHandle.position, _lockTarget.transform.position);
    //        Debug.Log(Quaternion.Angle(PlayerHandle.rotation, targetRotation));
    //        while (Quaternion.Angle(PlayerHandle.rotation, targetRotation) > 0.1f)
    //        {
    //            PlayerHandle.rotation = Quaternion.Slerp(PlayerHandle.rotation, targetRotation, Time.deltaTime);
    //            Debug.Log(Quaternion.Angle(PlayerHandle.rotation, targetRotation));
    //        }
    //        _cameraRotateSuccess = true;
    //        yield return null;
    //    }
    //    #endregion
    //}
    #endregion
}