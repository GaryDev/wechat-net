using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeChat.Framwork.Plugin.Demo
{
    public class WeChatTextFactory
    {
        private static WeChatTextFactory _factory;

        private WeChatTextFactory() { }

        public static WeChatTextFactory GetInstance()
        {
            if (_factory == null)
                _factory = new WeChatTextFactory();
            return _factory;
        }

        public WeChatTextAnalyzer CreateAnalyzer(string content)
        {
            if (content.Length < 3)
                throw new WeChatTextInvalidArgumentException("您的输入不正确, 请重新输入。");
            
            string code = content.Substring(0, 2).ToUpper();
            string data = content.Substring(2).Trim();
            
            WeChatTextAnalyzer analyzer = null;
            switch (code)
            {
                case "FY":
                    analyzer = new WeChatTextTranslation(data);
                    break;                
                default:
                    throw new WeChatTextInvalidArgumentException("敬请期待...");
            }
            return analyzer;
        }
    }
}
