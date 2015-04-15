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
        private const string WELCOME_MESSAGE = 
            "感谢您关注【微信测试平台】\n" + 
            "微信号：devhunter\n" +
            "我们为您提供相关信息查询，做最好的微信测试平台。\n" + 
            "目前平台功能如下：\n" + 
            "【1】 查天气，如输入：TQ上海\n" +
            "【2】 单词翻译，如输入：FYgood\n" + 
            //"【3】 查公交，如输入：苏州公交178\n" +             
            //"【4】 苏州信息查询，如输入：苏州观前街\n" + 
            "更多内容，敬请期待...";

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
                new XElement("Content", WELCOME_MESSAGE));
            context.Response.Write(result);
        }
    }
}
