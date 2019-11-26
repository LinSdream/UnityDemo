using UnityEngine;

namespace Game
{
    #region Class Node
    public class Node
    {
        public Vector3 Position;
        public int VertexIndex = -1;

        public Node(Vector3 pos)
        {
            Position = pos;
        }
    }
    #endregion

    /// <summary>
    /// A   B   C
    /// D   E   F
    /// G   H   I
    /// 控制点B、D
    /// </summary>
    public class ControlNode : Node
    {
        public bool Active;
        public Node AboveNode;
        public Node RightNode;

        /// <param name="pos">E点，方块的中点</param>
        /// <param name="active">该方块是否激活</param>

        public ControlNode(Vector3 pos, bool active,float squareSize):base(pos)
        {
            Active = active;
            AboveNode = new Node(Position + Vector3.forward * squareSize / 2);
            RightNode = new Node(Position + Vector3.right * squareSize / 2);
        }
    }

}