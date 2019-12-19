using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS.Cameras
{
    /// <summary>
    /// y轴与目标对象对等的固定摄像头
    /// </summary>
    public class FixedCameraCol : MonoBehaviour
    {
        
        /// <summary>目标对象</summary>
        [Tooltip("摄像头跟踪的目标对象")]
        public Transform TargetObj;
        /// <summary>  摄像头与人物的距离，z轴位移</summary>
        [Tooltip("人机水平位移，z轴")]
        public float DistanceForward = 10f;
        /// <summary> 摄像头与人物的高度，y轴距离</summary>
        [Tooltip("人机高度位移，y轴")]
        public float DistanceUp = 2;

        /// <summary>目标点</summary>
        Vector3 _target;
        private void Start()
        {
            _target = TargetObj.position - transform.position;
        }
        private void LateUpdate()
        {
            Rotate();
            _target = TargetObj.position + Vector3.up * DistanceUp - TargetObj.forward * DistanceForward;//人物的距离+新的

            transform.position = _target;
        }

        void Rotate()
        {
            Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, TargetObj.eulerAngles.y, transform.eulerAngles.z);
            transform.rotation = targetRotation;
        }
    }

}