using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
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

            //参考
            //1   A   2
            //B   *   C
            //3   D   4
            switch (square.Configuration)
            {
                //No points
                case 0://0000
                    break;

                // 1 point 
                case 1://0001   3
                    MeshFromPoints(square.CentreBottom, square.BottomLeft, square.CentreLeft);//D3B
                    break;
                case 2://0010   4
                    MeshFromPoints(square.CentreRight, square.BottomRight, square.CentreBottom);//C4D
                    break;
                case 4://0100   2
                    MeshFromPoints(square.CentreTop, square.TopRight, square.CentreRight);//A2C
                    break;
                case 8://1000   1
                    MeshFromPoints(square.TopLeft, square.CentreTop, square.CentreLeft);//1AB
                    break;

                //2 points
                case 3://0011 34
                    MeshFromPoints(square.CentreRight, square.BottomRight, square.BottomLeft, square.CentreLeft);//BC43
                    break;
                case 6://0110 24
                    MeshFromPoints(square.CentreTop, square.TopRight, square.BottomRight, square.CentreBottom);//A24D
                    break;
                case 9://1001 31
                    MeshFromPoints(square.TopLeft, square.CentreTop, square.CentreBottom, square.BottomLeft);//1AD3
                    break;
                case 12://1100 12
                    MeshFromPoints(square.TopLeft, square.TopRight, square.CentreRight, square.CentreLeft);//12CB
                    break;
                case 5://0101 32
                    MeshFromPoints(square.CentreTop, square.TopRight, square.CentreRight, square.CentreBottom, square.BottomLeft, square.CentreLeft);//A2CD3B
                    break;
                case 10://1010 41
                    MeshFromPoints(square.TopLeft, square.CentreTop, square.CentreRight, square.BottomRight, square.CentreBottom, square.CentreLeft);//1AC4DB
                    break;

                //3 points
                case 7://0111   342
                    MeshFromPoints(square.CentreTop, square.TopRight, square.BottomRight, square.BottomLeft, square.CentreLeft);//A243B
                    break;
                case 11://1011   341
                    MeshFromPoints(square.TopLeft, square.CentreTop, square.CentreRight, square.BottomRight, square.BottomLeft);//1AC43
                    break;
                case 13://1101   321
                    MeshFromPoints(square.TopLeft, square.TopRight, square.CentreRight, square.CentreBottom, square.BottomLeft);//12CD3
                    break;
                case 14://1110   421
                    MeshFromPoints(square.TopLeft, square.TopRight, square.BottomRight, square.CentreBottom, square.CentreLeft);//124DB
                    break;

                //4 points
                case 15://1111  1243
                    MeshFromPoints(square.TopLeft, square.TopRight, square.BottomRight, square.BottomLeft);//1243
                    break;
            }
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