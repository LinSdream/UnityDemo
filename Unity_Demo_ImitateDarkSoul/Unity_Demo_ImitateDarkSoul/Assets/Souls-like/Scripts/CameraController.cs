using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class CameraController : MonoBehaviour
    {
        public UserInput _input;
        public GameObject Camera;
        [Tooltip("平滑阻尼")][Range(0, 1)] public float DampCoefficient;

        [Header("Inversion")]
        public bool HorizontalInversion = false;
        public bool VerticalInversion = false;

        [Header("CameraRotationSpeed")]
        public float HorizontalSpeed;
        public float VerticalSpeed;

        [Header("Limit Angle,x:min , y:max")]
        public Vector2 VerticalAngle;

        Transform VerticalAxis;
        Transform HorizontalAxis;
        float _tmpEulerX = 0;
        Transform _model;
        Vector3 _dampVec;

        private void Awake()
        {
            VerticalAxis = transform.parent;
            HorizontalAxis = transform.parent.parent;

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

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {

            Vector3 modelEuler = _model.eulerAngles;

            if (Input.GetKeyDown(KeyCode.Alpha1))
                _input.LockCursor = !_input.LockCursor;

            _tmpEulerX -= _input.CameraVertical * VerticalSpeed * Time.deltaTime;
            _tmpEulerX = Mathf.Clamp(_tmpEulerX, VerticalAngle.x, VerticalAngle.y);

            VerticalAxis.localEulerAngles = new Vector3(_tmpEulerX, 0, 0);
            HorizontalAxis.Rotate(Vector3.up, _input.CameraHorizontal * HorizontalSpeed * Time.deltaTime);
            //VerticalAxis.Rotate(Vector3.right, _input.CameraVertical * VerticalSpeed * Time.deltaTime);

            _model.eulerAngles = modelEuler;

            Camera.transform.position = transform.position;
            //Camera.transform.eulerAngles = transform.eulerAngles;
            Camera.transform.LookAt(VerticalAxis);
            Camera.transform.position = Vector3.SmoothDamp(Camera.transform.position, transform.position, ref _dampVec, DampCoefficient);
        }

        public void Inversion(bool horizontal,bool vertical)
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
    }

}