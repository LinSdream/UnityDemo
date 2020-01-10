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
        float _radius;
        IsInGroundEventArgs _args = new IsInGroundEventArgs();

        #region MonoBehaviour Callbacks
        private void Awake()
        {
            _radius = (ModelCollider.radius - SensorGroundOffset);
        }

        private void FixedUpdate()
        {
            _pointDown = transform.position + transform.up * (_radius - SensorGroundOffset);
            _pointUp = transform.position + transform.up * (ModelCollider.height - SensorGroundOffset) - transform.up * _radius;

            _args.IsInGround = Physics.CheckCapsule(_pointDown, _pointUp, _radius, Mask);
            MessageCenter.Instance.SendMessage("IsInGround", gameObject, _args);

            //var colliders = Physics.OverlapCapsule(_pointDown, _pointUp,_radius, Mask);
            //if (Physics.CheckCapsule(_pointDown, _pointUp, _radius, Mask))
            //    MessageCenter.Instance.SendMessage("InGround");
            //else
            //    MessageCenter.Instance.SendMessage("NotInGround");
        }

        #endregion
    }

}