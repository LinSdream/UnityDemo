using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LS.Common;

namespace LS.Helper.Timer
{

    /// <summary>
    /// 计时器管理类
    /// Update方式实现
    /// Time:  2018.4.2
    /// </summary>
    public class TimerManagerByUpdate : ASingletonBasis<TimerManagerByUpdate>
    {
        private List<TimerByUpdate> _timers;
        private Dictionary<string, TimerByUpdate> _timersDict;

        protected override void Awake()
        {
            base.Awake();
            _timers = new List<TimerByUpdate>();
            _timersDict = new Dictionary<string, TimerByUpdate>();
        }

        private void Update()
        {
            foreach(TimerByUpdate t in _timers)
            {
                t.OnUpdate(Time.deltaTime);
            }
        }

        public void AddTimer(string name, TimerByUpdate timer)
        {
            if (_timersDict.ContainsKey(name))
            {
                _timersDict[name].LeftTime += _timersDict[name].Duration;
            }
            else
            {
                _timersDict.Add(name, timer);
                _timers.Add(timer);
            }
        }

        public void RemoveTimer(string name)
        {
            TimerByUpdate timer = _timersDict[name];
            if (_timers != null)
            {
                _timers.Remove(timer);
                _timersDict.Remove(name);
            }
            else
            {
                Debug.LogError($"RemoveTimer Error : can't have the timer whitch name is {name}");
                return;
            }
        }

    }

}