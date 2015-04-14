using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeChat.Framwork.Core
{
    public abstract class WeChatControllerAdapter : WeChatController
    {
        /// <summary>
        /// 微信消息类型
        /// </summary>
        public abstract WeChatMsgType WeChatMsgType { get; }
        /// <summary>
        /// 微信事件消息类型
        /// </summary>
        public virtual WeChatEventType WeChatEventType
        {
            get { return WeChatEventType.None; }
        }

        public abstract void ProcessWeChat(WeChatContext context);        
    }
}
