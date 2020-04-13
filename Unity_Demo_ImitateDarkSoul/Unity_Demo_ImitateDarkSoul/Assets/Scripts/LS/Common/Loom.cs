using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace LS.Common
{

    public class Loom:MonoSingletionBasis<Loom>
    {
        public struct DelayedQueueItem
        {
            public float Time;
            public Action DelayedAction;
        }

        public static int MaxThreads = 8;
        static int _numThreads;

        List<Action> _actions = new List<Action>();
        List<Action> _currentActions = new List<Action>();
        List<DelayedQueueItem> _delayedList = new List<DelayedQueueItem>();
        List<DelayedQueueItem> _currentDelayed = new List<DelayedQueueItem>();

        public static void QueueOnMainThread(Action action)
        {
            QueueOnMainThread(action, 0f);
        }

        private static void QueueOnMainThread(Action action, float time)
        {
            if(time!=0)
            {
                lock(_instance._delayedList)
                {
                    _instance._delayedList.Add(new DelayedQueueItem() { Time = Time.time+time, DelayedAction = action });
                }
            }
            else
            {
                lock(_instance._actions)
                {
                    _instance._actions.Add(action);
                }
            }
        }

        static Thread RunActionAsync(Action action)
        {
            while(_numThreads>=MaxThreads)
            {
                Thread.Sleep(1);
            }
            Interlocked.Increment(ref _numThreads);
            ThreadPool.QueueUserWorkItem(RunAction, action);
            return null;
        }

        private static void RunAction(object action)
        {
            try
            {
                ((Action)action)();
            }
            catch
            {

            }
            finally
            {
                Interlocked.Decrement(ref _numThreads);
            }
        }

        private void Update()
        {
            lock(_actions)
            {
                _currentActions.Clear();
                _currentActions.AddRange(_actions);
                _actions.Clear();
            }
            foreach(var cell in _currentActions)
            {
                cell();
            }
            lock(_delayedList)
            {
                _currentDelayed.Clear();
                _currentDelayed.AddRange(_delayedList.Where(d => d.Time <= Time.time));
                foreach(var cell in _currentDelayed)
                {
                    cell.DelayedAction();
                }
            }
        }
    }

}