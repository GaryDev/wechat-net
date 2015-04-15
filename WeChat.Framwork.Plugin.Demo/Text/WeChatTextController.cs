using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeChat.Framwork.Core;
using WeChat.Framwork.Core.Entities;
using System.Data;
using System.Collections;

namespace WeChat.Framwork.Plugin.Demo
{
    public class WeChatTextController : WeChatControllerAdapter
    {

        public override WeChatMsgType WeChatMsgType
        {
            get { return WeChatMsgType.Text; }
        }

        public override void ProcessWeChat(WeChatContext context)
        {
            WeChatTextMessageEntity request = context.Request.GetRequestModel<WeChatTextMessageEntity>();
            WeChatMessageBaseEntity response;
            if (string.IsNullOrEmpty(request.Content))
            {
                response = new WeChatTextMessageEntity
                {
                    ToUserName = request.FromUserName,
                    Content = "说点什么吧...",
                    MsgType = request.MsgType
                };
            }
            else
            {
                try
                {
                    WeChatTextAnalyzer analyzer = WeChatTextFactory.GetInstance().CreateAnalyzer(request.Content.Trim());
                    response = analyzer.GetResponse(request);
                }
                catch (WeChatTextInvalidArgumentException ex)
                {
                    response = new WeChatTextMessageEntity
                    {
                        ToUserName = request.FromUserName,
                        Content = ex.Message,
                        MsgType = request.MsgType
                    };
                }
            }
            context.Response.Write(response);
        }
    }
}
