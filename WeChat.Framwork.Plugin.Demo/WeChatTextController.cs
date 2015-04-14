using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeChat.Framwork.Core;
using WeChat.Framwork.Core.Entities;

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
            WeChatTextMessageEntity response = new WeChatTextMessageEntity
            {
                ToUserName = request.FromUserName,
                Content = string.Format("你请求的是text类型消息!执行的控制器是:{0},实现:{1}", this.GetType().FullName, this.GetType().GetInterface("WeChatController").FullName),
                MsgType = request.MsgType
            };
            context.Response.Write(response);
        }
    }
}
