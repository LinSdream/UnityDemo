using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS.Helper.Timer
{
    public class Timer
    {
        /// <summary>
        /// 计时器时长
        /// </summary>
        public float Duration;
        /// <summary>
        /// 计时器初始化回调
        /// </summary>
        public Action InitDo;
        /// <summary>
        /// 计时器循环回调，部分协程不会调用AfterDo
        /// </summary>
        public Action AfterDo;
        /// <summary>
        /// 计时器结束后回调
        /// </summary>
        public Action Callback;

        public int Status = -1;

        public Timer()
        {
            Duration = 0f;
        }

        public Timer(float duration, Action initDo = null, Action afterDo = null, Action callback = null)
        {
            Duration = duration;
            InitDo = initDo;
            AfterDo = afterDo;
            Callback = callback;
        }

        /// <summary>
        /// 简单计时器，简简单单就是美，无任何的事件回调
        /// </summary>
        public IEnumerator SimpleTimerForSeconds()
        {
            Status = 1;
            yield return new WaitForSeconds(Duration);
            Status = 0;
        }

        /// <summary>
        /// 正计时器, 可以指定每次停顿时执行的动作，
        /// 如果循环，AfterDo将在每轮循环的时候调用，如果不循环，作用同Callback
        /// </summary>
        /// <param name="repeat">是否循环</param>
        public IEnumerator TimerForSeconds(bool repeat = false)
        {
            Status = 1;
            InitDo?.Invoke();
            do
            {
                yield return new WaitForSeconds(Duration);
                AfterDo?.Invoke();
            } while (repeat);
            Callback?.Invoke();
            Status = 0;
        }

        /// <summary>
        /// 指定时间段内，每x秒进行操作，调用AfterDo事件
        /// </summary>
        /// <param name="xTime">间隔xTime 秒，调用一次的AfterDo </param>
        public IEnumerator TimerForEveryXSecond(float xTime = 1f)
        {
            Status = 1;
            float timer = 0;
            InitDo?.Invoke();
            do
            {
                yield return new WaitForSeconds(xTime);
                AfterDo?.Invoke();
                timer += xTime;
            } while (timer <= Duration);
            Callback?.Invoke();
        }

        /// <summary>
        /// 每帧计时
        /// </summary>
        public IEnumerator TimerForEndFrame()
        {
            Status = 1;
            InitDo?.Invoke();
            int count = 0;
            do
            {
                count++;
                yield return new WaitForEndOfFrame();
                AfterDo?.Invoke();
            } while (count < Duration);
            Callback?.Invoke();
            Status = 0;
        }

        /// <summary>
        /// 从程序开始以来的真实时间的计时器
        /// </summary>
        public IEnumerator TimerForRealTimeSinceStartup()
        {
            Status = 1;
            InitDo?.Invoke();
            float start = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup < start + Duration)
            {
                yield return null;
                AfterDo();
            }
            Callback?.Invoke();
            Status = 0;
        }

        public IEnumerator NextFrame()
        {
            Status = 1;
            InitDo?.Invoke();
            yield return new WaitForEndOfFrame();
            Callback?.Invoke();
            Status = 0;
        }

        public IEnumerator NextFixedFrame()
        {
            Status = 1;
            InitDo?.Invoke();
            yield return new WaitForFixedUpdate();
            Callback?.Invoke();
            Status = 0;
        }

    }
}