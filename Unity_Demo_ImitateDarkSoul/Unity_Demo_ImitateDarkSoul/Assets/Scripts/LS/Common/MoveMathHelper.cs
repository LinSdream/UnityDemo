using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace LS.Common
{
    public static class MoveMathHelper
    {
        #region Public Methods
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

        
        /// <summary>
        /// 匀速直线运动
        /// </summary>
        /// <param name="obj">移动对象</param>
        /// <param name="direction">移动方向</param>
        /// <param name="v0">初速度</param>
        /// <param name="a">加速度</param>
        /// <param name="t">时间(推荐定义为Time.deltaTime)</param>
        public static void UniformAccelerationMove(GameObject obj, Vector3 direction, float v0, float a, float t)
        {
            float vt = v0 + a * t;
            obj.transform.Translate(direction * vt * t);
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