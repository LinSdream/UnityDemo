using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    #region Class Square
    public class Square
    {
        public ControlNode TopLeft, TopRight, BottomLeft, BottomRight;
        public Node CenterTop, CenterBottom, CenterRight, CenterLeft;
        public int Configuration;//8个点的方块，三角网格配置有16种，即4位二进制0000 - 1111

        public Square(ControlNode _topLeft, ControlNode _topRight, ControlNode _bottomRight, ControlNode _bottomLeft)
        {
            TopLeft = _topLeft;
            TopRight = _topRight;
            BottomRight = _bottomRight;
            BottomLeft = _bottomLeft;

            CenterTop = TopLeft.RightNode;
            CenterRight = BottomRight.AboveNode;
            CenterBottom = BottomLeft.RightNode;
            CenterLeft = BottomLeft.AboveNode;

            if (TopLeft.Active)
                Configuration += 8;
            if (TopRight.Active)
                Configuration += 4;
            if (BottomRight.Active)
                Configuration += 2;
            if (BottomLeft.Active)
                Configuration += 1;
        }


        //public Square(ControlNode bottomLeft, ControlNode bottomRight, ControlNode topLeft, ControlNode topRight)
        //{
        //    TopLeft = topLeft;
        //    TopRight = topRight;
        //    BottomLeft = bottomLeft;
        //    BottomRight = bottomRight;

        //    CenterTop = TopLeft.RightNode;
        //    CenterRight = BottomRight.AboveNode;
        //    CenterBottom = BottomLeft.RightNode;
        //    CenterLeft = TopLeft.AboveNode;

        //    if (TopLeft.Active)
        //        Configuration += 8;
        //    if (TopRight.Active)
        //        Configuration += 4;
        //    if (BottomRight.Active)
        //        Configuration += 2;
        //    if (BottomLeft.Active)
        //        Configuration += 1;
        //}
    }
    #endregion

    #region Class SquareGird
    public class SquareGird
    {
        public Square[,] Squares;

        public SquareGird(int [,] map,float squareSize)
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
            for(int x = 0; x < nodeCountX - 1; x++)
            {
                for(int y = 0; y < nodeCountY - 1; y++)
                {
                    //public Square(ControlNode _topLeft, ControlNode _topRight, ControlNode _bottomRight, ControlNode _bottomLeft)
                    Squares[x, y] = new Square(controlNodes[x, y + 1], controlNodes[x + 1, y + 1], controlNodes[x + 1, y], controlNodes[x, y]);
                    //Squares[x, y] = new Square(controlNodes[x, y], controlNodes[x+1, y], controlNodes[x, y+1], controlNodes[x + 1, y + 1]);
                }
            }
        }
    }
    #endregion
    public class MeshGenerator : MonoBehaviour
    {
        #region Fields
        public SquareGird mSquareGird;

        List<Vector3> _vertices;
        List<int> _triangles;
        #endregion

        #region MonoBehaviour Callbacks
        private void Awake()
        {
            _vertices = new List<Vector3>();
            _triangles = new List<int>();
        }

        private void OnDrawGizmos()
        {
            //if (mSquareGird != null)
            //{
            //    int squareX = mSquareGird.Squares.GetLength(0);
            //    int squareY = mSquareGird.Squares.GetLength(1);

            //    for (int x = 0; x < squareX; x++)
            //    {
            //        for (int y = 0; y < squareY; y++)
            //        {
            //            Square square = mSquareGird.Squares[x, y];
            //            Gizmos.color = (square.TopLeft.Active) ? Color.black : Color.white;
            //            Gizmos.DrawCube(square.TopLeft.Position, Vector3.one * 0.4f);

            //            Gizmos.color = (square.TopRight.Active) ? Color.black : Color.white;
            //            Gizmos.DrawCube(square.TopRight.Position, Vector3.one * 0.4f);

            //            Gizmos.color = (square.BottomLeft.Active) ? Color.black : Color.white;
            //            Gizmos.DrawCube(square.BottomLeft.Position, Vector3.one * 0.4f);

            //            Gizmos.color = (square.BottomRight.Active) ? Color.black : Color.white;
            //            Gizmos.DrawCube(square.BottomRight.Position, Vector3.one * 0.4f);

            //            Gizmos.color = Color.grey;
            //            Gizmos.DrawCube(square.CenterTop.Position, Vector3.one * 0.15f);
            //            Gizmos.DrawCube(square.CenterBottom.Position, Vector3.one * 0.15f);
            //            Gizmos.DrawCube(square.CenterRight.Position, Vector3.one * 0.15f);
            //            Gizmos.DrawCube(square.CenterLeft.Position, Vector3.one * 0.15f);

            //        }
            //    }
            //}
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 三角网格
        /// </summary>
        void TriangulateSquare(Square square)
        {

            switch (square.Configuration)
            {
                case 0:
                    break;

                // 1 points:
                case 1:
                    MeshFromPoints(square.CenterBottom, square.BottomLeft, square.CenterLeft);
                    break;
                case 2:
                    MeshFromPoints(square.CenterRight, square.BottomRight, square.CenterBottom);
                    break;
                case 4:
                    MeshFromPoints(square.CenterTop, square.TopRight, square.CenterRight);
                    break;
                case 8:
                    MeshFromPoints(square.TopLeft, square.CenterTop, square.CenterLeft);
                    break;

                // 2 points:
                case 3:
                    MeshFromPoints(square.CenterRight, square.BottomRight, square.BottomLeft, square.CenterLeft);
                    break;
                case 6:
                    MeshFromPoints(square.CenterTop, square.TopRight, square.BottomRight, square.CenterBottom);
                    break;
                case 9:
                    MeshFromPoints(square.TopLeft, square.CenterTop, square.CenterBottom, square.BottomLeft);
                    break;
                case 12:
                    MeshFromPoints(square.TopLeft, square.TopRight, square.CenterRight, square.CenterLeft);
                    break;
                case 5:
                    MeshFromPoints(square.CenterTop, square.TopRight, square.CenterRight, square.CenterBottom, square.BottomLeft, square.CenterLeft);
                    break;
                case 10:
                    MeshFromPoints(square.TopLeft, square.CenterTop, square.CenterRight, square.BottomRight, square.CenterBottom, square.CenterLeft);
                    break;

                // 3 point:
                case 7:
                    MeshFromPoints(square.CenterTop, square.TopRight, square.BottomRight, square.BottomLeft, square.CenterLeft);
                    break;
                case 11:
                    MeshFromPoints(square.TopLeft, square.CenterTop, square.CenterRight, square.BottomRight, square.BottomLeft);
                    break;
                case 13:
                    MeshFromPoints(square.TopLeft, square.TopRight, square.CenterRight, square.CenterBottom, square.BottomLeft);
                    break;
                case 14:
                    MeshFromPoints(square.TopLeft, square.TopRight, square.BottomRight, square.CenterBottom, square.CenterLeft);
                    break;

                // 4 point:
                case 15:
                    MeshFromPoints(square.TopLeft, square.TopRight, square.BottomRight, square.BottomLeft);
                    break;
            }

            //参考
            //1   A   2
            //B   *   C
            //3   D   4
            //switch (square.Configuration)
            //{
            //    //No points
            //    case 0://0000
            //        break;

            //    // 1 point 
            //    case 1://0001   3
            //        MeshFromPoints(square.CenterBottom, square.BottomLeft, square.CenterLeft);//D3B
            //        break;
            //    case 2://0010   4
            //        MeshFromPoints(square.CenterRight, square.BottomRight, square.CenterBottom);//C4D
            //        break;
            //    case 4://0100   2
            //        MeshFromPoints(square.CenterTop, square.TopRight, square.CenterRight);//A2C
            //        break;
            //    case 8://1000   1
            //        MeshFromPoints(square.TopLeft, square.CenterTop, square.CenterLeft);//1AB
            //        break;

            //    //2 points
            //    case 3://0011 34
            //        MeshFromPoints(square.CenterRight, square.BottomRight, square.BottomLeft, square.CenterLeft);//BC43
            //        break;
            //    case 6://0110 24
            //        MeshFromPoints(square.CenterTop, square.TopRight, square.BottomRight, square.CenterBottom);//A24D
            //        break;
            //    case 9://1001 31
            //        MeshFromPoints(square.TopLeft, square.CenterTop, square.CenterBottom, square.BottomLeft);//1AD3
            //        break;
            //    case 12://1100 12
            //        MeshFromPoints(square.TopLeft, square.TopRight, square.CenterRight, square.CenterLeft);//12CB
            //        break;
            //    case 5://0101 32
            //        MeshFromPoints(square.CenterTop, square.TopRight, square.CenterRight, square.CenterBottom,square.BottomLeft,square.CenterLeft);//A2CD3B
            //        break;
            //    case 10://1010 41
            //        MeshFromPoints(square.TopLeft, square.CenterLeft, square.CenterBottom, square.BottomRight, square.CenterRight, square.CenterTop);//1BD4CA
            //        break;

            //    //3 points
            //    case 7://0111   342
            //        MeshFromPoints(square.CenterTop, square.TopRight, square.BottomRight, square.BottomLeft,square.CenterLeft);//B342A
            //        break;
            //    case 11://1011   341
            //        MeshFromPoints(square.TopLeft, square.CenterTop, square.CenterRight, square.BottomRight, square.BottomLeft);//1AC43
            //        break;
            //    case 13://1101   321
            //        MeshFromPoints(square.TopLeft, square.BottomLeft, square.CenterBottom, square.CenterRight, square.TopRight);//13DC2
            //        break;
            //    case 14://1110   421
            //        MeshFromPoints(square.TopLeft, square.CenterLeft, square.CenterBottom, square.BottomRight, square.TopRight);//1BD42
            //        break;

            //    //4 points
            //    case 15://1111  1234
            //        MeshFromPoints(square.TopLeft, square.TopRight, square.BottomRight, square.BottomLeft);//1234
            //        break;
            //}
        }

        /// <summary>
        /// 点网格
        /// </summary>
        void MeshFromPoints(params Node[] points)
        {
            AssignVertices(points);
            if (points.Length >= 3)
                CreateTriangle(points[0], points[1], points[2]);
            if (points.Length >= 4)
                CreateTriangle(points[0], points[2], points[3]);
            if (points.Length >= 5)
                CreateTriangle(points[0], points[3], points[4]);
            if (points.Length >= 6)
                CreateTriangle(points[0], points[4], points[5]);
        }

        void AssignVertices(Node[] points)
        {
            for(int i = 0; i < points.Length; i++)
            {
                if (points[i].VertexIndex == -1)
                {
                    points[i].VertexIndex = _vertices.Count;
                    _vertices.Add(points[i].Position);
                }
            }
        }

        void CreateTriangle(Node a,Node b,Node c)
        {
            _triangles.Add(a.VertexIndex);
            _triangles.Add(b.VertexIndex);
            _triangles.Add(c.VertexIndex);
        }
#endregion

        #region Public Methods

        /// <summary>
        /// 基于map生成网格
        /// </summary>
        /// <param name="squareSize">网格大小</param>
        public void GeneratorMesh(int[,] map, float squareSize)
        {
            mSquareGird = new SquareGird(map, squareSize);
            int squareX = mSquareGird.Squares.GetLength(0);
            int squareY = mSquareGird.Squares.GetLength(1);

            for (int x = 0; x < squareX; x++)
            {
                for (int y = 0; y < squareY; y++)
                {
                    TriangulateSquare(mSquareGird.Squares[x, y]);
                }
            }

            Mesh mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
            mesh.vertices = _vertices.ToArray();
            mesh.triangles = _triangles.ToArray();

            mesh.RecalculateNormals();
        }
        #endregion
    }

}

