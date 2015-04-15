using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeChat.Framwork.Plugin.Demo
{
    public class WeChatTextInvalidArgumentException : Exception
    {
        private string _message;

        public override string Message
        {
            get
            {
                if (string.IsNullOrEmpty(this._message))
                {
                    this._message = "出错啦!！！";
                }
                return this._message;
            }
        }

        public WeChatTextInvalidArgumentException() { }

        public WeChatTextInvalidArgumentException(string message) { this._message = message; }

    }
}
