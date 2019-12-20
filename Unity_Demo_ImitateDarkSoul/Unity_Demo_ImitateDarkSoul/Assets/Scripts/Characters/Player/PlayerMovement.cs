using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlayerMovement : MonoBehaviour
    {
        #region Public Fileds
        public float TurnSpeed;
        public float MoveSpeed;
        #endregion

        #region Private Fields
        Animator _anim;
        Rigidbody _body;
        CharacterController _characterCol;
        #endregion

        #region MonoBehaviour
        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _body = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            
            
        }

        private void FixedUpdate()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            Rotation(h, v);
            Move(h, v);
        }
        #endregion

        public void Rotation(float h, float v)
        {
            if (h == 0 || v == 0)
            {
                return;
            }
            Quaternion rotation = Quaternion.LookRotation(new Vector3(h, 0, v),Vector3.up);
            Quaternion targetRotation = Quaternion.Lerp(_body.rotation, rotation, Time.deltaTime * TurnSpeed);
            _body.MoveRotation(targetRotation);
        }

        public void Move(float h,float v)
        {
            if (Mathf.Abs(h) < .2f && Mathf.Abs(v) < .2f)
            {
                _anim.SetBool("Running", false);
            }
            else
            {
                _anim.SetBool("Running", true);
                _body.MovePosition(_body.position + new Vector3(h, 0, v) * Time.deltaTime * MoveSpeed);
            }

        }
    }

}