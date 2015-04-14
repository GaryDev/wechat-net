using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeChat.Framwork.Core
{
    /// <summary>
    /// 微信处理控制器请求接口
    /// </summary>
    public interface WeChatController
    {
        /// <summary>
        /// 处理微信请求
        /// </summary>
        /// <param name="context">微信上下文</param>
        void ProcessWeChat(WeChatContext context);
    }
}
