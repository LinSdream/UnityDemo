using UnityEngine;

namespace Game
{

    /// <summary>
    /// 定义常量，字符串信息等
    /// </summary>
    public static class Define
    {
        public static class PlayerDefine
        {
            public const string SpeedStr = "speed";
            public static int SpeedID = Animator.StringToHash(SpeedStr);

            public const string IsOnGroundStr = "isOnGround";
            public static int GroundID = Animator.StringToHash(IsOnGroundStr);

            public const string IsHangingStr = "isHanging";
            public static int HangingID = Animator.StringToHash(IsHangingStr);

            public const string IsCrouchingStr = "isCrouching";
            public static int CrouchID = Animator.StringToHash(IsCrouchingStr);

            public const string FallStr = "verticalVelocity";
            public static int FallID = Animator.StringToHash(FallStr);

        }

    }
}