using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace WeChat.Framwork.Core
{
    public class WeChatContext
    {
        #region 字段/属性
        /// <summary>
        /// 微信请求上下文
        /// </summary>
        public WeChatRequest Request { get; set; }
        /// <summary>
        /// 微信响应上下文
        /// </summary>
        public WeChatResponse Response { get; set; }
        #endregion

        #region 构造函数

        public WeChatContext()
        {

        }

        /// <summary>
        /// 构造函数，把请求消息封装到微信请求/响应上下文属性
        /// </summary>
        /// <param name="requestXml">请求消息xml</param>
        public WeChatContext(XElement requestXml)
        {
            Request = new WeChatRequest(requestXml);
            Response = new WeChatResponse();
        }

        #endregion
    }
}
