using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeChat.Framwork.Core.Entities
{
    public class WeChatNewsMessageEntity : WeChatMessageBaseEntity
    {
        public int ArticleCount { get; set; }

        public List<Article> Articles { get; set; }

    }

    public class Article
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string PicUrl { get; set; }
        public string Url { get; set; }
    }
}
