using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS.Helper.Timer
{
    public class Timer
    {
        public float Duration => _duration;
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


        public int Status => _status;

        private int _status = -1;

        /// <summary>
        /// 计时器时长
        /// </summary>
        private float _duration;
        private WaitForSeconds _second;

        public Timer(float duration = 0, Action initDo = null, Action afterDo = null, Action callback = null)
        {
            _duration = duration;
            InitDo = initDo;
            AfterDo = afterDo;
            Callback = callback;
            _second = new WaitForSeconds(duration);
        }

        /// <summary>
        /// 简单计时器，简简单单就是美，无任何的事件回调
        /// </summary>
        public IEnumerator SimpleTimerForSeconds()
        {
            _status = 1;
            yield return _second;
            _status = 0;
        }

        /// <summary>
        /// 正计时器, 可以指定每次停顿时执行的动作，
        /// 如果循环，AfterDo将在每轮循环的时候调用，如果不循环，作用同Callback
        /// </summary>
        /// <param name="repeat">是否循环</param>
        public IEnumerator TimerForSeconds(bool repeat = false)
        {
            _status = 1;
            InitDo?.Invoke();
            do
            {
                yield return _second;
                AfterDo?.Invoke();
            } while (repeat);
            Callback?.Invoke();
            _status = 0;
        }

        /// <summary>
        /// 指定时间段内，每x秒进行操作，调用AfterDo事件
        /// </summary>
        /// <param name="xTime">间隔xTime 秒，调用一次的AfterDo </param>
        public IEnumerator TimerForEveryXSecond(float xTime = 1f, bool repeat = false)
        {
            _status = 1;
            var x = new WaitForSeconds(xTime);
            float timer = 0;
            InitDo?.Invoke();
            if (repeat)
            {
                while (true)
                {
                    yield return x;
                    AfterDo?.Invoke();
                }
            }
            else
            {
                do
                {
                    yield return x;
                    AfterDo?.Invoke();
                    timer += xTime;
                } while (timer <= _duration);
            }

            Callback?.Invoke();
        }

        /// <summary>
        /// 每帧计时
        /// </summary>
        public IEnumerator TimerForEndFrame()
        {
            _status = 1;
            InitDo?.Invoke();
            int count = 0;
            do
            {
                count++;
                yield return new WaitForEndOfFrame();
                AfterDo?.Invoke();
            } while (count < _duration);
            Callback?.Invoke();
            _status = 0;
        }

        /// <summary>
        /// 从程序开始以来的真实时间的计时器
        /// </summary>
        public IEnumerator TimerForRealTimeSinceStartup()
        {
            _status = 1;
            InitDo?.Invoke();
            float start = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup < start + _duration)
            {
                yield return null;
                AfterDo();
            }
            Callback?.Invoke();
            _status = 0;
        }

        public IEnumerator NextFrame()
        {
            _status = 1;
            InitDo?.Invoke();
            yield return new WaitForEndOfFrame();
            Callback?.Invoke();
            _status = 0;
        }

        public IEnumerator NextFixedFrame()
        {
            _status = 1;
            InitDo?.Invoke();
            yield return new WaitForFixedUpdate();
            Callback?.Invoke();
            _status = 0;
        }

        /// <summary>
        /// 重新设置时间
        /// </summary>
        /// <param name="value">新时间</param>
        /// <param name="createNewWaitForSecond">是否重新设置等待时间</param>
        public void SetDuration(float value, bool createNewWaitForSecond = true)
        {
            _duration = value;
            if (createNewWaitForSecond)
                _second = new WaitForSeconds(_duration);
        }

    }
}