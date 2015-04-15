using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using System.IO;
using WeChat.Framwork.Core;
using WeChat.Framwork.Core.Entities;
using System.Collections;

namespace WeChat.Framwork.Plugin.Demo
{
    public class WeChatTextWeather : WeChatTextAnalyzer
    {
        private readonly string URL = "http://api.map.baidu.com/telematics/v3/weather?location={0}&output=json&ak=ECe3698802b9bf4457f0e01b544eb6aa";

        private string _city;

        public WeChatTextWeather(string city)
        {
            this._city = city;
        }

        public WeChatMessageBaseEntity GetResponse(WeChatTextMessageEntity request)
        {
            Dictionary<string, dynamic> result = QueryWeather();
            if (result["error"] == 0)
            {
                int hour = DateTime.Now.Hour;
                WeChatNewsMessageEntity news = new WeChatNewsMessageEntity
                {
                    ToUserName = request.FromUserName,
                    MsgType = WeChatMsgType.News.ToString().ToLower()
                };
                dynamic weather = result["results"][0];
                List<Article> articles = new List<Article>();
                articles.Add(new Article { Title = weather["currentCity"] + "天气预报" });
                dynamic[] weatherday = (weather["weather_data"] as ArrayList).ToArray();
                for (int i = 0; i < weatherday.Length; i++)
                {
                    articles.Add(new Article {
                        Title = weatherday[i]["date"] + "\n"
                                + weatherday[i]["weather"] + " "
                                + weatherday[i]["wind"] + " "
                                + weatherday[i]["temperature"],
                        PicUrl = hour >= 6 && hour < 18 ? weatherday[i]["dayPictureUrl"] : weatherday[i]["nightPictureUrl"]
                    });
                }
                news.ArticleCount = articles.Count;
                news.Articles = articles;
                return news;
            }
            else
            {
                return new WeChatTextMessageEntity
                {
                    ToUserName = request.FromUserName,
                    Content = string.Format("抱歉，没有查到{0}的天气信息！", _city),
                    MsgType = request.MsgType
                };
            }
        }

        private Dictionary<string, dynamic> QueryWeather()
        {
            /*
             * 例子：
             * {
	            "error":0,
	            "status":"success",
	            "date":"2015-04-15",
	            "results":[
		            {
			            "currentCity":"北京",
			            "pm25":"174",
			            "index":[
				            {"title":"穿衣","zs":"舒适","tipt":"穿衣指数","des":"建议着长袖T恤、衬衫加单裤等服装。年老体弱者宜着针织长袖衬衫、马甲和长裤。"},
				            {"title":"洗车","zs":"不宜","tipt":"洗车指数","des":"不宜洗车，未来24小时内有雨，如果在此期间洗车，雨水和路上的泥水可能会再次弄脏您的爱车。"},
				            {"title":"旅游","zs":"一般","tipt":"旅游指数","des":"有降水，温度适宜，风较大，外出旅游请避开降雨时段并注意防雷防风。"},
				            {"title":"感冒","zs":"较易发","tipt":"感冒指数","des":"虽然温度适宜但风力较大，仍较易发生感冒，体质较弱的朋友请注意适当防护。"},
				            {"title":"运动","zs":"较不宜","tipt":"运动指数","des":"有降水，且风力很强，推荐您选择室内运动；若坚持户外运动，请注意保暖并携带雨具。"},
				            {"title":"紫外线强度","zs":"弱","tipt":"紫外线强度指数","des":"紫外线强度较弱，建议出门前涂擦SPF在12-15之间、PA+的防晒护肤品。"}
			            ],
			            "weather_data":[
				            {"date":"周三 04月15日 (实时：21℃)",
				            "dayPictureUrl":"http://api.map.baidu.com/images/weather/day/leizhenyu.png",
				            "nightPictureUrl":"http://api.map.baidu.com/images/weather/night/duoyun.png",
				            "weather":"雷阵雨转多云",
				            "wind":"北风6-7级",
				            "temperature":"26 ~ 12℃"},
				            {"date":"周四",
				            "dayPictureUrl":"http://api.map.baidu.com/images/weather/day/duoyun.png",
				            "nightPictureUrl":"http://api.map.baidu.com/images/weather/night/qing.png",
				            "weather":"多云转晴",
				            "wind":"北风4-5级",
				            "temperature":"20 ~ 7℃"},
				            {"date":"周五",
				            "dayPictureUrl":"http://api.map.baidu.com/images/weather/day/qing.png",
				            "nightPictureUrl":"http://api.map.baidu.com/images/weather/night/yin.png",
				            "weather":"晴转阴",
				            "wind":"微风",
				            "temperature":"22 ~ 10℃"},
				            {"date":"周六",
				            "dayPictureUrl":"http://api.map.baidu.com/images/weather/day/yin.png",
				            "nightPictureUrl":"http://api.map.baidu.com/images/weather/night/duoyun.png",
				            "weather":"阴转多云",
				            "wind":"微风",
				            "temperature":"20 ~ 10℃"}
			            ]
		            }
	            ]
             *}
             */
            string url = string.Format(URL, HttpUtility.UrlEncode(_city, Encoding.UTF8));
            WebApiClient client = new WebApiClient(url);
            return client.GetResponse();
        }
    }
}
