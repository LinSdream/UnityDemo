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
        [HideInInspector] public bool IsCrouch = false;
        private void Update()
        {
            Horizontal = Input.GetAxisRaw(HorizontalRaw);
            Vertical = Input.GetAxisRaw(VerticalRaw);

            IsCrouch = Input.GetButton(CrouchButton);
        }
    }
}