using UnityEngine;
using System.Collections;

namespace LS.Common.Math
{
    public static class SharedMethods
    {

        /// https://arxiv.org/ftp/arxiv/papers/1509/1509.06344.pdf 第五页
        /// 可用于角色向量的映射到圆形是的向量，其原理与shader中的uv映射应用的是同一个公式
        /// 该论文有挺多有意思的公式可以学习
        #region SquareToDiscMapping Methods
        /// <summary>
        /// 平面直角坐标转化为球面坐标
        /// </summary>
        /// <param name="input">平面坐标</param>
        /// <returns>球面坐标</returns>
        public static Vector2 SquareToDiscMapping(Vector2 input)
        {
            Vector2 output = Vector2.zero;
            output.x = input.x * (Mathf.Sqrt(1 - (input.y * input.y) / 2));
            output.y = input.y * (Mathf.Sqrt(1 - (input.x * input.x) / 2));
            return output;
        }

        /// <summary>
        /// 平面直角坐标转化为球面坐标
        /// </summary>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        /// <returns>球面坐标</returns>
        public static Vector2 SquareToDiscMapping(float x, float y)
        {
            Vector2 output = Vector2.zero;
            output.x = x * (Mathf.Sqrt(1 - (y * y) / 2));
            output.y = y * (Mathf.Sqrt(1 - (x * x) / 2));
            return output;
        }

        /// <summary>
        /// 平面直角坐标转化为球面坐标
        /// </summary>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        /// <param name="u">x坐标对应的新的x坐标</param>
        /// <param name="v">y坐标对应的新的y坐标</param>
        public static void SquareToDiscMapping(float x, float y, ref float u, ref float v)
        {
            u = x * (Mathf.Sqrt(1 - (y * y) / 2));
            v = y * (Mathf.Sqrt(1 - (x * x) / 2));
        }
        #endregion
    }

}