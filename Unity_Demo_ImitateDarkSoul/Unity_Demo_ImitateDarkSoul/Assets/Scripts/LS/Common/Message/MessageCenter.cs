using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace LS.Common.Message
{

    public delegate void CustomMessageHandle(GameObject render,EventArgs e);

    public class MessageCenter : SingletonBasisNoMono<MessageCenter>
    {
        Dictionary<string, CustomMessageHandle> _listeners = new Dictionary<string, CustomMessageHandle>();

        /// <summary>
        /// 注册消息监听事件
        /// </summary>
        /// <param name="name">消息名</param>
        /// <param name="handle">消息</param>
        public void AddListener(string name,CustomMessageHandle handle)
        {
            if(!_listeners.ContainsKey(name))
            {
                _listeners.Add(name, handle);
            }
            else
            {
                _listeners[name] += handle;
            }
        }

        public void RemoveListener(string name,CustomMessageHandle handle)
        {
            if(!_listeners.ContainsKey(name))
            {
                Debug.LogWarning("MessageCenter/RemoveListener Warning : The message not be add , the name is " + name);
                return;
            }
            if (_listeners[name] != null)
                _listeners[name] -= handle;
            if (_listeners[name] == null)
                _listeners.Remove(name);
        }

        public void RemoveListener(string name)
        {
            _listeners.Remove(name);
        }

        /// <summary>
        /// 发送消息,消息具有完整信息
        /// </summary>
        /// <param name="name">消息名</param>
        /// <param name="sender">发送者</param>
        /// <param name="e">消息</param>
        public void SendMessage(string name,GameObject sender,EventArgs e)
        {
            if(!_listeners.ContainsKey(name))
            {
                Debug.LogWarning("MessageCenter/SendMessage Warning : The message not be add , the name is " + name);
                return;
            }
            _listeners[name](sender, e);
        }

        /// <summary>
        /// 发送消息，消息仅包含发送者信息
        /// </summary>
        /// <param name="name">消息名</param>
        /// <param name="sender">发送者</param>
        public void SendMessage(string name,GameObject sender)
        {
            if (!_listeners.ContainsKey(name))
            {
                Debug.LogWarning("MessageCenter/SendMessage Warning : The message not be add , the name is " + name);
                return;
            }
            _listeners[name](sender, null);
        }

        /// <summary>
        /// 发送消息，空消息
        /// </summary>
        /// <param name="name">消息名</param>
        public void SendMessage(string name)
        {
            if (!_listeners.ContainsKey(name))
            {
                Debug.LogWarning("MessageCenter/SendMessage Warning : The message not be add , the name is " + name);
                return;
            }
            _listeners[name](null, null);
        }

        /// <summary>
        /// 获取已存在的消息名
        /// </summary>
        public string[] GetRegisteredMessagesName()
        {
            string[] names = new string[_listeners.Count];
            int index = 0;
            foreach(KeyValuePair<string,CustomMessageHandle> pair in _listeners)
            {
                names[index] = pair.Key;
                index++;
            }
            return names;
        }
    }

}