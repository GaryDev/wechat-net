using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace WeChat.Framwork.Core.Entities
{
    public class WeChatTextMessageEntity : WeChatMessageBaseEntity
    {
        /// <summary>
        /// 文本消息内容
        /// </summary>
        public string Content { get; set; }

    }
}
