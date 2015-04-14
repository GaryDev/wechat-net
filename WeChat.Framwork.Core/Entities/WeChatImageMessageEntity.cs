using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace WeChat.Framwork.Core.Entities
{
    /// <summary>
    /// 图片消息实体类
    /// </summary>
    public class WeChatImageMessageEntity : WeChatMessageBaseEntity
    {
        /// <summary>
        /// 图片链接
        /// </summary>
        public string PicUrl { get; set; }
        /// <summary>
        /// 图片消息媒体id
        /// </summary>
        public string MediaId { get; set; }

    }
}
