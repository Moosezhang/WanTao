using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.WeChat
{
    public class MessageResolver
    {
        public bool IsMessage { get; set; }
        public bool IsEvent { get; set; }

    }

    public enum MessageType
    {
        /// <summary>
        /// 文本消息
        /// </summary>
        text,

        image,

        voice,

    }

    public enum EventType
    {

    }
}
