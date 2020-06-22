using LS.InputFramework;
using UnityEngine;

namespace Game
{
    public class PlayerInput : InputBase
    {
        [Header("轴信号量")]
        [Tooltip("水平轴")] public string HorizontalRaw = "Horizontal";
        [Tooltip("垂直轴")] public string VerticalRaw = "Vertical";

        [Header("持续信号量，按下键后会持续输出信号")]
        [Tooltip("下蹲")] public string CrouchButton = "Crouch";
        [Tooltip("跳跃")] public string JumpButton = "Jump";

        /// <summary>
        /// 下蹲
        /// </summary>
        [HideInInspector] public bool IsCrouch = false;
        /// <summary>
        /// 单次按下跳跃键
        /// </summary>
        [HideInInspector] public bool JumpPressed;
        /// <summary>
        /// 持续按下跳跃
        /// </summary>
        [HideInInspector] public bool JumpHeld;

        private void Update()
        {
            Horizontal = Input.GetAxisRaw(HorizontalRaw);
            Vertical = Input.GetAxisRaw(VerticalRaw);

            IsCrouch = Input.GetButton(CrouchButton);

            JumpPressed = Input.GetButtonDown(JumpButton);
            JumpHeld = Input.GetButton(JumpButton);
        }
    }
}