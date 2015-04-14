using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace WeChat.Framwork.Core
{
    public class WeChatRequest
    {
        #region 字段/属性
        /// <summary>
        /// 请求消息XElement类型属性
        /// </summary>
        public XElement RequestXmlElement { get; private set; }
        /// <summary>
        /// 开发者微信账号
        /// </summary>
        public string ToUserName { get; private set; }
        /// <summary>
        /// 发送方微信账号
        /// </summary>
        public string FromUserName { get; private set; }
        /// <summary>
        /// 微信消息创建时间
        /// </summary>
        public DateTime CreateTime { get; private set; }
        /// <summary>
        /// 微信消息类型
        /// </summary>
        public WeChatMsgType WeChatMsgType { get; private set; }
        /// <summary>
        /// 微信事件消息类型
        /// </summary>
        public WeChatEventType WeChatEventType { get; private set; }
        #endregion

        #region 构造函数

        public WeChatRequest()
        {

        }

        /// <summary>
        /// 构造函数，设置请求上下文的相关属性
        /// </summary>
        /// <param name="requestXml">请求消息xml</param>
        public WeChatRequest(XElement requestXml)
        {
            try
            {
                RequestXmlElement = requestXml;

                ToUserName = RequestXmlElement.Element("ToUserName").Value;
                FromUserName = RequestXmlElement.Element("FromUserName").Value;
                CreateTime = RequestXmlElement.Element("CreateTime").Value.IntStringToDateTime();
                switch (RequestXmlElement.Element("MsgType").Value)
                {
                    case "text":
                        WeChatMsgType = WeChatMsgType.Text;
                        break;
                    case "image":
                        WeChatMsgType = WeChatMsgType.Image;
                        break;
                    case "voice":
                        WeChatMsgType = WeChatMsgType.Voice;
                        break;
                    case "video":
                        WeChatMsgType = WeChatMsgType.Video;
                        break;
                    case "location":
                        WeChatMsgType = WeChatMsgType.Location;
                        break;
                    case "link":
                        WeChatMsgType = WeChatMsgType.Link;
                        break;
                    case "event":
                        WeChatMsgType = WeChatMsgType.Event;
                        switch (RequestXmlElement.Element("Event").Value.ToUpper())
                        {
                            case "SUBSCRIBE":
                                WeChatEventType = WeChatEventType.SubScribe;
                                break;
                            case "SCAN":
                                WeChatEventType = WeChatEventType.Scan;
                                break;
                            case "CLICK":
                                WeChatEventType = WeChatEventType.Click;
                                break;
                            case "LOCATION":
                                WeChatEventType = WeChatEventType.Location;
                                break;
                            case "VIEW":
                                WeChatEventType = WeChatEventType.View;
                                break;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 方法
        /// <summary>
        /// 获取请求消息T的类型实体对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns>类型T的实体对象</returns>
        public T GetRequestModel<T> () where T : class
        {
            if (RequestXmlElement == null)
            {
                return Activator.CreateInstance(typeof(T)) as T;
            }
            return RequestXmlElement.Get<T>();
        }

        #endregion
    }
}
