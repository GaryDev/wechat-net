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
            string respMsg = string.Empty;
            if (string.IsNullOrEmpty(request.Content))
                respMsg = "说点什么吧...";
            else
            {
                try
                {
                    WeChatTextAnalyzer analyzer = WeChatTextFactory.GetInstance().CreateAnalyzer(request.Content.Trim());
                    respMsg = analyzer.GetResponseMessage();
                }
                catch (WeChatTextInvalidArgumentException ex)
                {
                    respMsg = ex.Message;
                }
            }
            WeChatTextMessageEntity response = new WeChatTextMessageEntity
            {
                ToUserName = request.FromUserName,
                Content = respMsg,
                MsgType = request.MsgType
            };
            context.Response.Write(response);
        }
    }
}
