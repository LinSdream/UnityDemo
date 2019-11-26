using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{

    public class Square
    {
        public ControlNode TopLeft, TopRight, BottomLeft, BottomRight;
        public Node CenterTop, CenterBottom, CenterRight, CenterLeft;

        public Square(ControlNode bottomLeft, ControlNode bottomRight,ControlNode topLeft, ControlNode topRight)
        {
            TopLeft = topLeft;
            TopRight = topRight;
            BottomLeft = bottomLeft;
            BottomRight = bottomRight;

            CenterTop = TopLeft.RightNode;
            CenterRight = BottomRight.AboveNode;
            CenterBottom = BottomLeft.RightNode;
            CenterLeft = TopLeft.AboveNode;
        }
    }

    public class SquareGird
    {
        public Square[,] Squares;

        public SquareGird(int [,] map,float squareSize)
        {
            int nodeCountX = map.GetLength(0);//x轴node数目
            int nodeCountY = map.GetLength(1);//y轴node数量

            float mapWidth = nodeCountX * squareSize;//map宽度
            float mapHeight = nodeCountY * squareSize;//map高度

            ControlNode[,] controlNode = new ControlNode[nodeCountX, nodeCountY];
            for (int x = 0; x < nodeCountX; x++)
            {
                for (int y = 0; y < nodeCountY; y++)
                {
                    //计算控制点的中点
                    Vector3 pos = new Vector3(-mapWidth / 2 + x * squareSize + squareSize / 2, 0, -mapHeight / 2 + y * squareSize + squareSize / 2);
                    controlNode[x, y] = new ControlNode(pos, map[x, y] == 1, squareSize);
                }
            }
            Squares = new Square[nodeCountX - 1, nodeCountY - 1];
            for(int x = 0; x < nodeCountX - 1; x++)
            {
                for(int y = 0; y < nodeCountY - 1; y++)
                {
                    Squares[x, y] = new Square(controlNode[x, y], controlNode[x, y + 1], controlNode[x + 1, y], controlNode[x + 1, y + 1]);
                }
            }
        }
    }

    public class MeshGenerator : MonoBehaviour
    {
        public SquareGird mSquareGird;

        /// <summary>
        /// 基于map生成网格
        /// </summary>
        /// <param name="squareSize">网格大小</param>
        public void GeneratorMesh(int [,]map,float squareSize)
        {
            mSquareGird = new SquareGird(map, squareSize);
            
        }

        private void OnDrawGizmos()
        {
            if (mSquareGird != null)
            {
                int squareX = mSquareGird.Squares.GetLength(0);
                int squareY = mSquareGird.Squares.GetLength(1);

                for(int x = 0; x < squareX; x++)
                {
                    for(int y = 0; y < squareY; y++)
                    {
                        Square square = mSquareGird.Squares[x, y];
                        Gizmos.color = (square.TopLeft.Active) ? Color.black : Color.white;
                        Gizmos.DrawCube(square.TopLeft.Position, Vector3.one * 0.4f);

                        Gizmos.color = (square.TopRight.Active) ? Color.black : Color.white;
                        Gizmos.DrawCube(square.TopRight.Position, Vector3.one * 0.4f);

                        Gizmos.color = (square.BottomLeft.Active) ? Color.black : Color.white;
                        Gizmos.DrawCube(square.BottomLeft.Position, Vector3.one * 0.4f);

                        Gizmos.color = (square.BottomRight.Active) ? Color.black : Color.white;
                        Gizmos.DrawCube(square.BottomRight.Position, Vector3.one * 0.4f);

                        Gizmos.color = Color.grey;
                        Gizmos.DrawCube(square.CenterTop.Position, Vector3.one * 0.15f);
                        Gizmos.DrawCube(square.CenterBottom.Position, Vector3.one * 0.15f);
                        Gizmos.DrawCube(square.CenterRight.Position, Vector3.one * 0.15f);
                        Gizmos.DrawCube(square.CenterLeft.Position, Vector3.one * 0.15f);

                    }
                }
            }
        }
    }

}