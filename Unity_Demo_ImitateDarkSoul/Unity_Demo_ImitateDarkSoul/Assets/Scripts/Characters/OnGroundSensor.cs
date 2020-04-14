using LS.Common.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class OnGroundSensor : MonoBehaviour
    {
        public CapsuleCollider ModelCollider;
        public LayerMask Mask;
        public float SensorGroundOffset = .1f;

        Vector3 _pointDown;
        Vector3 _pointUp;
        BaseController _controller;
        float _radius;

        #region MonoBehaviour Callbacks
        private void Awake()
        {
            _radius = (ModelCollider.radius - SensorGroundOffset);
            _controller = ModelCollider.GetComponent<BaseController>();
        }

        private void FixedUpdate()
        {
            if (gameObject.CompareTag("Enemy"))
            {
                _controller.IsGrounded = true;
                return;
            }

            _pointDown = transform.position + transform.up * (_radius - SensorGroundOffset);
            _pointUp = transform.position + transform.up * (ModelCollider.height - SensorGroundOffset) - transform.up * _radius;

            _controller.IsGrounded = Physics.CheckCapsule(_pointDown, _pointUp, _radius, Mask);

        }


        private void OnDrawGizmos()
        {
            _pointDown = transform.position + transform.up * (_radius - SensorGroundOffset);
            _pointUp = transform.position + transform.up * (ModelCollider.height - SensorGroundOffset) - transform.up * _radius;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_pointDown, _pointUp);
        }
        #endregion
    }

}