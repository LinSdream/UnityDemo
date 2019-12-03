using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Triangle Mesh 相关连接：https://www.cnblogs.com/haoyul/p/5738364.html

namespace Game
{
    #region Struct Triangle

    /// <summary>
    /// 三角形级别信息
    /// </summary>
    public struct Triangle
    {
        int[] _vertexs;

        public Triangle(int a,int b,int c)
        {
            _vertexs = new int[3];
            _vertexs[0] = a;
            _vertexs[1] = b;
            _vertexs[2] = c;
        }

        public int this[int index]
        {
            get
            {
                return _vertexs[index];
            }
        }

        public bool Contains(int vertexIndex)
        {
            return vertexIndex == _vertexs[0] || vertexIndex == _vertexs[1] || vertexIndex == _vertexs[2];
        }
    }
    #endregion

    public class MeshGenerator : MonoBehaviour
    {
        #region Fields
        public SquareGird mSquareGird;
        public MeshFilter WallMeshFilter;

        List<Vector3> _vertices;//顶点表
        List<int> _triangles;//维护所有三角形的数组，内部存储的数据数是顶点表的下角标
        Dictionary<int, List<Triangle>> _triangleDic;
        List<List<int>> _outlines;
        HashSet<int> _checkedVertices;
        #endregion

        #region MonoBehaviour Callbacks
        private void Awake()
        {
            _vertices = new List<Vector3>();
            _triangles = new List<int>();
            _triangleDic = new Dictionary<int, List<Triangle>>();
            _outlines = new List<List<int>>();
            _checkedVertices = new HashSet<int>();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 方块内计算三角形个数，生成单个方块内存在多个三角形
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
                    MeshFromPoints(square.CentreLeft, square.CentreBottom, square.BottomLeft);//BD3
                    break;
                case 2://0010   4
                    MeshFromPoints(square.BottomRight, square.CentreBottom, square.CentreRight);//4DC
                    break;
                case 4://0100   2
                    MeshFromPoints(square.TopRight, square.CentreRight, square.CentreTop);//2CA
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
                    _checkedVertices.Add(square.TopLeft.VertexIndex);
                    _checkedVertices.Add(square.TopRight.VertexIndex);
                    _checkedVertices.Add(square.BottomRight.VertexIndex);
                    _checkedVertices.Add(square.BottomLeft.VertexIndex);
                    break;
            }
        }

        /// <summary>
        /// 创建三角网格中的三角块
        /// </summary>
        void MeshFromPoints(params Node[] points)
        {
            AssignVertices(points);
            //三角扇维护方式，第一个顶点为所有的三角形共同的点
            if (points.Length >= 3)
                CreateTriangle(points[0], points[1], points[2]);
            if (points.Length >= 4)
                CreateTriangle(points[0], points[2], points[3]);
            if (points.Length >= 5)
                CreateTriangle(points[0], points[3], points[4]);
            if (points.Length >= 6)
                CreateTriangle(points[0], points[4], points[5]);
        }

        /// <summary>
        /// 填写顶点表
        /// </summary>
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

        /// <summary>
        /// 建立三角网格中的三角关系
        /// </summary>
        void CreateTriangle(Node a,Node b,Node c)
        {
            _triangles.Add(a.VertexIndex);
            _triangles.Add(b.VertexIndex);
            _triangles.Add(c.VertexIndex);

            Triangle triangle = new Triangle(a.VertexIndex, b.VertexIndex, c.VertexIndex);

            AddTriangleToDictionary(triangle[0], triangle);
            AddTriangleToDictionary(triangle[1], triangle);
            AddTriangleToDictionary(triangle[2], triangle);
        }

        /// <summary>
        /// 添加三角网格到字典中
        /// </summary>
        void AddTriangleToDictionary(int vertexIndexKey,Triangle triangleValue)
        {
            if (_triangleDic.ContainsKey(vertexIndexKey))
            {
                _triangleDic[vertexIndexKey].Add(triangleValue);
            }
            else
            {
                List<Triangle> triangles = new List<Triangle>();
                triangles.Add(triangleValue);
                _triangleDic.Add(vertexIndexKey, triangles);
            }
        }

