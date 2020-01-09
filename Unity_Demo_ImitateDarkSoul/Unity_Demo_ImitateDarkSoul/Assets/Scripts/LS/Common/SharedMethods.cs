using UnityEngine;
using System.Collections;
using System;

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

        /// <summary>
        /// 计算离源目标最近的collider
        /// </summary>
        public static Collider CalculateNearestCollider(Transform origin, Collider[] colliders)
        {
            float[] distance = new float[colliders.Length];
            int index = 0;
            foreach (var cell in colliders)
            {
                distance[index] = (cell.transform.position - origin.position).sqrMagnitude;
                index++;
            }
            index = GetMinOrMaxIndexInArray(distance,false);
            return colliders[index];
        }


        /// <summary>
        /// 获取最大或最小值的下标
        /// </summary>
        /// <typeparam name="T">继承自IComparable</typeparam>
        /// <param name="arr">数组</param>
        /// <param name="isMax">是否是取最大值</param>
        public static  int GetMinOrMaxIndexInArray<T>(T[] arr,bool isMax=true) where T :IComparable<T>
        {
            int index = 0;
            if (arr == null||arr.Length==0)
            {
                Debug.LogError("SharedMethods/GetMinOrMaxInArray Error :  array of arr is an invalid value ! ");
                return 0;
            }

            T tmp = arr[0];

            for(int i=0;i<arr.Length;i++)
            {
                if(isMax)
                {
                    if(tmp.CompareTo(arr[i])!=1)
                    {
                        tmp = arr[i];
                        index = i;
                    }
                }
                else
                {
                    if (tmp.CompareTo(arr[i]) != -1)
                    {
                        tmp = arr[i];
                        index = i;
                    }
                }
            }
            return index;
        }

    }

}