using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeChat.Framwork.Core
{
    /// <summary>
    /// 微信消息请求处理控制器未找到异常实体类
    /// </summary>
    public class WeChatControllerNotFoundException : Exception
    {
        private string _message;
        public override string Message
        {
            get
            {
                if (string.IsNullOrEmpty(this._message))
                {
                    this._message = "控制器未找到异常:此消息类型控制器可能未实现!";
                }
                return this._message;
            }
        }
        /// <summary>
        /// 异常信息
        /// </summary>
        public string ExceptionMessage
        {
            set { this._message = value; }
        }
    }
}
