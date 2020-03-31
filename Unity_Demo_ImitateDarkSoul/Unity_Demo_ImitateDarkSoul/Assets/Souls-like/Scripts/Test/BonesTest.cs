using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls.Test
{
    public class BonesTest : MonoBehaviour
    {
        public SkinnedMeshRenderer OriginMeshRenderer;
        public SkinnedMeshRenderer[] TargetMeshRenderers;

        private void Start()
        {
            foreach (var cell in TargetMeshRenderers)
            {
    

                cell.bones = OriginMeshRenderer.bones;
            }
            
        }
    }

}