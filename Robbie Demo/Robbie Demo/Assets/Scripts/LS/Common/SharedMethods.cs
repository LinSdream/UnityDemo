﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace LS.Common
{
    public static class SharedMethods
    {
        /// <summary> 权重 </summary>
        public interface IWeightedPair
        {
            string WeightedName { get; }
            int WeightedValue { get; }
        }

        #region BeizerCurve Methods 2阶
        /// <summary>
        /// 获取存取贝塞尔曲线点的数组
        /// </summary>
        /// <param name="startPoint">起始点</param>
        /// <param name="controlPoint">控制点</param>
        /// <param name="targetPoint">目标点</param>
        /// <param name="segmentNum">采样点的数量</param>
        /// <returns>存储贝塞尔曲线点的List表</returns>
        public static List<Vector3> GetBeizerCurveList(Vector3 startPoint, Vector3 controlPoint, Vector3 targetPoint, int segmentNum)
        {
            List<Vector3> list = new List<Vector3>(segmentNum);
            for (int i = 1; i <= segmentNum; i++)
            {
                float t = i / (float)segmentNum;
                Vector3 pixel = BezierCurvePoint(startPoint, controlPoint, targetPoint, t);
                list.Add(pixel);
            }
            return list;
        }

        /// <summary>
        /// 贝塞尔曲线运动
        /// </summary>
        /// <param name="obj">运动对象</param>
        /// <param name="points">贝塞尔曲线点集</param>
        /// <param name="T">差值系数</param>
        public static void BezierCurveMove(GameObject obj, List<Vector3> points, float T = 1.0f)
        {
            if (points.Count <= 1)
            {
                Debug.LogWarning("BezierCurveMove Warning : pointsList Less than 2. Maybe this gameObject already got to the end , else pointsList have mistake");
                return;
            }
            obj.transform.position = Vector3.Lerp(points[0], points[1], T);
            points.RemoveAt(0);
        }

        #endregion

        #region SquareToDiscMapping Methods
        /// https://arxiv.org/ftp/arxiv/papers/1509/1509.06344.pdf 第五页
        /// 可用于角色向量的映射到圆形是的向量，其原理与shader中的uv映射应用的是同一个公式
        /// 该论文有挺多有意思的公式可以学习

        /// <summary>
        /// 平面直角坐标转化为平面的球形坐标
        /// </summary>
        /// <param name="input">平面坐标</param>
        /// <returns>平面的球形坐标</returns>
        public static Vector2 SquareToDiscMapping(Vector2 input)
        {
            Vector2 output = Vector2.zero;
            output.x = input.x * (Mathf.Sqrt(1 - (input.y * input.y) / 2));
            output.y = input.y * (Mathf.Sqrt(1 - (input.x * input.x) / 2));
            return output;
        }

        /// <summary>
        /// 平面直角坐标转化为平面的球形坐标
        /// </summary>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        /// <returns>平面的球形坐标</returns>
        public static Vector2 SquareToDiscMapping(float x, float y)
        {
            Vector2 output = Vector2.zero;
            output.x = x * (Mathf.Sqrt(1 - (y * y) / 2));
            output.y = y * (Mathf.Sqrt(1 - (x * x) / 2));
            return output;
        }

        /// <summary>
        /// 平面直角坐标转化为平面的球形坐标
        /// </summary>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        /// <param name="u">x坐标对应的新的u坐标</param>
        /// <param name="v">y坐标对应的新的v坐标</param>
        public static void SquareToDiscMapping(float x, float y, out float u, out float v)
        {
            u = x * (Mathf.Sqrt(1 - (y * y) / 2));
            v = y * (Mathf.Sqrt(1 - (x * x) / 2));
        }

        #endregion

        #region Others

        /// <summary>
        /// 比较两种颜色
        /// </summary>
        public static bool CompareColorsWithColor32(this Color32 a, Color32 b)
        {
            return (a.a == b.a) && (a.r == b.r) && (a.g == b.g) && (a.b == b.b);
        }

        /// <summary>
        /// 将秒数转为时间戳
        /// </summary>
        /// <param name="value">秒</param>
        public static string SecondValueToTimeStrHHMMSS(int value)
        {
            return $"{value / 3600}:{value % 3600 / 60}:{value % 60}";
        }

        //this的作用是扩展方法 
        //官方文档：https://docs.microsoft.com/zh-cn/dotnet/csharp/programming-guide/classes-and-structs/extension-methods
        /// <summary>从原数组中获取随机个数量的不重复的数组</summary>
        /// <param name="list">源数组</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public static T[] GetRandomArrayFromList<T>(this List<T> list,int min,int max)
        {
            //取随机数
            int value = UnityEngine.Random.Range(min, max);
            if(value>list.Count)
            {
                Debug.LogError("SharedMethods/GetRandomArrayFromList Error : Array out of bounds ");
                return null;
            }

            T[] res = new T[value];

            //记录源数组的下标
            List<int> temp = new List<int>();
            for(int i=0;i<list.Count;i++)
            {
                temp.Add(i);
            }

            int randomNum;
            int index = 0;
            while(temp.Count>list.Count-value)
            {
                randomNum = UnityEngine.Random.Range(0, temp.Count);
                res[index++] = list[temp[randomNum]];
                temp.Remove(temp[randomNum]);
            }
            return res;

        }

        /// <summary>根据权重获得随机数</summary>
        /// <param name="list">随机数组</param>
        public static T GetWeightedRandomRes<T>(this T[] list) where T:IWeightedPair
        {
            //将所有的权重累加
            int sum = 0;
            for(int i=0;i<list.Length;i++)
            {
                sum += list[i].WeightedValue;
            }
            //从1到权重随机取值
            int random = UnityEngine.Random.Range(1, sum+1);//取闭区间
            sum = 0;
            for(int i=0;i<list.Length;i++)//遍历所有的权重
            {
                sum += list[i].WeightedValue;
                if (sum >= random)//如果权重和大于等于随机数，则该值为索要选取的随机数
                    return list[i];
            }
            return list[0];
        }

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
            index = distance.GetMinOrMaxIndexInArray(false);
            return colliders[index];
        }

       
        /// <summary> 计算目标对象是否在源对象的范围内 </summary>
        /// <param name="origin">源对象</param>
        /// <param name="target">目标对象</param>
        /// <param name="sqrDistance">距离平方</param>
        /// <param name="halfAngle">半角</param>
        public static bool IsInArea(Transform origin,Transform target,float sqrDistance, float halfAngle)
        {
            if (target == null)
                return false;

            //计算自己到目标的向量
            Vector3 toTarget = target.position - origin.position;
            //如果距离超出视线范围
            float distance = toTarget.sqrMagnitude;
            if (distance > sqrDistance)
                return false;
            float angle = Mathf.Acos(Vector3.Dot(origin.forward.normalized, toTarget.normalized)) * Mathf.Rad2Deg;
            if (angle > halfAngle)
                return false;
            return true;
        }

        /// <summary>
        /// 获取最大或最小值的下标
        /// </summary>
        /// <typeparam name="T">继承自IComparable</typeparam>
        /// <param name="arr">数组</param>
        /// <param name="isMax">是否是取最大值</param>
        public static int GetMinOrMaxIndexInArray<T>(this T[] arr, bool isMax = true) where T : IComparable<T>
        {
            int index = 0;
            if (arr == null || arr.Length == 0)
            {
                Debug.LogError("SharedMethods/GetMinOrMaxInArray Error :  array of arr is an invalid value ! ");
                return 0;
            }

            T tmp = arr[0];

            for (int i = 0; i < arr.Length; i++)
            {
                if (isMax)
                {
                    if (tmp.CompareTo(arr[i]) != 1)
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

        /// <summary>
        /// 截取父数组中的子数组
        /// </summary>
        /// <param name="arr">父数组</param>
        /// <param name="startIndex">开始位置</param>
        /// <param name="lenth">长度</param>
        public static T[] SubArray<T>(T[] arr,int startIndex,int lenth)
        {
            if(startIndex<0||startIndex>arr.Length||lenth<0)
            {
                Debug.LogError("SharedMethods/SubArray Error : argument out of range");
                return null;
            }
            T[] newArr = new T[lenth];
            if(startIndex+lenth<=arr.Length)
            {
                for (int i = 0; i < lenth; i++)
                    newArr[i] = arr[i + startIndex];
            }
            else
            {
                Debug.LogError("SharedMethods/SubArray Error : argument out of range");
                return null;
            }
            return newArr;
        }

        /// <summary>
        /// 搜索父物体下的目标名的Transform
        /// </summary>
        public static Transform DeepFindTransform(Transform parent, string targetName)
        {

            Transform temp = null;

            foreach (Transform child in parent)
            {
                if (child.name == targetName)
                    return child;
                else
                {
                    temp = DeepFindTransform(child, targetName);
                    if (temp != null)
                        return temp;
                }
            }
            return null;
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// 根据T值，计算贝塞尔曲线上面相对应的点
        /// </summary>
        /// <param name="startPoint">其实点</param>
        /// <param name="controllerPoint">控制点</param>
        /// <param name="targetPoint">目标点</param>
        /// <param name="T">T值</param>
        /// <returns>根据T值计算出来的贝塞尔曲线点</returns>
        private static Vector3 BezierCurvePoint(Vector3 startPoint, Vector3 controllerPoint, Vector3 targetPoint, float T)
        {
            float u = 1 - T;
            float tt = T * T;
            float uu = u * u;
            Vector3 point = uu * startPoint;
            point += 2 * u * T * controllerPoint;
            point += tt * targetPoint;
            return point;
        }

        /// <summary>
        /// 根据T值，计算贝塞尔曲线上面相对应的点  三阶
        /// </summary>
        /// <param name="startPoint">起始点</param>
        /// <param name="controlPoint1">控制点 1 </param>
        /// <param name="controlPoint2">控制点 2 </param>
        /// <param name="endPoint">终点</param>
        /// <param name="T">T值</param>
        /// <returns>根据T值计算的点</returns>
        private static Vector3 GetBeizerCurveList3(Vector3 startPoint, Vector3 controlPoint1, Vector3 controlPoint2, Vector3 endPoint, float T)
        {
            Vector3 result;
            Vector3 p0p1 = (1 - T) * startPoint + T * controlPoint1;
            Vector3 p1p2 = (1 - T) * controlPoint1 + T * controlPoint2;
            Vector3 p2p3 = (1 - T) * controlPoint2 + T * endPoint;
            Vector3 p0p1p2 = (1 - T) * p0p1 + T * p1p2;
            Vector3 p1p2p3 = (1 - T) * p1p2 + T * p2p3;
            result = (1 - T) * p0p1p2 + T * p1p2p3;
            return result;
        }
        #endregion

    }

}