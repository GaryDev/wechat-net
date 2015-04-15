using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeChat.Framwork.Core.Entities
{
    public class WeChatLinkMessageEntity : WeChatMessageBaseEntity
    {
        public string Title { get; set; }
        
        public string Description { get; set; }
       
        public string Url { get; set; }
    }
}
