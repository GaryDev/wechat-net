using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using WeChat.Framwork.Core.Entities;
using System.Configuration;

namespace WeChat.Framwork.Core
{
    /// <summary>
    /// 获取微信消息处理控制器工厂实体类（单例模式）
    /// </summary>
    public class WeChatControllerFactory
    {
        #region 字段/属性

        /// <summary>
        /// 控制器工厂单例
        /// </summary>
        private static WeChatControllerFactory _wechatControllerFactory;

        /// <summary>
        /// 插件集合目录
        /// </summary>
        private static readonly string PluginFolder;

        /// <summary>
        /// 实现IWeiXinHandler接口的类插件类型集合
        /// </summary>
        private static readonly List<Type> PluginTypes;

        #endregion

        #region 构造函数

        static WeChatControllerFactory()
        {
            // 插件目录
            PluginFolder = AppDomain.CurrentDomain.BaseDirectory + "bin\\" + ConfigurationManager.AppSettings["WeChatPluginFolder"];

            PluginTypes = new List<Type>();

            // 获取插件目录下名称匹配为WeChat.*.dll所有程序集路径
            string[] pathArray = Directory.GetFiles(PluginFolder, "WeChat.*.dll");

            foreach (string path in pathArray)
            {
                Assembly assembly = Assembly.LoadFile(path);
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    // 类型是否继承WeChatController接口且不是抽象类
                    if (!typeof(WeChatController).IsAssignableFrom(type) || type.IsAbstract) continue;
                    PluginTypes.Add(type);
                }
            }
        }

        private WeChatControllerFactory()
        {

        }

        #endregion

        #region 方法
        /// <summary>
        /// 获取一个实例
        /// </summary>
        /// <returns>工厂类对象</returns>
        public static WeChatControllerFactory GetInstance()
        {
            if (_wechatControllerFactory == null)
            {
                _wechatControllerFactory = new WeChatControllerFactory(); ;
            }
            return _wechatControllerFactory;
        }
        /// <summary>
        /// 根据消息类型创建消息控制器
        /// </summary>
        /// <param name="request">微信请求上下文</param>
        /// <returns>实现IWeiXinHandler微信消息处理控制器</returns>
        public WeChatController CreateWeChatController(WeChatRequest request)
        {
            WeChatController wechatController = null;
            try
            {
                foreach (Type pluginType in PluginTypes)
                {
                    WeChatControllerAdapter wechatHandlerType = Activator.CreateInstance(pluginType) as WeChatControllerAdapter;

                    if (wechatHandlerType == null) continue;

                    if (request.WeChatMsgType != WeChatMsgType.Event)
                    {
                        if (wechatHandlerType.WeChatMsgType == request.WeChatMsgType)
                        {
                            wechatController = wechatHandlerType;
                            break;
                        }
                    }
                    else
                    {
                        if (wechatHandlerType.WeChatEventType == request.WeChatEventType)
                        {
                            wechatController = wechatHandlerType;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (wechatController == null)
                throw new WeChatControllerNotFoundException
                    {
                        ExceptionMessage = new WeChatTextMessageEntity
                            {
                                ToUserName = request.FromUserName,
                                Content = "控制器未找到异常:此消息类型控制器可能未实现!",
                                MsgType = WeChatMsgType.Text.ToString().ToLower()
                            }.GetXElement().ToString()
                    };

            return wechatController;
        }

        #endregion
    }
}
