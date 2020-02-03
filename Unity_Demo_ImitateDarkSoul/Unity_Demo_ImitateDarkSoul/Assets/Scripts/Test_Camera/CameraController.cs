using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.TestCamera
{
    public class CameraController : MonoBehaviour
    {

        public GameObject CameraPivot;

        PlayerInput _input;

        private void Awake()
        {
            _input = GetComponent<PlayerInput>();
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            //x轴旋转，无论何时，镜头的正方向就是Player的正方向
        }
    }
}