/*
 switch (square.Configuration)
            {
                case 0:
                    break;

                // 1 points:
                case 1:
                    MeshFromPoints(square.CenterBottom, square.BottomLeft, square.CenterLeft);
                    break;
                case 2:
                    MeshFromPoints(square.CenterRight, square.BottomRight, square.CenterBottom);
                    break;
                case 4:
                    MeshFromPoints(square.CenterTop, square.TopRight, square.CenterRight);
                    break;
                case 8:
                    MeshFromPoints(square.TopLeft, square.CenterTop, square.CenterLeft);
                    break;

                // 2 points:
                case 3:
                    MeshFromPoints(square.CenterRight, square.BottomRight, square.BottomLeft, square.CenterLeft);
                    break;
                case 6:
                    MeshFromPoints(square.CenterTop, square.TopRight, square.BottomRight, square.CenterBottom);
                    break;
                case 9:
                    MeshFromPoints(square.TopLeft, square.CenterTop, square.CenterBottom, square.BottomLeft);
                    break;
                case 12:
                    MeshFromPoints(square.TopLeft, square.TopRight, square.CenterRight, square.CenterLeft);
                    break;
                case 5:
                    MeshFromPoints(square.CenterTop, square.TopRight, square.CenterRight, square.CenterBottom, square.BottomLeft, square.CenterLeft);
                    break;
                case 10:
                    MeshFromPoints(square.TopLeft, square.CenterTop, square.CenterRight, square.BottomRight, square.CenterBottom, square.CenterLeft);
                    break;

                // 3 point:
                case 7:
                    MeshFromPoints(square.CenterTop, square.TopRight, square.BottomRight, square.BottomLeft, square.CenterLeft);
                    break;
                case 11:
                    MeshFromPoints(square.TopLeft, square.CenterTop, square.CenterRight, square.BottomRight, square.BottomLeft);
                    break;
                case 13:
                    MeshFromPoints(square.TopLeft, square.TopRight, square.CenterRight, square.CenterBottom, square.BottomLeft);
                    break;
                case 14:
                    MeshFromPoints(square.TopLeft, square.TopRight, square.BottomRight, square.CenterBottom, square.CenterLeft);
                    break;

                // 4 point:
                case 15:
                    MeshFromPoints(square.TopLeft, square.TopRight, square.BottomRight, square.BottomLeft);
                    break;
            }
     
     */
