<%@ WebHandler Language="C#" Class="WeChatHandler" %>

using System;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using WeChat.Framwork.Core;

public class WeChatHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        
        string httpMethod = context.Request.HttpMethod.ToUpper();
        object responseXml = string.Empty;

        try
        {
            switch (httpMethod)
            {
                case "GET":
                    string signature = context.Request.QueryString["signature"], // 微信加密签名
                           timestamp = context.Request.QueryString["timestamp"], // 时间戳
                           nonce = context.Request.QueryString["nonce"], // 随机数
                           echostr = context.Request.QueryString["echostr"];// 随机字符串
                    if (!string.IsNullOrEmpty(signature) &&
                        !string.IsNullOrEmpty(timestamp) &&
                        !string.IsNullOrEmpty(nonce) &&
                        !string.IsNullOrEmpty(echostr))
                    {
                        if (WeChatHelper.CheckSignature(signature, timestamp, nonce))
                        {
                            responseXml = echostr;
                        }
                    }
                    break;
                case "POST":
                    XElement requestXml;
                    if (!string.IsNullOrEmpty(context.Request.Form["wechatMsg"]))
                    {
                        // 处理test.html页面测试的请求，并返回信息
                        requestXml = XElement.Parse(context.Request.Form["wechatMsg"]);                        
                    }
                    else
                    {
                        // 解析微信请求
                        requestXml = XElement.Load(context.Request.InputStream);
                    }
                    WeChatApplication app = new WeChatApplication(requestXml);
                    responseXml = app.GetResponseXml();
                    break;
            }
        }
        catch (XmlException ex)
        {
            responseXml = string.Format("xml解析异常:{0}", ex.Message);
        }
        catch (WeChatControllerNotFoundException ex)
        {
            responseXml = ex.Message;
        }
        catch (Exception ex)
        {
            responseXml = ex.Message;
        }

        context.Response.Clear();
        context.Response.Charset = "UTF-8";
        context.Response.Write(responseXml);
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}