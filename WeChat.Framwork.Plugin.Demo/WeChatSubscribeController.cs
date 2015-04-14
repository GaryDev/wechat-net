using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeChat.Framwork.Core;
using System.Xml.Linq;

namespace WeChat.Framwork.Plugin.Demo
{
    public class WeChatSubscribeController : WeChatControllerAdapter
    {

        public override WeChatMsgType WeChatMsgType
        {
            get { return WeChatMsgType.Event; }
        }

        public override WeChatEventType WeChatEventType
        {
            get { return WeChatEventType.SubScribe; }
        }

        public override void ProcessWeChat(WeChatContext context)
        {
            XElement result = new XElement("xml", new XElement("ToUserName", context.Request.FromUserName),
                new XElement("FromUserName", context.Request.ToUserName),
                new XElement("CreateTime", DateTime.Now.GetInt()),
                new XElement("MsgType", WeChatMsgType.Text.ToString().ToLower()),
                new XElement("Content", "欢迎关注***的微信订阅号.回复试试,惊喜不断!微信机器人"));
            context.Response.Write(result);
        }
    }
}
