using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS.Helper.Characters
{

    public delegate void MovementAnimatorHandle(float h, float v);
    public delegate void InputHandle(ref float h, ref float v);
    public delegate void InputCustomActionHandle();

    [RequireComponent(typeof(CharacterController),typeof(Rigidbody))]
    public class ThirdPersonMovement : MonoBehaviour
    {
        #region Public Fields

        public float MoveSpeed = 10f;
        public float TurnSpeed = 100f;
        public float JumpPower = 12f;

        [HideInInspector] public event MovementAnimatorHandle MovementInGroundAnimEvent;
        [HideInInspector] public event MovementAnimatorHandle MovementJumpAnimEvent;
        [HideInInspector] public event MovementAnimatorHandle MovementDefaultEvent;

        [HideInInspector] public event InputCustomActionHandle BeforeInputAction;
        [HideInInspector] public event InputHandle CharacterInputBeforeMoveEvent;
        #endregion

        #region Private Fields
        Player _player;
        CharacterController _character;
        Rigidbody _body;
        float _capsuleHeight;
        Vector3 _capsuleCenter;
        bool _crouching;
        #endregion
        private void Awake()
        {
            _character = GetComponent<CharacterController>();
            _player = GetComponent<Player>();
        }

        private void Update()
        {
            BeforeInputAction?.Invoke();
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            CharacterInputBeforeMoveEvent?.Invoke(ref h, ref v);
            Move(h, v);
            Rotate(h, v);
        }

        void Move(float h,float v)
        {
            Vector3 dir = Vector3.zero;

            MovementDefaultEvent?.Invoke(h, v);

            //翻滚状态下，禁止移动
            if (_player.Status == PlayerStatus.Roll
                ||_player.Status==PlayerStatus.Drinking)
            {
                dir = new Vector3(h, 0, v).normalized;
                dir.y += Physics.gravity.y * Time.deltaTime;
                _character.Move(dir * Time.deltaTime);
                return;
            }

            if (_character.isGrounded)
            {
                dir = transform.TransformDirection(new Vector3(h, 0, v) * MoveSpeed);
                MovementInGroundAnimEvent?.Invoke(h, v);
            }
            else
            {
                dir = transform.TransformDirection(new Vector3(h, 0, v) * MoveSpeed);
                dir.y = JumpPower;
                MovementJumpAnimEvent?.Invoke(h, v);
            }
            dir.y += Physics.gravity.y * Time.deltaTime;
            _character.Move(dir * Time.deltaTime);
        }

        public void Rotate(float h,float v)
        {
            //float turnSpeed = Mathf.Lerp(StationaryTurnSpeed, MovingTurnSpeed, _forwardAmount);
            //transform.Rotate(0, _turnAmount * turnSpeed * Time.deltaTime, 0);
            Vector3 originEuler = transform.eulerAngles;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, h * 90, 0) + originEuler);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * TurnSpeed);
        }

    }

}