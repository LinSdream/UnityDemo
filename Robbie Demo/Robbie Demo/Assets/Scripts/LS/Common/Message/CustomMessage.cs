using UnityEngine;
using System.Collections;
using System;

namespace LS.Common.Message
{
    public class CustomMessage
    {
        /// <summary>
        /// 消息的发送者
        /// </summary>
        public GameObject Sender;

        /// <summary>
        /// 消息
        /// </summary>
        public EventArgs Args;

        public CustomMessage() { }

        public CustomMessage(GameObject sender,EventArgs e)
        {
            Sender = sender;
            Args = e;
        }

        public CustomMessage(EventArgs e)
        {
            Sender = null;
            Args = e;
        }

        public CustomMessage(GameObject sender)
        {
            Sender = sender;
            Args = null;
        }
    }

}