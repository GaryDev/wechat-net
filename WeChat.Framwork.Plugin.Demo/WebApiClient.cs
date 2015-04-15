using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using WeChat.Framwork.Core;

namespace WeChat.Framwork.Plugin.Demo
{
    public class WebApiClient
    {
        private string _url;

        public WebApiClient(string url)
        {
            this._url = url;
        }

        public Dictionary<string, dynamic> GetResponse()
        {
            HttpWebRequest webRequest = HttpWebRequest.Create(_url) as HttpWebRequest;
            HttpWebResponse webResponse = webRequest.GetResponse() as HttpWebResponse;
            using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
            {
                string respResult = reader.ReadToEnd();
                Dictionary<string, dynamic> dict = WeChatHelper.DeserializeToDictionary(respResult);
                return dict;
            }
        }
    }
}
