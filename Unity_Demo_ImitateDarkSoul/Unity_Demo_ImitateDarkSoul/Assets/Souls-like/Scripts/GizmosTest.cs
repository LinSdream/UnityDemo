using LS.Common.Math;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{

    public class GizmosTest : MonoBehaviour
    {

        public PlayerController PlayerCol;
        public Vector3 BoxSize;

        public int CompareTo(GizmosTest other)
        {
            throw new NotImplementedException();
        }

        private void OnDrawGizmosSelected()
        {
            Vector3 modelOrigin1 = PlayerCol.Model.transform.position;
            Vector3 modelOrigin2 = modelOrigin1 + new Vector3(0, 1, 0);
            Vector3 boxCenter = modelOrigin2 + PlayerCol.Model.transform.forward * 5f;
            Gizmos.DrawCube(boxCenter, BoxSize);
        }
    }

}