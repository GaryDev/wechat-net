using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeChat.Framwork.Core.Entities;

namespace WeChat.Framwork.Plugin.Demo
{
    public interface WeChatTextAnalyzer
    {
        WeChatMessageBaseEntity GetResponse(WeChatTextMessageEntity request);
    }
}
