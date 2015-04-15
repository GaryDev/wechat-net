using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeChat.Framwork.Core
{
    /// <summary>
    /// 微信消息枚举类型
    /// </summary>
    public enum WeChatMsgType
    {
        /// <summary>
        /// 文本消息
        /// </summary>
        Text,

        /// <summary>
        /// 图片消息
        /// </summary>
        Image,

        /// <summary>
        /// 语音消息
        /// </summary>
        Voice,

        /// <summary>
        /// 视频消息
        /// </summary>
        Video,

        /// <summary>
        /// 地理位置消息
        /// </summary>
        Location,

        /// <summary>
        /// 链接消息
        /// </summary>
        Link,

        /// <summary>
        /// 事件消息
        /// </summary>
        Event,

        /// <summary>
        /// 图文消息
        /// </summary>
        News
    }
}