        /// <summary>
        /// 判断是否为边缘
        /// </summary>
        bool IsOutlineEdge(int vertexA,int vertexB)
        {
            List<Triangle> triangleContainingVertexA = _triangleDic[vertexA];
            int sharderTriangleCount = 0;

            for(int i = 0; i < triangleContainingVertexA.Count; i++)
            {
                if (triangleContainingVertexA[i].Contains(vertexB))
                {
                    sharderTriangleCount++;
                    if (sharderTriangleCount > 1)
                        break;
                }
            }
            return sharderTriangleCount == 1;
        }

        int GetConnectedOutlineVertex(int vertexIndex)
        {
            List<Triangle> trianglesContainingVertex = _triangleDic[vertexIndex];

            for (int i = 0; i < trianglesContainingVertex.Count; i++)
            {
                Triangle triangle = trianglesContainingVertex[i];
                for(int j = 0; j < 3; j++)
                {
                    int vertexB = triangle[j];
                    if (vertexB != vertexIndex && !_checkedVertices.Contains(vertexB))
                    {
                        if (IsOutlineEdge(vertexIndex, vertexB))
                        {
                            return vertexB;
                        }
                    }
                }
            }
            return -1;
        }

        void CalculateMeshOutlines()
        {
            for(int i = 0; i < _vertices.Count; i++)
            {
                if (!_checkedVertices.Contains(i))
                {
                    int newOutlineVertex = GetConnectedOutlineVertex(i);
                    if (newOutlineVertex != -1)
                    {
                        _checkedVertices.Add(i);

                        List<int> newOutlines = new List<int>();
                        newOutlines.Add(i);
                        _outlines.Add(newOutlines);
                        FollowOutline(newOutlineVertex, _outlines.Count - 1);
                        _outlines[_outlines.Count - 1].Add(i);
                    }
                }
            }
        }

        void FollowOutline(int vertexIndex, int outlineIndex)
        {
            _outlines[outlineIndex].Add(vertexIndex);
            _checkedVertices.Add(vertexIndex);
            int nextVertexIndex = GetConnectedOutlineVertex(vertexIndex);
            if (nextVertexIndex != -1)
            {
                FollowOutline(nextVertexIndex, outlineIndex);
            }
        }

        void CreateWallMesh()
        {
            CalculateMeshOutlines();

            List<Vector3> wallVertices = new List<Vector3>();
            List<int> wallTriangles = new List<int>();

            Mesh wallMesh = new Mesh();
            float wallHeight = 5;
            
            foreach(List<int> group in _outlines)
            {
                for (int i = 0; i < group.Count - 1; i++) 
                {
                    int startIndex = wallVertices.Count;
                    wallVertices.Add(_vertices[group[i]]);//left
                    wallVertices.Add(_vertices[group[i+1]]);//right
                    wallVertices.Add(_vertices[group[i]] - Vector3.up * wallHeight);//bottomLeft
                    wallVertices.Add(_vertices[group[i+1]] - Vector3.up * wallHeight);//bottomRight

                    wallTriangles.Add(startIndex + 0);
                    wallTriangles.Add(startIndex + 2);
                    wallTriangles.Add(startIndex + 3);

                    wallTriangles.Add(startIndex + 3);
                    wallTriangles.Add(startIndex + 1);
                    wallTriangles.Add(startIndex + 0);
                }
            }

            wallMesh.vertices = wallVertices.ToArray();
            wallMesh.triangles = wallTriangles.ToArray();

            WallMeshFilter.mesh = wallMesh;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// 基于map生成三角网格
        /// </summary>
        public void GeneratorMesh(int[,] map, float squareSize)
        {
            _triangleDic.Clear();
            _outlines.Clear();
            _checkedVertices.Clear();

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

            Debug.Log(_triangleDic.Count);
            //Create Triangle Mesh
            Mesh mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
            mesh.vertices = _vertices.ToArray();
            mesh.triangles = _triangles.ToArray();

            mesh.RecalculateNormals();

            //Create the wall's triangle mesh
            CreateWallMesh();
        }

        #endregion
    }

}