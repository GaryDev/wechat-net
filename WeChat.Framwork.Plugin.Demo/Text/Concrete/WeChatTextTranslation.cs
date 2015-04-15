using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using WeChat.Framwork.Core;
using System.Collections;

namespace WeChat.Framwork.Plugin.Demo
{
    public class WeChatTextTranslation : WeChatTextAnalyzer
    {
        private readonly string URL = "http://fanyi.youdao.com/openapi.do?keyfrom=24979870&key=1831130904&type=data&doctype=json&version=1.1&q={0}";
        private string _word;
        
        public WeChatTextTranslation(string word)
        {
            this._word = word;
        }
        
        public string GetResponseMessage()
        {
            string resp = string.Empty;
            Dictionary<string, dynamic> result = DoTranslation();
            if (result["errorCode"] == 0)
            {
                object[] translations = (result["translation"] as ArrayList).ToArray();
                object[] explains = (result["basic"]["explains"] as ArrayList).ToArray();
                object[] web = (result["web"] as ArrayList).ToArray();
                object[] webexp = new object[] { };
                if (web.Length > 0)
                {
                    dynamic value = web[0];
                    webexp = (value["value"] as ArrayList).ToArray();
                }
                resp = string.Format("{0}:\n{1}\n{2}\n网络释义：\n{3}",
                    result["query"],
                    string.Join(", ", translations),
                    string.Join(", ", explains),
                    string.Join(", ", webexp));
            }
            else
            {
                resp = string.Format("对不起，您输入的单词{0}无法翻译，请检查拼写", _word);
            }
            return resp;
        }

        private Dictionary<string, dynamic> DoTranslation()
        {
            string url = string.Format(URL, HttpUtility.UrlEncode(_word, Encoding.UTF8));
            HttpWebRequest webRequest = HttpWebRequest.Create(url) as HttpWebRequest;
            HttpWebResponse webResponse = webRequest.GetResponse() as HttpWebResponse;
            using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
            {
                /* 例子：
                 * {
                 * "translation":["测试"],
                 * "basic":{"us-phonetic":"tɛst",
                 *          "phonetic":"test",
                 *          "uk-phonetic":"test",
                 *          "explains":["n. 试验；检验","vt. 试验；测试","vi. 试验；测试","n. (Test)人名；(英)特斯特"]
                 *          },
                 * "query":"test",
                 * "errorCode":0,
                 * "web":[{"value":["测试","检验","测验"],"key":"test"},
                 *        {"value":["测试员","测试工程师","软件测试工程师"],"key":"Test Engineer"},
                 *        {"value":["硬度试验","硬度测试","硬度实验"],"key":"hardness test"}]
                 *}*/
                string respResult = reader.ReadToEnd();
                Dictionary<string, dynamic> dict = WeChatHelper.DeserializeToDictionary(respResult);
                return dict;
            }
        }
    }

}
