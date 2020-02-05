using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS.Cameras
{
    public abstract class AbstractTargetFollower : MonoBehaviour
    {
        /// <summary>Camera Update Type </summary>
        public enum UpdateType
        {
            /// <summary>Update in FixedUpdate </summary>
            FIXEDUPDATE,
            /// <summary>Update in LateUpdate </summary>
            LATEUPDATE,
            /// <summary>Update by user call </summary>
            MANUALUPDATE,
        };

        ///<summary> 目标对象 </summary>
        public Transform TargetModel;
        

        [Header("Camera Settings")]
        public UpdateType CameraUpdateType = UpdateType.FIXEDUPDATE;

        private void FixedUpdate()
        {
            OnFixedUpdate();

            if (TargetModel == null)
                return;
            if (CameraUpdateType == UpdateType.FIXEDUPDATE)
                FollowTarget(Time.deltaTime);

        }

        private void LateUpdate()
        {
            OnLateUpdate();

            if (TargetModel == null)
                return;
            if (CameraUpdateType == UpdateType.LATEUPDATE)
                FollowTarget(Time.deltaTime);
        }

        public void ManualUpdate()
        {
            OnManualUpdate();

            if (TargetModel == null)
                return;
            if (CameraUpdateType == UpdateType.MANUALUPDATE)
                FollowTarget(Time.deltaTime);
        }

        /// <summary> Camera跟踪目标方式</summary>
        protected abstract void FollowTarget(float deltaTime);

        /// <summary>FixedUpdate回调</summary>
        protected virtual void OnFixedUpdate() { }

        /// <summary> LateUpate回调 </summary>
        protected virtual void OnLateUpdate() { }

        /// <summary> ManualUpdate回调 </summary>
        protected virtual void OnManualUpdate() { }

    }

}