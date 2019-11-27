using UnityEngine;
using System.Collections;

namespace Game
{
    public class Square
    {
        public ControlNode TopLeft, TopRight, BottomLeft, BottomRight;
        public Node CentreTop, CentreBottom, CentreRight, CentreLeft;
        public int Configuration;//8个点的方块，三角网格配置有16种，即4位二进制0000 - 1111

        public Square(ControlNode topLeft, ControlNode topRight, ControlNode bottomRight, ControlNode bottomLeft)
        {
            TopLeft = topLeft;
            TopRight = topRight;
            BottomRight = bottomRight;
            BottomLeft = bottomLeft;

            CentreTop = TopLeft.RightNode;
            CentreRight = BottomRight.AboveNode;
            CentreBottom = BottomLeft.RightNode;
            CentreLeft = BottomLeft.AboveNode;

            if (TopLeft.Active)
                Configuration += 8;
            if (TopRight.Active)
                Configuration += 4;
            if (BottomRight.Active)
                Configuration += 2;
            if (BottomLeft.Active)
                Configuration += 1;
        }
    }

    public class SquareGird
    {
        public Square[,] Squares;

        public SquareGird(int[,] map, float squareSize)
        {
            int nodeCountX = map.GetLength(0);//x轴node数目
            int nodeCountY = map.GetLength(1);//y轴node数量

            float mapWidth = nodeCountX * squareSize;//map宽度
            float mapHeight = nodeCountY * squareSize;//map高度

            ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountY];
            for (int x = 0; x < nodeCountX; x++)
            {
                for (int y = 0; y < nodeCountY; y++)
                {
                    //计算控制点的中点
                    Vector3 pos = new Vector3(-mapWidth / 2 + x * squareSize + squareSize / 2, 0, -mapHeight / 2 + y * squareSize + squareSize / 2);
                    controlNodes[x, y] = new ControlNode(pos, map[x, y] == 1, squareSize);
                }
            }
            Squares = new Square[nodeCountX - 1, nodeCountY - 1];
            for (int x = 0; x < nodeCountX - 1; x++)
            {
                for (int y = 0; y < nodeCountY - 1; y++)
                {
                    Squares[x, y] = new Square(controlNodes[x, y + 1], controlNodes[x + 1, y + 1], controlNodes[x + 1, y], controlNodes[x, y]);
                }
            }
        }
    }
}