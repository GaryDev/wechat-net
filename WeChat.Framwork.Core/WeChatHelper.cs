using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;
using System.Web.Security;
using System.Xml.Linq;
using System.Reflection;
using System.Data;
using System.Web.Script.Serialization;
using System.Collections;
using WeChat.Framwork.Core.Entities;

namespace WeChat.Framwork.Core
{
    /// <summary>
    /// 微信通用处理工具类
    /// </summary>
    public class WeChatHelper
    {
        #region 字段/属性

        /// <summary>
        /// 微信自定义密钥常量
        /// </summary>
        private static readonly string Token;

        #endregion

        #region 构造函数

        static WeChatHelper()
        {
            Token = ConfigurationManager.AppSettings["WeChatToken"];
        }

        #endregion

        #region 方法

        #region 检查加密签名是否一致 - public static bool CheckSignature(string signature, string timestamp, string nonce)

        /// <summary>
        /// 检查加密签名是否一致
        /// </summary>
        /// <param name="signature">微信加密签名</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <returns></returns>
        public static bool CheckSignature(string signature, string timestamp, string nonce)
        {
            List<string> stringList = new List<string> {Token, timestamp, nonce};
            // 字典排序
            stringList.Sort();
            return Sha1Encrypt(string.Join("", stringList)) == signature;
        }

        #endregion

        #region 对字符串SHA1加密 - public static string Sha1Encrypt(string targetString)

        /// <summary>
        /// 对字符串SHA1加密
        /// </summary>
        /// <param name="targetString">源字符串</param>
        /// <returns>加密后的十六进制字符串</returns>
        private static string Sha1Encrypt(string targetString)
        {
            byte[] byteArray = Encoding.Default.GetBytes(targetString);
            HashAlgorithm hashAlgorithm = new SHA1CryptoServiceProvider();
            byteArray = hashAlgorithm.ComputeHash(byteArray);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte item in byteArray)
            {
                stringBuilder.AppendFormat("{0:x2}", item);
            }
            return stringBuilder.ToString();
        }

        #endregion

        #region 根据加密类型对字符串SHA1加密 - public static string Sha1Encrypt(string targetString, string encryptType)

        /// <summary>
        /// 根据加密类型对字符串SHA1加密
        /// </summary>
        /// <param name="targetString">源字符串</param>
        /// <param name="encryptType">加密类型：MD5/SHA1</param>
        /// <returns>加密后的字符串</returns>
        private static string Sha1Encrypt(string targetString, string encryptType)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(targetString, encryptType);
        }

        #endregion

        #region Json 字符串 转换为 Dictionary<string, dynamic>数据集合

        public static Dictionary<string, dynamic> DeserializeToDictionary(string json)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
            Dictionary<string, dynamic> values = javaScriptSerializer.Deserialize<Dictionary<string, dynamic>>(json);
            return values;
        }

        #endregion

        #endregion
    }


    /// <summary>
    /// .net扩展类，扩展相关类的方法
    /// </summary>
    public static class DotNetExtensions
    {
        /// <summary>
        /// 获取时间相对于1970.1.1日的时间戳（整型）
        /// </summary>
        /// <param name="dateTime">时间对象</param>
        /// <returns>时间戳（整型）</returns>
        public static int GetInt(this DateTime dateTime)
        {
            try
            {
                DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                return Convert.ToInt32((DateTime.Now - startTime).TotalSeconds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 根据相对于1970.1.1日的时间戳（整型）获取时间
        /// </summary>
        /// <param name="intString">时间戳（整型）</param>
        /// <returns>DateTime时间对象</returns>
        public static DateTime IntStringToDateTime(this string intString)
        {
            try
            {
                DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                long longTime = long.Parse(intString + "0000000");
                TimeSpan timeNow = new TimeSpan(longTime);
                return startTime.Add(timeNow);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 通过反射将XElement转换成类型T的实体对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="element">XElement对象</param>
        /// <returns>类型T的对象</returns>
        public static T Get<T>(this XElement element) where T : class
        {
            try
            {
                Type type = typeof(T);
                T entity = Activator.CreateInstance(type) as T;
                PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    XElement temp = element.Element(propertyInfo.Name);
                    if (temp != null)
                    {
                        propertyInfo.SetValue(entity, temp.Value, null);
                    }
                }
                return entity;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// 实体对象转换成XElement对象
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>XElement对象</returns>
        public static XElement GetXElement(this object entity, string root = "xml")
        {
            try
            {
                XElement element = new XElement(root);
                Type type = entity.GetType();
                PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    object value = propertyInfo.GetValue(entity, null);
                    if (value == null) continue;
                    
                    XElement temp = new XElement(propertyInfo.Name, value);
                    element.Add(temp);
                }
                //using (MemoryStream memoryStream=new MemoryStream())
                //{
                //    XmlSerializer xmlSerializer=new XmlSerializer(entity.GetType());
                //    xmlSerializer.Serialize(memoryStream,entity);

                //    memoryStream.Position = 0;
                //    using (StreamReader streamReader = new StreamReader(memoryStream))
                //    {
                //        string xml = streamReader.ReadToEnd();
                //        element.Add(XElement.Parse(xml));
                //    }
                //    element.Add(XElement.Load(memoryStream));
                //}
                return element;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        
    }
}
