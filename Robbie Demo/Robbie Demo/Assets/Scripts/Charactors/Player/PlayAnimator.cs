using LS.Test.Others;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlayAnimator : MonoBehaviour
    {
        [Tooltip("动画脚本")]   public Animator Anim;
        [Tooltip("角色基本操作控制")] public PlayerMovement Movement;

        public string[] StepAudios;
        public string[] CrouchAudios;
        public AudioSource StepAudioSource;
        public AudioSource CrouchAudioSource;

        private void Update()
        {
            Anim.SetFloat(Define.PlayerDefine.SpeedID, Mathf.Abs(Movement.XVelocity));
            Anim.SetBool(Define.PlayerDefine.GroundID, Movement.State.IsOnGround);
            Anim.SetBool(Define.PlayerDefine.CrouchID, Movement.State.IsCrouch);
            Anim.SetBool(Define.PlayerDefine.HangingID, Movement.State.IsHanging);
            Anim.SetFloat(Define.PlayerDefine.FallID, Movement.Body.velocity.y);
        }

        public void StepAudio()
        {
            var index = Random.Range(0, StepAudios.Length);
            StepAudioSource.clip = AudioManager.Instance.GetAudioClip(StepAudios[index]);
            StepAudioSource.Play();
        }

        public void CrouchStepAudio()
        {
            var index = Random.Range(0, CrouchAudios.Length);
            CrouchAudioSource.clip = AudioManager.Instance.GetAudioClip(CrouchAudios[index]);
            CrouchAudioSource.Play();
        }

    }

}