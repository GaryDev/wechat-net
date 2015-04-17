using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using WeChat.Framwork.Core.Entities;

namespace WeChat.Framwork.Core
{
    public class WeChatApplication
    {
        #region 字段/属性
        /// <summary>
        /// 微信上下文
        /// </summary>
        private WeChatContext _context;
        #endregion

        #region 构造函数

        public WeChatApplication()
        {

        }
        /// <summary>
        /// 构造函数，把请求消息封装到微信上下文
        /// </summary>
        /// <param name="requestXml">请求消息Xml</param>
        public WeChatApplication(XElement requestXml)
        {
            this._context = new WeChatContext(requestXml);
        }

        #endregion

        #region 方法

        /// <summary>
        /// 获取响应消息xml格式字符串
        /// </summary>
        /// <returns>响应消息xml格式字符串</returns>
        public string GetResponseXml()
        {
            try
            {
                WeChatController wechatController = WeChatControllerFactory.GetInstance().CreateWeChatController(this._context.Request);
                wechatController.ProcessWeChat(this._context);
            }
            catch (WeChatControllerNotFoundException e)
            {
                this._context.Response.Write(e.Message);
            }
            catch (Exception e)
            {
                WeChatTextMessageEntity ex = new WeChatTextMessageEntity
                {
                    ToUserName = this._context.Request.FromUserName,
                    Content = e.Message,
                    MsgType = WeChatMsgType.Text.ToString()
                };
                this._context.Response.Write(ex);
            }
            return this._context.Response.ResponseXml;
        }

        #endregion
    }
}
