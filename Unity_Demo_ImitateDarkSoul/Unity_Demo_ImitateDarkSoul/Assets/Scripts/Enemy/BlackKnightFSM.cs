using LS.Common;
using LS.Test.AI;
using LS.Test.AI.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Souls
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class BlackKnightFSM : FSMBase
    {
        public State Show;
        [HideInInspector] public NavMeshAgent AI;
        [HideInInspector] public BaseController Controller;

        NavMeshTriangulation _navMeshData;
        #region Callbacks
        private void Awake()
        {
            AI = GetComponent<NavMeshAgent>();
            Controller = GetComponent<BaseController>();
            AI.speed = Controller.WalkSpeed;
            AI.angularSpeed = Controller.RotationSpeed;
        }

        protected override void OnStart()
        {
            TargetGameObject = GameObject.Find("Player");
        }

        protected override void OnUpdate()
        {
            Show = CurrentState;
            Debug.Log(CurrentState == null ? 1 : 0);
        }

        #endregion
        public void SetForwardAnimator(float value)
        {
            Controller.Anim.SetFloat("Forward", value);
        }


        /// <summary>判断玩家是否在范围内 </summary>
        /// <param name="sqrDistance">距离的平方</param>
        /// <param name="halfAngle">半角</param>
        public bool IsInArea(float sqrDistance, float halfAngle)
        {
            return SharedMethods.IsInArea(transform, TargetGameObject.transform, sqrDistance, halfAngle);
        }

        /// <summary>获取navmesh上随机的一个点 </summary>
        public Vector3 GetRandomLocaion()
        {

            //每三个相邻顶点构成一个三角网格，数组减去3防止越界
            int trianglePoint = Random.Range(0, _navMeshData.indices.Length - 3);

            //取任意一个三角网格的三点的中点
            Vector3 point = (_navMeshData.vertices[_navMeshData.indices[trianglePoint]] +
                _navMeshData.vertices[_navMeshData.indices[trianglePoint + 1]]
                + _navMeshData.vertices[_navMeshData.indices[trianglePoint + 2]]) / 3;

            return point;
        }
    }

}