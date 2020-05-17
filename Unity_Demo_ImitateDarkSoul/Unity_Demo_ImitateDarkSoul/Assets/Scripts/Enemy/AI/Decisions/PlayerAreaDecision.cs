
using LS.Test.AI;
using LS.Test.AI.Decisions;
using UnityEngine;

namespace Souls.AI
{

    [CreateAssetMenu(menuName = "Souls/EnemyAI/Decision/PlayerInArea")]
    public class PlayerAreaDecision : Decision
    {
        [Tooltip("向量表示范围，平方表示 0-10,10-50")]
        public Vector2 Offset = new Vector2(100f, 2500f);

        public override bool Decide(FSMBase controller)
        {
            if (controller.TargetGameObject == null)
                return false;
            var fsm = controller as BlackKnightFSM;

            float distance = (fsm.TargetGameObject.transform.position - fsm.transform.position).sqrMagnitude;

            //如果在范围内，返回真
            if (distance <= Offset.x)
                return true;
            else if (distance > Offset.x && distance <= Offset.y)
                return true;
            else
                return false;
        }
    }
}
